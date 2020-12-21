using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Emgu.CV;
using MediaToolkit;
using MediaToolkit.Model;
using Newtonsoft.Json.Bson;
using WallpaperManager.ApplicationData;
using WMPLib;
using Color = System.Drawing.Color;

namespace WallpaperManager
{
    public partial class WallpaperForm : Form
    {
        private const int SetDeskWallpaper = 20;
        private const int UpdateIniFile = 0x01;
        private const int SendWinIniChange = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private Rectangle pictureBoxBounds;

        private PictureStyle pictureStyle;

        private int volume = 50;

        public WallpaperForm(Screen monitor, IntPtr workerw, PictureStyle pictureStyle)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();

            // TODO Use this later for other optional features
            axWindowsMediaPlayerWallpaper.PlayStateChange += (ax_s, ax_e) =>
            {
                if (ax_e.newState == 8) // this state indicates that the media has ended | States: http://msdn.microsoft.com/en-us/library/windows/desktop/dd562460%28v=vs.85%29.aspx
                {
                    //axWindowsMediaPlayerWallpaper.Ctlcontrols.fastReverse();
                }
            };

            Load += (s, e) =>
            {
                BackColor = Color.Black;

                // Sets bounds of the form
                Width = monitor.Bounds.Width;
                Height = monitor.Bounds.Height;
                Left = monitor.Bounds.X + MonitorData.MonitorXAdjustment;
                Top = monitor.Bounds.Y + MonitorData.MinMonitorY;

                // Sets bounds of the wallpaper
                pictureBoxBounds = new Rectangle(0, 0, Size.Width, Size.Height); //? Used further under SetWallpaperStyle
                pictureBoxWallpaper.Bounds = pictureBoxBounds;
                //pictureBoxWallpaper.Top = 0;
                //pictureBoxWallpaper.Left = 0;
                //pictureBoxWallpaper.Size = Size;
                //pictureBoxWallpaper.SizeMode = PictureBoxSizeMode.StretchImage;

                axWindowsMediaPlayerWallpaper.Bounds = pictureBoxBounds;
                //axWindowsMediaPlayerWallpaper.Ctlenabled = false;
                axWindowsMediaPlayerWallpaper.stretchToFit = true;
                axWindowsMediaPlayerWallpaper.uiMode = "none";
                axWindowsMediaPlayerWallpaper.settings.volume = 0;
                axWindowsMediaPlayerWallpaper.settings.setMode("loop", true);

                SetWallpaperStyle(pictureStyle);

                // This line makes the form a child of the WorkerW window, thus putting it behind the desktop icons and out of reach 
                // for any user input. The form will just be rendered, no keyboard or mouse input will reach it.
                // (Would have to use WH_KEYBOARD_LL and WH_MOUSE_LL hooks to capture mouse and keyboard input)
                Win32.SetParent(Handle, workerw);
            };

            Closing += (s, e) =>
            {
                Controls.Remove(pictureBoxWallpaper);
                Controls.Remove(axWindowsMediaPlayerWallpaper);

                // Upon closing the application you'll revert back to your default, windows wallpapers
                SystemParametersInfo(SetDeskWallpaper, 0, null, UpdateIniFile | SendWinIniChange);
            };
        }

        //? Using this will cause old wallpapers to remain visible if you aren't filling the entire screen
        /*
        // makes the background transparent to prevent flickering (Only stops the 1 frame flicker when closing, not the one that occurs when loading)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var sb = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
            e.Graphics.FillRectangle(sb, this.DisplayRectangle);
        }
        */

