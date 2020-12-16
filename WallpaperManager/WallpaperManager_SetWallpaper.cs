using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using WallpaperManager.ApplicationData;

namespace WallpaperManager
{
    public partial class WallpaperManager : Form
    {
        private Thread setWallpaperThread;
        private BackgroundWorker[] animatedWallpaperThreads;
        private bool[] activatedWallpaperThreads;

        private void SetWallpaper()
        {
            /* TODO
            SetOverlayWallpaper();
            return;
            */

            setWallpaperThread = new Thread(() =>
            {
                // Check if all potential wallpapers exist
                foreach (string wallpaperPath in PathData.ActiveWallpapers)
                {
                    if (!File.Exists(wallpaperPath))
                    {
                        MessageBox.Show("The following file path does not exist: [" + wallpaperPath + "] | Wallpaper will not be changed");
                        return;
                    }
                }

                for (int i = 0; i < PathData.ActiveWallpapers.Length; i++)
                {
                    string wallpaperPath = PathData.ActiveWallpapers[i];
                    string wallpaperName = new FileInfo(wallpaperPath).Name; // pathless string of file name

                    if (WallpaperData.ContainsImage(wallpaperPath))
                    {
                        wallpaperMenuItems[i].Text = i + 1 + " | R: " + WallpaperData.GetImageRank(wallpaperPath) + " | " + wallpaperName;
                    }
                    else
                    {
                        wallpaperMenuItems[i].Text = i + 1 + " | [NOT FOUND]" + wallpaperName;
                    }
                }

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

                if (MonitorData.Screens.Length > 1) // For multi-monitor setups
                {
                    key.SetValue(@"WallpaperStyle", 0.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());

                    DrawBackground();
                    SystemParametersInfo(SetDeskWallpaper, 0, PathData.ActiveWallpaperImage, UpdateIniFile | SendWinIniChange);
                }
                else
                {
                    #region Style Settings
                    switch (WallpaperStyle)
                    {
                        case PictureStyle.Fill:
                            key.SetValue(@"WallpaperStyle", 10.ToString());
                            key.SetValue(@"TileWallpaper", 0.ToString());
                            break;

                        case PictureStyle.Fit:
                            key.SetValue(@"WallpaperStyle", 6.ToString());
                            key.SetValue(@"TileWallpaper", 0.ToString());
                            break;

                        case PictureStyle.Span:
                            key.SetValue(@"WallpaperStyle", 22.ToString());
                            key.SetValue(@"TileWallpaper", 0.ToString());
                            break;

                        case PictureStyle.Stretch:
                            key.SetValue(@"WallpaperStyle", 2.ToString());
                            key.SetValue(@"TileWallpaper", 0.ToString());
                            break;

                        case PictureStyle.Tile:
                            key.SetValue(@"WallpaperStyle", 0.ToString());
                            key.SetValue(@"TileWallpaper", 1.ToString());
                            break;

                        case PictureStyle.Center:
                            key.SetValue(@"WallpaperStyle", 0.ToString());
                            key.SetValue(@"TileWallpaper", 0.ToString());
                            break;
                    }
                    #endregion

                    SystemParametersInfo(SetDeskWallpaper, 0, PathData.ActiveWallpapers[0], UpdateIniFile | SendWinIniChange);
                }

                IntPtr result = IntPtr.Zero;
                SendMessageTimeout(FindWindow("Program", IntPtr.Zero), 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 500, out result);
            });

            if (setWallpaperThread.IsAlive) setWallpaperThread.Abort();
            setWallpaperThread.Start();
        }

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

