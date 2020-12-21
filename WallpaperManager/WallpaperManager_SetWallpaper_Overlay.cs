using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using SharpDX.DirectWrite;
using WallpaperManager.ApplicationData;

namespace WallpaperManager
{
    //? Overlay/Direct Draw implementation of SetWallpaper
    public partial class WallpaperManager : Form
    {
        #region Overlay Wallpaper
        // Derived from: https://www.codeproject.com/Articles/856020/Draw-Behind-Desktop-Icons-in-Windows-plus
        private void SetOverlayWallpaper()
        {
            //?-----Fetch the Program window-----
            IntPtr program = Win32.FindWindow("Program");

            //?-----Spawn a WorkerW behind the desktop icons (If it is already there, nothing happens)-----
            IntPtr result = IntPtr.Zero;
            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            Win32.SendMessageTimeout(program,
                0x052C,
                new IntPtr(0),
                IntPtr.Zero,
                Win32.SendMessageTimeoutFlags.SMTO_NORMAL,
                1000,
                out result);

            //?-----Find the Window that's underneath the desktop icons-----
            // Spy++ output
            // .....
            // 0x00010190 "" WorkerW
            //   ...
            //   0x000100EE "" SHELLDLL_DefView
            //     0x000100F0 "FolderView" SysListView32
            // 0x00100B8A "" WorkerW       <-- This is the WorkerW instance we are after!
            // 0x000100EC "Program Manager" Progman

            IntPtr workerw = IntPtr.Zero;

            // We enumerate all Windows, until we find one, that has the SHELLDLL_DefView 
            // as a child. 
            // If we found that window, we take its next sibling and assign it to workerw.
            Win32.EnumWindows(new Win32.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = Win32.FindWindowEx(tophandle,
                    IntPtr.Zero,
                    "SHELLDLL_DefView",
                    string.Empty);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = Win32.FindWindowEx(IntPtr.Zero,
                        tophandle,
                        "WorkerW",
                        string.Empty);
                }

                return true;
            }), IntPtr.Zero);


            //?-----Check if any of the used images are animated-----
            bool animatedImagePresent = false;
            bool[] isMonitorAnimated = new bool[PathData.ActiveWallpapers.Length];
            for (var i = 0; i < PathData.ActiveWallpapers.Length; i++)
            { 
                if (IsAnimatedGif(PathData.ActiveWallpapers[i]))
                {
                    isMonitorAnimated[i] = true;

                    if (!animatedImagePresent) // this should only be set once
                    {
                        animatedImagePresent = true;
                    }
                }
            }

            animatedWallpaperThreads = new BackgroundWorker[PathData.ActiveWallpapers.Length];
            if (animatedImagePresent)
            {
                Debug.WriteLine("Activating Animated Wallpaper");
                for (int i = 0; i < isMonitorAnimated.Length; i++)
                {
                    if (isMonitorAnimated[i])
                    {
                        Debug.WriteLine("Index of Animated Wallpaper: " + i);
                        int currentFrame = 0;
                        animatedWallpaperThreads[i] = new BackgroundWorker();
                        animatedWallpaperThreads[i].WorkerSupportsCancellation = true;
                        
                        int index = i; // prevents the index from changing within the thread
                        animatedWallpaperThreads[i].DoWork += (sender, e) =>
                        {
                            BackgroundWorker activeWorker = animatedWallpaperThreads[index];
                            List<byte[]> frames = EnumerateFrames(PathData.ActiveWallpapers[index]);
                            int totalFrames = frames.Count;

                            while (currentFrame < totalFrames)
                            {
                                if (activeWorker != animatedWallpaperThreads[index])
                                {
                                    Debug.WriteLine("Cancelling Wallpaper");
                                    e.Cancel = true;
                                    break;
                                }

                                IntPtr animatedDc = Win32.GetDCEx(workerw, IntPtr.Zero, (Win32.DeviceContextValues)0x403);
                                using (Graphics g = Graphics.FromHdc(animatedDc))
                                {
                                    //?Debug.WriteLine("currentFrame: " + currentFrame);
                                    DoWallpaperGraphics(g, ConvertBytesToImage(frames[currentFrame]),
                                        MonitorData.Screens[index], MonitorData.MonitorXAdjustment, MonitorData.MinMonitorY);

                                    currentFrame = currentFrame != totalFrames - 1 ? currentFrame + 1 : 0; // loops the current frame if the last frame is reached
                                }

                                Thread.Sleep(1000/60); //TODO Make the frame-rate customizable | 1000/60 is 60fps
                            }
                        };
                        animatedWallpaperThreads[i].RunWorkerAsync();
                    }
                }

                Debug.WriteLine("Threading Done");
            }

            //?-----Get DC & Draw to the Desktop-----
            // Get the Device Context of the WorkerW
            IntPtr staticDc = Win32.GetDCEx(workerw, IntPtr.Zero, (Win32.DeviceContextValues)0x403);
            if (staticDc != IntPtr.Zero)
            {
                //? Draw Wallpaper
                DrawStaticWallpaper(ref staticDc, animatedImagePresent, isMonitorAnimated); // since static images only need to be drawn once they can be combined into one graphics object

                // make sure to release the device context after use.
                Win32.ReleaseDC(workerw, staticDc); // allows for the drawing of the wallpaper
            }
        }

        private void DrawStaticWallpaper(ref IntPtr dc, bool animatedImagePresent, bool[] isMonitorAnimated)
        {
            //? workerw is a hWnd btw
            Debug.WriteLine("Drawing Static Wallpaper");

            Image[] images = new Image[PathData.ActiveWallpapers.Length];

            if (RetreiveImage(ref images)) // attempts to load images, will prevent the program from crashing if memory overloads and instead just not load the wallpaper
            {
                using (Graphics g = Graphics.FromHdc(dc)) // Draw Background
                {
                    for (int i = 0; i < MonitorData.Screens.Length; i++)
                    //?for (int i = 0; i < PathData.ActiveWallpapers.Length; i++)
                    {
                        //?Image curImage = images[i];

                        if (!animatedImagePresent || !isMonitorAnimated[i]) // This process only draws static wallpapers
                        {
                            DoWallpaperGraphics(g, new Bitmap(PathData.ActiveWallpapers[i]), 
                                MonitorData.Screens[i], MonitorData.MonitorXAdjustment, MonitorData.MinMonitorY);
                        }
                    }

                    foreach (Image image in images) image.Dispose();
                }
            }
        }

        void DoWallpaperGraphics(Graphics g, Bitmap bitmap, Screen monitor, int MonitorXAdjustment, int minMonitorY)
        {
            //? Bitmap is disposed at the end of this method
            int MonitorWidth = monitor.Bounds.Width;
            int MonitorHeight = monitor.Bounds.Height;
            int MonitorX = monitor.Bounds.X + MonitorXAdjustment;
            int MonitorY = monitor.Bounds.Y;
            int MonitorYAdjustment = MonitorY - minMonitorY;

            int ImageWidth = bitmap.Width;
            int ImageHeight = bitmap.Height;

            switch (WallpaperStyle)
            {
                case PictureStyle.Fill: //TODO Implement a cropping feature for fill so that it doesn't extend into other monitors

                    if (ImageWidth > ImageHeight)
                    {
                        float sizeRatio = (float) ImageWidth / MonitorWidth;
                        g.DrawImage(bitmap, MonitorX, MonitorYAdjustment, MonitorWidth * sizeRatio, MonitorHeight);
                    }
                    else
                    {
                        float sizeRatio = (float) ImageWidth / MonitorHeight;
                        g.DrawImage(bitmap, MonitorX, MonitorYAdjustment, MonitorWidth, MonitorHeight * sizeRatio);
                    }

                    break;

                case PictureStyle.Stretch:
                    g.DrawImage(bitmap, MonitorX, MonitorYAdjustment, MonitorWidth, MonitorHeight);
                    break;

                case PictureStyle.Zoom:

                    // it's best to check with ratios rather than the exact ImageHeight & ImageWidth in order to avoid scaling out of the monitor
                    float widthRatio = (float) ImageWidth / MonitorWidth;
                    float heightRatio = (float) ImageHeight / MonitorHeight;

                    int TaskBarHeight = 40;

                    using (Brush brush = new SolidBrush(Color.Black)) // used to fill in empty spaces with black, otherwise you'd still see the previous image
                    {
                        // if both are equal heightRatio should be preferred
                        if (heightRatio >= widthRatio) // scale image to match the monitor height and let the width have gaps 
                        {
                            float adjustedHeight = MonitorHeight - TaskBarHeight;
                            float adjustedWidth = ImageWidth * ((float) MonitorHeight / ImageHeight) * (adjustedHeight / MonitorHeight);
                            //float adjustedWidth = MonitorWidth * ((float) ImageWidth / ImageHeight);

                            float widthDifference = MonitorWidth - adjustedWidth;
                            float leftGap = widthDifference / 2;
                            float xPos = MonitorX + widthDifference - leftGap;

                            //? the +1 and -2 values prevent the previous wallpaper from still being visible by 1 pixel, which is a bit jarring
                            //? the rectangle must be drawn first to prevent weird glitching from lag
                            g.FillRectangle(brush, MonitorX, MonitorYAdjustment, leftGap + 1, MonitorHeight); // draws left-side border
                            g.FillRectangle(brush, xPos + adjustedWidth - 2, MonitorYAdjustment, leftGap + 2, MonitorHeight); // draws right-side border

                            g.DrawImage(bitmap, xPos, MonitorYAdjustment, adjustedWidth, adjustedHeight);
                        }
                        else // scale image to match the monitor width and let the height have gaps
                        {
                            float adjustedHeight = ImageHeight * ((float) MonitorWidth / ImageWidth);

                            float heightDifference = MonitorHeight - adjustedHeight;
                            float bottomGap = heightDifference / 2;
                            float yPos = heightDifference - bottomGap;

                            //? the +1 and -2 values prevent the previous wallpaper from still being visible by 1-2 pixels, which is a bit jarring
                            //? the rectangle must be drawn first to prevent weird glitching from lag
                            g.FillRectangle(brush, MonitorX, MonitorYAdjustment, MonitorWidth, heightDifference + 1); // draws top-side border
                            g.FillRectangle(brush, MonitorX, yPos + adjustedHeight + MonitorYAdjustment - 2, MonitorWidth, bottomGap + 2); // draws bottom-side border

                            g.DrawImage(bitmap, MonitorX, yPos + MonitorYAdjustment, MonitorWidth, adjustedHeight);
                        }
                    }

                    break;
            }

            bitmap.Dispose();
        } //? End of void DoWallpaperGraphics()

        public bool IsAnimatedGif(string filepath) => GetFrameCount(filepath) > 1;

        public int GetFrameCount(string filepath)
        {
            using (Bitmap bitmap = new Bitmap(filepath))
            {
                FrameDimension dimensions = new FrameDimension(bitmap.FrameDimensionsList[0]);
                return bitmap.GetFrameCount(dimensions);
            }
        }

        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }
        #endregion

        //Derived from: https://www.codeproject.com/Tips/659027/How-to-Split-a-GIF-Frame-By-Frame-and-Save-Them-to
        #region Frame Handler
        private List<byte[]> EnumerateFrames(string imagePath)
        {
            try
            {
                //Make sure the image exists
                if (!File.Exists(imagePath))
                {
                    throw new FileNotFoundException("Unable to locate " + imagePath);
                }

                Dictionary<Guid, ImageFormat> guidToImageFormatMap = new Dictionary<Guid, ImageFormat>()
                {
                    {ImageFormat.Bmp.Guid,  ImageFormat.Bmp},
                    {ImageFormat.Gif.Guid,  ImageFormat.Png},
                    {ImageFormat.Icon.Guid, ImageFormat.Png},
                    {ImageFormat.Jpeg.Guid, ImageFormat.Jpeg},
                    {ImageFormat.Png.Guid,  ImageFormat.Png}
                };

                List<byte[]> tmpFrames = new List<byte[]>() { };

                using (Image img = Image.FromFile(imagePath, true))
                {
                    //Check the image format to determine what
                    //format the image will be saved to the 
                    //memory stream in
                    ImageFormat imageFormat = null;
                    Guid imageGuid = img.RawFormat.Guid;

                    foreach (KeyValuePair<Guid, ImageFormat> pair in guidToImageFormatMap)
                    {
                        if (imageGuid == pair.Key)
                        {
                            imageFormat = pair.Value;
                            break;
                        }
                    }

                    if (imageFormat == null)
                    {
                        throw new NoNullAllowedException("Unable to determine image format");
                    }

                    //Get the frame count
                    FrameDimension dimension = new FrameDimension(img.FrameDimensionsList[0]);
                    int frameCount = img.GetFrameCount(dimension);

                    //Step through each frame
                    for (int i = 0; i < frameCount; i++)
                    {
                        //Set the active frame of the image and then 
                        //write the bytes to the tmpFrames array
                        img.SelectActiveFrame(dimension, i);
                        using (MemoryStream ms = new MemoryStream())
                        {

                            img.Save(ms, imageFormat);
                            tmpFrames.Add(ms.ToArray());
                        }
                    }
                }

                return tmpFrames;

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error type: " + ex.GetType().ToString() + "\n" +
                    "Message: " + ex.Message,
                    "Error in " + MethodBase.GetCurrentMethod().Name
                    );
            }

            return null;
        }

        //Now we should have a List of byte arrays if everything went well. 
        //Now, how to convert the bytes back to an image.

        //To convert the byte array back to an image, recompile the byte array.
        //This method returns null if it failed.
        private Bitmap ConvertBytesToImage(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;
            }

            try
            {
                //Read bytes into a MemoryStream
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    //Recreate the frame from the MemoryStream
                    using (Bitmap bmp = new Bitmap(ms))
                    {
                        return (Bitmap)bmp.Clone();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error type: " + ex.GetType().ToString() + "\n" +
                    "Message: " + ex.Message,
                    "Error in " + MethodBase.GetCurrentMethod().Name
                    );
            }

            return null;
        }
        #endregion
    }
}