        public void SetWallpaper(string imageLocation)
        {
            if (!WallpaperManagerTools.IsSupportedVideoType(new FileInfo(imageLocation).Extension))
            {
                pictureBoxWallpaper.Invoke((MethodInvoker) delegate
                {
                    pictureBoxWallpaper.ImageLocation = imageLocation;
                    pictureBoxWallpaper.Visible = true;
                });

                axWindowsMediaPlayerWallpaper.Invoke((MethodInvoker) delegate
                {
                    axWindowsMediaPlayerWallpaper.Visible = axWindowsMediaPlayerWallpaper.Enabled = false;
                    axWindowsMediaPlayerWallpaper.settings.volume = 0; // the audio still plays even after disabling everything, and no, the mute method didn't work
                });

            }
            else
            {
                pictureBoxWallpaper.Invoke((MethodInvoker)delegate
                {
                    pictureBoxWallpaper.Visible = false;
                });

                axWindowsMediaPlayerWallpaper.Invoke((MethodInvoker)delegate
                {
                    axWindowsMediaPlayerWallpaper.URL = imageLocation;
                    axWindowsMediaPlayerWallpaper.Visible = axWindowsMediaPlayerWallpaper.Enabled = true;
                    axWindowsMediaPlayerWallpaper.settings.volume = volume;
                });
            }
        }

        public void SetWallpaperStyle(PictureStyle wallpaperStyle)
        {
            pictureStyle = wallpaperStyle;
            pictureBoxWallpaper.Invoke((MethodInvoker) delegate
            {
                if (pictureBoxWallpaper.Visible)
                {
                    pictureBoxWallpaper.SuspendLayout();
                    switch (pictureStyle)
                    {
                        case PictureStyle.Fill:
                            using (Image image = Image.FromFile(pictureBoxWallpaper.ImageLocation))
                            {
                                int heightDiff = Math.Abs(Height - image.Height);
                                pictureBoxWallpaper.Width = Width; // scales the image to its width
                                pictureBoxWallpaper.Height = Height + heightDiff; // any additional height will be pushed offscreen
                                pictureBoxWallpaper.Top = -heightDiff / 2; // centers the height pushed offscreen
                                pictureBoxWallpaper.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                            break;

                        case PictureStyle.Stretch:
                            pictureBoxWallpaper.Bounds = pictureBoxBounds;
                            pictureBoxWallpaper.SizeMode = PictureBoxSizeMode.StretchImage;
                            break;

                        case PictureStyle.Zoom:
                            pictureBoxWallpaper.Bounds = pictureBoxBounds;
                            pictureBoxWallpaper.SizeMode = PictureBoxSizeMode.Zoom;
                            break;
                    }

                    pictureBoxWallpaper.ResumeLayout();
                }
            });

            axWindowsMediaPlayerWallpaper.Invoke((MethodInvoker) delegate
            {
                if (axWindowsMediaPlayerWallpaper.Visible)
                {
                    axWindowsMediaPlayerWallpaper.SuspendLayout();
                    VideoCapture video = new VideoCapture(axWindowsMediaPlayerWallpaper.URL);

                    Mat m = new Mat();
                    video.Read(m);
                    video.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosAviRatio, 0);
                    int videoWidth = video.Width;
                    int videoHeight = video.Height;
                    Debug.WriteLine("Width: " + videoWidth + " | Height: " + videoHeight);

                    switch (pictureStyle)
                    {
                        case PictureStyle.Fill:
                            int heightDiff = Math.Abs(Height - videoHeight);
                            //int heightDiff = Math.Abs(Height - image.Height);
                            axWindowsMediaPlayerWallpaper.Width = Width; // scales the image to its width
                            axWindowsMediaPlayerWallpaper.Height = Height + heightDiff; // any additional height will be pushed offscreen
                            axWindowsMediaPlayerWallpaper.Top = -heightDiff / 2; // centers the height pushed offscreen
                            axWindowsMediaPlayerWallpaper.Refresh();
                            break;

                        case PictureStyle.Stretch:
                            axWindowsMediaPlayerWallpaper.Bounds = pictureBoxBounds;
                            break;

                        case PictureStyle.Zoom:
                            axWindowsMediaPlayerWallpaper.Bounds = pictureBoxBounds;
                            break;
                    }

                    axWindowsMediaPlayerWallpaper.ResumeLayout();

                }
            });
        }
    }
}
