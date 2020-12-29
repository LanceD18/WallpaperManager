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

        private int volume = 25;

        private const int TASKBAR_SIZE = 40;
        private const double FRAME_LENGTH = (double)1 / 60;

        // TODO Check why this is claiming that it's functioning on a different thread yet no thread is made when calling this? Finding this out will determine if the Invokes remain
        public WallpaperForm(Screen monitor, IntPtr workerw)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();

            // TODO Use this later for other optional features
            /*
            axWindowsMediaPlayerWallpaper.PlayStateChange += (ax_s, ax_e) =>
            {
                if (ax_e.newState == 8) // this state indicates that the media has ended | States: http://msdn.microsoft.com/en-us/library/windows/desktop/dd562460%28v=vs.85%29.aspx
                {
                    //axWindowsMediaPlayerWallpaper.Ctlcontrols.fastReverse();
                }
            };
            */

            Load += (s, e) =>
            {
                this.Invoke((MethodInvoker) delegate
                {
                    BackColor = Color.Black;

                    // Sets bounds of the form
                    Width = monitor.Bounds.Width;
                    Height = monitor.Bounds.Height;
                    Left = monitor.Bounds.X + MonitorData.MonitorXAdjustment;
                    Top = monitor.Bounds.Y + MonitorData.MinMonitorY;
                    pictureBoxBounds = new Rectangle(0, 0, Width, Height);

                    // Sets bounds of the wallpaper
                    pictureBoxWallpaper.Bounds = pictureBoxBounds;

                    axWindowsMediaPlayerWallpaper.PlayStateChange += (s2, e2) =>
                    {
                        // some file types (.mp4) don't appear to have this issue, so doing this causes them to return to position 0 twice
                        if (axWindowsMediaPlayerWallpaper.URL.Contains(".webm"))
                        {
                            if (e2.newState == 8) // MediaEnded state
                            {
                                Debug.WriteLine("State Change");
                                axWindowsMediaPlayerWallpaper.Ctlcontrols.currentPosition = 0;
                            }
                        }
                    };

                    /* Alternative Method, can guarantee no black flicker at the cost of losing some frames
                    // (Note that flicker length is random, the state change method was more consistent than this)
                    WallpaperData.WallpaperManagerForm.AppendTimerVideoLooperEvent_Tick(new Action(() =>
                    {
                        if (axWindowsMediaPlayerWallpaper.Ctlcontrols.currentItem != null)
                        {
                            if (axWindowsMediaPlayerWallpaper.Ctlcontrols.currentPosition > axWindowsMediaPlayerWallpaper.Ctlcontrols.currentItem.duration - (FRAME_LENGTH * 3))
                            {
                                Debug.WriteLine("Duration: " + axWindowsMediaPlayerWallpaper.Ctlcontrols.currentItem.duration);
                                axWindowsMediaPlayerWallpaper.Ctlcontrols.currentPosition = 0;
                            }
                        }
                    }));
                    */

                    axWindowsMediaPlayerWallpaper = WallpaperManagerTools.InitializeWindowsMediaPlayer(axWindowsMediaPlayerWallpaper, false);
                    axWindowsMediaPlayerWallpaper.Bounds = pictureBoxBounds;
                    axWindowsMediaPlayerWallpaper.Ctlenabled = false;

                    ResetWallpaperStyle();

                    // This line makes the form a child of the WorkerW window, thus putting it behind the desktop icons and out of reach 
                    // for any user input. The form will just be rendered, no keyboard or mouse input will reach it.
                    // (Would have to use WH_KEYBOARD_LL and WH_MOUSE_LL hooks to capture mouse and keyboard input)
                    Win32.SetParent(Handle, workerw);
                });
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

        // TODO Create a queue that stores pictureBoxes/axWindowMediaPlayers for each wallpaper. This will be used to allow transitions & prevent flickering from
        // TODO style readjustment when changing wallpapers by *locking* the previous wallpaper in place
        public void SetWallpaper(string imageLocation)
        {
            //! The below conditions were removed but I may consider re-adding this in the future when monitor-specific settings are added, although other methods of handling
            //! not changing a monitor's wallpaper would probably be better than this
            //!if (imageLocation == null) return; //? Under certain conditions a wallpaper won't be selected, this prevents the program from crashing over this

            this.Invoke((MethodInvoker) delegate
            {
                if (!WallpaperManagerTools.IsSupportedVideoType(new FileInfo(imageLocation).Extension))
                {
                    //?pictureBoxWallpaper.Invoke((MethodInvoker) delegate
                    //?{
                        pictureBoxWallpaper.ImageLocation = imageLocation;
                        pictureBoxWallpaper.Visible = true;
                    //?});

                    //?axWindowsMediaPlayerWallpaper.Invoke((MethodInvoker) delegate
                    //?{
                        axWindowsMediaPlayerWallpaper.Visible = axWindowsMediaPlayerWallpaper.Enabled = false;
                        axWindowsMediaPlayerWallpaper.settings.volume = 0; // the audio still plays even after disabling everything, and no, the mute method didn't work
                    //?});

                }
                else
                {
                    //?pictureBoxWallpaper.Invoke((MethodInvoker) delegate
                    //?{
                        pictureBoxWallpaper.Visible = false;
                    //?});

                    //?axWindowsMediaPlayerWallpaper.Invoke((MethodInvoker) delegate
                    //?{
                        axWindowsMediaPlayerWallpaper.Visible = true;
                        axWindowsMediaPlayerWallpaper = WallpaperManagerTools.UpdateWindowsMediaPlayer(axWindowsMediaPlayerWallpaper, imageLocation);

                    //?});
                }
            });

            ResetWallpaperStyle(); // this needs to be readjusted with each image
        }

        public void ResetWallpaperStyle()
        {
            SetWallpaperStyle(WallpaperData.WallpaperManagerForm.GetWallpaperStyle());
        }

        public void SetWallpaperStyle(PictureStyle wallpaperStyle)
        {
            Debug.WriteLine("Setting Style");
            pictureBoxWallpaper.Invoke((MethodInvoker) delegate
            {
                if (pictureBoxWallpaper.Visible)
                {
                    pictureBoxWallpaper.SuspendLayout();
                    switch (wallpaperStyle)
                    {
                        case PictureStyle.Fill:
                            using (Image image = Image.FromFile(pictureBoxWallpaper.ImageLocation))
                            {
                                int heightDiff = GetFillHeightDiff(image.Width, image.Height);
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
                            pictureBoxWallpaper.Bounds = new Rectangle(pictureBoxBounds.X, pictureBoxBounds.Y, pictureBoxBounds.Width, pictureBoxBounds.Height - TASKBAR_SIZE);
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
                    //video.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosAviRatio, 0);

                    switch (wallpaperStyle)
                    {
                        case PictureStyle.Fill:
                            int heightDiff = GetFillHeightDiff(video.Width, video.Height);
                            axWindowsMediaPlayerWallpaper.Width = Width; // scales the image to its width
                            axWindowsMediaPlayerWallpaper.Height = Height + heightDiff; // any additional height will be pushed offscreen
                            axWindowsMediaPlayerWallpaper.Top = -heightDiff / 2; // centers the height pushed offscreen
                            axWindowsMediaPlayerWallpaper.Refresh();
                            break;

                        case PictureStyle.Stretch:
                            axWindowsMediaPlayerWallpaper.Bounds = pictureBoxBounds;
                            break;

                        case PictureStyle.Zoom:
                            axWindowsMediaPlayerWallpaper.Bounds = GetZoomBounds(video.Width, video.Height);
                            break;
                    }

                    axWindowsMediaPlayerWallpaper.ResumeLayout();

                }
            });
        }

        private int GetFillHeightDiff(int imageWidth, int imageHeight)
        {
            float imageRatio = (float)imageWidth / imageHeight;
            float monitorRatio = (float)Width / Height;

            float combinedRatio = monitorRatio / imageRatio;
            float rescaledImageHeight = Height * combinedRatio;

            return (int)Math.Abs(Height - rescaledImageHeight);
        }

        private Rectangle GetZoomBounds(int videoWidth, int videoHeight) // images can do this automatically with the pictureBox
        {
            // it's best to check with ratios rather than the exact ImageHeight & ImageWidth in order to avoid scaling out of the monitor
            float widthRatio = (float)videoWidth / Width;
            float heightRatio = (float)videoHeight / Height;

            int TaskBarHeight = TASKBAR_SIZE; // TODO Calculate this based on the actual object & reposition it for different TaskBar positions

            // if both are equal heightRatio should be preferred
            if (heightRatio >= widthRatio) // scale image to match the monitor height and let the width have gaps 
            {
                float adjustedHeight = Height - TaskBarHeight;
                float adjustedWidth = videoWidth * ((float)Height / videoHeight) * (adjustedHeight / Height);
                //float adjustedWidth = MonitorWidth * ((float) ImageWidth / ImageHeight);

                float widthDifference = Width - adjustedWidth;
                float leftGap = widthDifference / 2;
                float xPos = 0 + widthDifference - leftGap;

                return new Rectangle((int)xPos, 0, (int)adjustedWidth, (int)adjustedHeight);
            }
            else // scale image to match the monitor width and let the height have gaps
            {
                float adjustedHeight = videoHeight * ((float)Width / videoWidth);

                float heightDifference = Height - adjustedHeight;
                float bottomGap = heightDifference / 2;
                float yPos = heightDifference - bottomGap;

                return new Rectangle(0, (int)yPos, Width, (int)adjustedHeight);
            }
        }
    }
}