            //?-----Get DC & Draw to the Desktop-----
            // Get the Device Context of the WorkerW
            IntPtr dc = Win32.GetDCEx(workerw, IntPtr.Zero, (Win32.DeviceContextValues)0x403);
            if (dc != IntPtr.Zero)
            {
                //? Draw Wallpaper
                DrawWallpaper(ref dc, workerw);
                // make sure to release the device context after use.
                Win32.ReleaseDC(workerw, dc);//? Done in Draw Wallpaper as well
            }
        }
        private void DrawWallpaper(ref IntPtr dc, IntPtr workerw)
        {
            //? workerw is a hWnd btw

            Image[] images = new Image[PathData.ActiveWallpapers.Length];
            activatedWallpaperThreads = new bool[PathData.ActiveWallpapers.Length];
            //! you're either going to have to get around instantiating this here or not instantiate it here at all
            animatedWallpaperThreads = new BackgroundWorker[PathData.ActiveWallpapers.Length];
            for (var i = 0; i < animatedWallpaperThreads.Length; i++) 
            {
                animatedWallpaperThreads[i] = new BackgroundWorker(); // initializes all BackgroundWorkers
                animatedWallpaperThreads[i].WorkerSupportsCancellation = true;
            }

            if (RetreiveImage(ref images)) // attempts to load images, will prevent the program from crashing if memory overloads and instead just not load the wallpaper
            {
                // Set up monitor bounds & adjustments
                int TotalMonitorWidth = 0;
                int MaxMonitorHeight = 0;
                int MonitorXAdjustment = 0;
                int minMonitorY = int.MaxValue;

                foreach (Screen monitor in MonitorData.Screens)
                {
                    if (monitor.Bounds.X < 0) // used to prevent wallpapers from being drawn off the screen
                    {
                        MonitorXAdjustment += Math.Abs(monitor.Bounds.X);
                    }

                    if (monitor.Bounds.Y < minMonitorY)
                    {
                        minMonitorY = monitor.Bounds.Y;
                    }

                    TotalMonitorWidth += monitor.Bounds.Width;
                    MaxMonitorHeight = Math.Max(MaxMonitorHeight, monitor.Bounds.Height);
                }

                // Draw Background
                for (int i = 0; i < MonitorData.Screens.Length; i++)
                {
                    Screen monitor = MonitorData.Screens[i];
                    Image curImage = images[i];

                    WallpaperGraphicsInfo graphicsInfo = new WallpaperGraphicsInfo(dc, GetFrameCount(PathData.ActiveWallpapers[i]));
                    if (IsAnimatedGif(PathData.ActiveWallpapers[i]))
                    {
                        //? Is gif, start thread
                        Debug.WriteLine("Only drawing i == 0 to speed up testing");
                        if (i > 0) return; //! THIS IS USED TO END THE WALLPAPER CHANGING PROCESS EARLY
                        //if (animatedWallpaperThreads[i] != null && animatedWallpaperThreads[i].IsBusy) animatedWallpaperThreads[i].CancelAsync();
                        //if (animatedWallpaperThreads[i]) animatedWallpaperThreads[i].CancelAsync();
                        //Debug.WriteLine("Is busy: " + animatedWallpaperThreads[i].IsBusy);

                        //animatedWallpaperThreads[i] = new Thread(DoWallpaperGraphics);
                        //animatedWallpaperThreads[i].IsBackground = true; // ensures that the thread ends upon closing the application
                        //animatedWallpaperThreads[i].Start();
                        animatedWallpaperThreads[i].DoWork += DoWallpaperGraphics;
                        animatedWallpaperThreads[i].RunWorkerAsync();
                    }
                    else
                    {
                        //? Not a gif, no thread
                        Debug.WriteLine("Only drawing i == 0 to speed up testing");
                        if (i > 0) return; //! THIS IS USED TO END THE WALLPAPER CHANGING PROCESS EARLY
                        DoWallpaperGraphics(null, null);
                    }

                    void DoWallpaperGraphics(object sender, DoWorkEventArgs e) // this was made into a method for multi-threading purposes
                    {
                        //(IntPtr, int) _parameters = ((IntPtr, int))parameters;
                        //WallpaperGraphicsInfo _graphicsInfo = graphicsInfo;
                        IntPtr _dc = graphicsInfo.dc;
                        int totalFrames = graphicsInfo.frame;

                        int currentFrame = 1; // all images start at 1
                        //lock (_dc)
                        //{
                        Debug.WriteLine("totalFrames: " + totalFrames);

                        using (Graphics g = Graphics.FromHdc(_dc))
                        {
                            Debug.WriteLine("Graphics gotten");
                            //g.Clear(Color.Black);
                            using (Bitmap bitmap = new Bitmap(PathData.ActiveWallpapers[i]))
                            {
                                /*
                                Bitmap bitmap2 = bitmap;
                                if (totalFrames != 1)
                                {
                                    //! BmpLst Approach
                                    using (FileStream fs = new FileStream(PathData.ActiveWallpapers[i], FileMode.Open, FileAccess.Read))
                                    {
                                        List<Bitmap> bmpLst = new List<Bitmap>();
                                        GifBitmapDecoder decoder = new GifBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                                        int frameCount = decoder.Frames.Count;

                                        for (int j = 0; j < frameCount; ++j)
                                        {
                                            // Create bitmap to hold the single frame
                                            Bitmap bmpSingleFrame = BitmapFromSource(decoder.Frames[j]);
                                            // add the frame (as a bitmap) to the bitmap list
                                            bmpLst.Add(bmpSingleFrame);
                                        }

                                        bitmap2 = bmpLst[currentFrame];
                                    }
                                }
                                */

                                while (currentFrame <= totalFrames) //? Draw wallpaper & loop through frames if needed
                                {
                                    if (animatedWallpaperThreads[i] != null && animatedWallpaperThreads[i].CancellationPending)
                                    {
                                        e.Cancel = true;
                                        return;
                                    }

                                    if (totalFrames != 1)
                                    {
                                        //? 1000 = 1 second, dividing this by 60 brings it to 60fps
                                        //Thread.Sleep(1000/60);
                                        //Thread.Sleep(60);
                                        Thread.Sleep(1000);

                                        //! Select Active Frame Approach
                                        if (currentFrame < totalFrames)
                                        {
                                            FrameDimension dimension = new FrameDimension(bitmap.FrameDimensionsList[0]);
                                            bitmap.SelectActiveFrame(dimension, currentFrame);
                                        }
                                    }
                                    Debug.WriteLine("bitmap size: " + bitmap.Size);

                                    Debug.WriteLine("currentFrame: " + currentFrame);
                                    int MonitorWidth = monitor.Bounds.Width;
                                    int MonitorHeight = monitor.Bounds.Height;
                                    int MonitorX = monitor.Bounds.X + MonitorXAdjustment;
                                    int MonitorY = monitor.Bounds.Y;
                                    int MonitorYAdjustment = MonitorY - minMonitorY;

                                    int ImageWidth = curImage.Width;
                                    int ImageHeight = curImage.Height;

                                    switch (WallpaperStyle)
                                    {
                                        case PictureStyle.Fill: //TODO Implement a cropping feature for fill so that it doesn't extend into other monitors

                                            if (ImageWidth > ImageHeight)
                                            {
                                                float sizeRatio = (float) ImageWidth / MonitorWidth;
                                                //!g.DrawImage(curImage, MonitorX, MonitorYAdjustment, MonitorWidth * sizeRatio, MonitorHeight);
                                                g.DrawImage(bitmap, MonitorX, MonitorYAdjustment, MonitorWidth * sizeRatio, MonitorHeight);
                                                //!g.DrawImage(bitmap2, MonitorX, MonitorYAdjustment, MonitorWidth * sizeRatio, MonitorHeight);
                                            }
                                            else
                                            {
                                                float sizeRatio = (float) ImageWidth / MonitorHeight;
                                                //!g.DrawImage(curImage, MonitorX, MonitorYAdjustment, MonitorWidth, MonitorHeight * sizeRatio);
                                                g.DrawImage(bitmap, MonitorX, MonitorYAdjustment, MonitorWidth, MonitorHeight * sizeRatio);
                                                //!g.DrawImage(bitmap2, MonitorX, MonitorYAdjustment, MonitorWidth, MonitorHeight * sizeRatio);
                                            }

                                            break;

                                        case PictureStyle.Stretch:
                                            //!g.DrawImage(curImage, MonitorX, MonitorYAdjustment, MonitorWidth, MonitorHeight);
                                            g.DrawImage(bitmap, MonitorX, MonitorYAdjustment, MonitorWidth, MonitorHeight);
                                            //!g.DrawImage(bitmap2, MonitorX, MonitorYAdjustment, MonitorWidth, MonitorHeight);
                                            break;

                                        case PictureStyle.Tile:

                                            // it's best to check with ratios rather than the exact ImageHeight & ImageWidth in order to avoid scaling out of the monitor
                                            float widthRatio = (float) ImageWidth / MonitorWidth;
                                            float heightRatio = (float) ImageHeight / MonitorHeight;

                                            int TaskBarHeight = 40;

                                            using (Brush brush = new SolidBrush(Color.Black))
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
                                                    g.FillRectangle(brush, 0, MonitorYAdjustment, leftGap + 1, MonitorHeight); // draws left-side border
                                                    g.FillRectangle(brush, xPos + adjustedWidth - 2, MonitorYAdjustment, leftGap + 2, MonitorHeight); // draws right-side border

                                                    //!g.DrawImage(curImage, xPos, MonitorYAdjustment, adjustedWidth, adjustedHeight);
                                                    g.DrawImage(bitmap, xPos, MonitorYAdjustment, adjustedWidth, adjustedHeight);
                                                    //!g.DrawImage(bitmap2, xPos, MonitorYAdjustment, adjustedWidth, adjustedHeight);
                                                }
                                                else // scale image to match the monitor width and let the height have gaps
                                                {
                                                    float adjustedHeight = ImageHeight * ((float) MonitorWidth / ImageWidth);

                                                    float heightDifference = MonitorHeight - adjustedHeight;
                                                    float bottomGap = heightDifference / 2;
                                                    float yPos = heightDifference - bottomGap;

                                                    //? the +1 and -2 values prevent the previous wallpaper from still being visible by 1-2 pixels, which is a bit jarring
                                                    //? the rectangle must be drawn first to prevent weird glitching from lag
                                                    g.FillRectangle(brush, 0, MonitorYAdjustment, MonitorWidth, heightDifference + 1); // draws top-side border
                                                    g.FillRectangle(brush, 0, yPos + adjustedHeight + MonitorYAdjustment - 2, MonitorWidth, bottomGap + 2); // draws bottom-side border

                                                    //!g.DrawImage(curImage, MonitorX, yPos + MonitorYAdjustment, MonitorWidth, adjustedHeight);
                                                    g.DrawImage(bitmap, MonitorX, yPos + MonitorYAdjustment, MonitorWidth, adjustedHeight);
                                                    //!g.DrawImage(bitmap2, MonitorX, yPos + MonitorYAdjustment, MonitorWidth, adjustedHeight);
                                                }
                                            }
                                            break;
                                    }

                                    if (currentFrame == totalFrames && totalFrames != 1) currentFrame = 0;
                                    currentFrame ++;
                                    Win32.ReleaseDC(workerw, _dc);
                                }

                                //bitmap2.Dispose();
                            }

                            //g.ReleaseHdc(_dc);
                            //g.ReleaseHdcInternal(_dc);
                            curImage.Dispose(); // curImage no longer needs to be accessed after drawing it to g
                        }
                    } // End of void DoWallpaperGraphics()
                }
            }
        }

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

        struct WallpaperGraphicsInfo
        {
            public IntPtr dc;
            public int frame;

            public WallpaperGraphicsInfo(IntPtr dc, int frame)
            {
                this.dc = dc;
                this.frame = frame;
            }
        }

        private void DrawBackground()
        {
            Image[] images = new Image[PathData.ActiveWallpapers.Length];

            if (RetreiveImage(ref images)) // attempts to load images, will prevent the program from crashing if memory overloads and instead just not load the wallpaper
            {
                // Set up monitor bounds & adjustments
                int TotalMonitorWidth = 0;
                int MaxMonitorHeight = 0;
                int MonitorXAdjustment = 0;
                int minMonitorY = int.MaxValue;

                foreach (Screen monitor in MonitorData.Screens)
                {
                    if (monitor.Bounds.X < 0) // used to prevent wallpapers from being drawn off the screen
                    {
                        MonitorXAdjustment += Math.Abs(monitor.Bounds.X);
                    }

                    if (monitor.Bounds.Y < minMonitorY)
                    {
                        minMonitorY = monitor.Bounds.Y;
                    }

                    TotalMonitorWidth += monitor.Bounds.Width;
                    MaxMonitorHeight = Math.Max(MaxMonitorHeight, monitor.Bounds.Height);
                }

                // Draw Background
                using (Bitmap b = new Bitmap(TotalMonitorWidth, MaxMonitorHeight))
                {
                    using (Graphics g = Graphics.FromImage(b))
                    {
                        for (int i = 0; i < MonitorData.Screens.Length; i++)
                        {
                            Screen monitor = MonitorData.Screens[i];
                            Image curImage = images[i];

                            int MonitorWidth = monitor.Bounds.Width;
                            int MonitorHeight = monitor.Bounds.Height;
                            int MonitorX = monitor.Bounds.X + MonitorXAdjustment;
                            int MonitorY = monitor.Bounds.Y;
                            int MonitorYAdjustment = MonitorY - minMonitorY;

                            int ImageWidth = curImage.Width;
                            int ImageHeight = curImage.Height;

                            switch (WallpaperStyle)
                            {
                                case PictureStyle.Fill: //TODO Implement a cropping feature for fill so that it doesn't extend into other monitors

                                    if (ImageWidth > ImageHeight)
                                    {
                                        float sizeRatio = (float)ImageWidth / MonitorWidth;
                                        g.DrawImage(curImage, MonitorX, MonitorYAdjustment, MonitorWidth * sizeRatio, MonitorHeight);
                                    }
                                    else
                                    {
                                        float sizeRatio = (float)ImageWidth / MonitorHeight;
                                        g.DrawImage(curImage, MonitorX, MonitorYAdjustment, MonitorWidth, MonitorHeight * sizeRatio);
                                    }
                                    break;

                                case PictureStyle.Stretch:
                                    g.DrawImage(curImage, MonitorX, MonitorYAdjustment, MonitorWidth, MonitorHeight);
                                    break;

                                case PictureStyle.Tile:

                                    // it's best to check with ratios rather than the exact ImageHeight & ImageWidth in order to avoid scaling out of the monitor
                                    float widthRatio = (float)ImageWidth / MonitorWidth;
                                    float heightRatio = (float)ImageHeight / MonitorHeight;

                                    int TaskBarHeight = 40;

                                    // if both are equal heightRatio should be preferred
                                    if (heightRatio >= widthRatio) // scale image to match the monitor height and let the width have gaps 
                                    {
                                        float adjustedHeight = MonitorHeight - TaskBarHeight;
                                        float adjustedWidth = ImageWidth * ((float)MonitorHeight / ImageHeight) * (adjustedHeight / MonitorHeight);
                                        //float adjustedWidth = MonitorWidth * ((float) ImageWidth / ImageHeight);

                                        float widthDifference = MonitorWidth - adjustedWidth;
                                        float leftGap = widthDifference / 2;
                                        float xPos = MonitorX + widthDifference - leftGap;

                                        g.DrawImage(curImage, xPos, MonitorYAdjustment, adjustedWidth, adjustedHeight);
                                    }
                                    else // scale image to match the monitor width and let the height have gaps
                                    {
                                        float adjustedHeight = ImageHeight * ((float)MonitorWidth / ImageWidth);

                                        float heightDifference = MonitorHeight - adjustedHeight;
                                        float bottomGap = heightDifference / 2;
                                        float yPos = heightDifference - bottomGap;

                                        g.DrawImage(curImage, MonitorX, yPos + MonitorYAdjustment, MonitorWidth, adjustedHeight);
                                    }
                                    break;
                            }

                            curImage.Dispose(); // curImage no longer needs to be accessed after drawing it to g
                        }
                    }

                    try
                    {
                        b.Save(PathData.ActiveWallpaperImage, ImageFormat.Jpeg);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            Thread.Sleep(1000); //? this buffer prevents a great deal of potential GDI+ errors
                            b.Save(PathData.ActiveWallpaperImage, ImageFormat.Jpeg);
                            Debug.WriteLine("Buffer Reload Attempt");
                            Debug.WriteLine(e);
                        }
                        catch (Exception e2)
                        {
                            Thread.Sleep(1000); //? Tries again with a blank background, then tries to reload the background
                            Bitmap placeholderBitmap = new Bitmap(TotalMonitorWidth, MaxMonitorHeight);
                            placeholderBitmap.Save(PathData.ActiveWallpaperImage, ImageFormat.Jpeg);
                            placeholderBitmap.Dispose();
                            Debug.WriteLine("Placeholder Bitmap");
                            Debug.WriteLine(e2);
                            DrawBackground();
                        }
                    }
                }
            }
        }

        private bool RetreiveImage(ref Image[] images)
        {
            return RetreiveImage(ref images, 0);
        }

        private bool RetreiveImage(ref Image[] images, int recursionCount)
        {
            bool redo = false;

            if (recursionCount > 5)
            {
                MessageBox.Show("Failed to load wallpaper");
                return false;
            }

            for (int i = 0; i < PathData.ActiveWallpapers.Length; i++)
            {
                FileStream fs;

                using (fs = new FileStream(PathData.ActiveWallpapers[i], FileMode.Open, FileAccess.Read))
                {
                    Thread.Sleep(25 * recursionCount);
                    try
                    {
                        images[i] = Image.FromStream(fs);
                    }
                    catch
                    {
                        Process proc = Process.GetCurrentProcess();
                        Debug.WriteLine("Memory Usage: " + proc.PrivateMemorySize64);
                        redo = true;
                    }
                }
            }

            if (redo)
            {
                Thread.Sleep(100);
                return RetreiveImage(ref images, recursionCount + 1);
            }

            return true;
        }

    }
}
