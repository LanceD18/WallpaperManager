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
    //! SetWallpaper should be its own class/static function, it has very few dependencies on the form itself (if any at all)
    public partial class WallpaperManager : Form
    {
        private Thread setWallpaperThread;
        private BackgroundWorker[] animatedWallpaperThreads;
        private bool[] activatedWallpaperThreads;

        private void SetWallpaper()
        {
            SetFormWallpaper();
            return;

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
                    SystemParametersInfo(SetDeskWallpaper, 0, PathData.ActiveWallpaperImageFile, UpdateIniFile | SendWinIniChange);
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

                        case PictureStyle.Zoom:
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

        private void DrawBackground() //? Although this method is currently not in use, note that some of the monitor data near the top are now calculated in the MonitorData class
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

                                case PictureStyle.Zoom:

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

                        foreach (Image image in images) image.Dispose();
                    }

                    try
                    {
                        b.Save(PathData.ActiveWallpaperImageFile, ImageFormat.Png);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            Thread.Sleep(1000); //? this buffer prevents a great deal of potential GDI+ errors
                            b.Save(PathData.ActiveWallpaperImageFile, ImageFormat.Png);
                            Debug.WriteLine("Buffer Reload Attempt");
                            Debug.WriteLine(e);
                        }
                        catch (Exception e2)
                        {
                            Thread.Sleep(1000); //? Tries again with a blank background, then tries to reload the background
                            Bitmap placeholderBitmap = new Bitmap(TotalMonitorWidth, MaxMonitorHeight);
                            placeholderBitmap.Save(PathData.ActiveWallpaperImageFile, ImageFormat.Png);
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
