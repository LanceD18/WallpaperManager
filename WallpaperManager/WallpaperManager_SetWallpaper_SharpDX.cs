using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using WallpaperManager.ApplicationData;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using Device = SharpDX.DXGI.Device;
using Device1 = SharpDX.Direct2D1.Device1;
using Factory = SharpDX.Direct2D1.Factory;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace WallpaperManager
{
    //? SharpDX implementation of SetWallpaper
    //! Currently not in use, may look into utilizing this in the future if it has the potential to improve optimization
    public partial class WallpaperManager : Form
    {
        private Bitmap _bitmap;

        // Derived from: https://github.com/sharpdx/SharpDX-Samples/blob/master/Desktop/Direct2D1/BitmapApp/Program.cs
        /// <summary>
        /// Loads a Direct2D Bitmap from a file using System.Drawing.Image.FromFile(...)
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <param name="file">The file.</param>
        /// <returns>A D2D1 Bitmap</returns>
        public Bitmap LoadFromFile(RenderTarget renderTarget, string file)
        {
            // Loads from file using System.Drawing.Image
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            //! Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
                }
            }
        }

        private void GetImageRenderTarget()
        {
            /*
            SharpDX.DXGI.Device2 dxgiDevice2 = new SharpDX.DXGI.Device2(IntPtr.Zero);
            SharpDX.Direct2D1.Device dxgiDevice = new SharpDX.Direct2D1.Device(dxgiDevice2);
            DeviceContext dxgiContext = new DeviceContext(dxgiDevice, DeviceContextOptions.None);
            BitmapProperties1 properties = new BitmapProperties1(new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied),
                DisplayIn.LogicalDpi, DisplayProperties.LogicalDpi, BitmapOptions.Target | BitmapOptions.CannotDraw);
            */
        }

        private void SetWallpaperSharpDX()
        {
            //b.Save(PathData.ActiveWallpaperImage, ImageFormat.Jpeg);

            setWallpaperThread = new Thread(() =>
            {
                // Check if all potential wallpapers exist
                foreach (string wallpaperPath in PathData.ActiveWallpapers)
                {
                    if (!File.Exists(wallpaperPath))
                    {
                        MessageBox.Show("The following file path does not exist: [" + wallpaperPath + "]\n\nWallpaper will not be changed");
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

                    //DrawBackground();
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

    }
}
