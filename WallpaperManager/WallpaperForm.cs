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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Emgu.CV;
using LanceTools.Mpv;
using MediaToolkit;
using MediaToolkit.Model;
using Mpv.NET.API;
using Mpv.NET.Player;
using Newtonsoft.Json.Bson;
using WallpaperManager.ApplicationData;
using WMPLib;
using Color = System.Drawing.Color;

namespace WallpaperManager
{
    public partial class WallpaperForm : Form
    {
        private Rectangle pictureBoxBounds;

        private int volume = 25;

        private const int TASKBAR_SIZE = 40;
        private const double FRAME_LENGTH = (double)1 / 60;

        public MpvPlayer player;
        private string playerVideoPath;

        // TODO Check why this is claiming that it's functioning on a different thread yet no thread is made when calling this? Finding this out will determine if the Invokes remain
        public WallpaperForm(Screen monitor, IntPtr workerw)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();

            Load += (s, e) =>
            {
                this.Invoke((MethodInvoker)delegate
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
                    panelWallpaper.Bounds = pictureBoxBounds;

                    // Initializes player
                    player = new MpvPlayer(panelWallpaper.Handle) // handle tells the player to draw itself onto the panelWallpaper
                    {
                        AutoPlay = true,
                        Loop = true
                    };
                    player.API.RequestLogMessages(MpvLogLevel.Warning);

                    ResetWallpaperStyle();

                    // This line makes the form a child of the WorkerW window, thus putting it behind the desktop icons and out of reach 
                    // for any user input. The form will just be rendered, no keyboard or mouse input will reach it.
                    // (Would have to use WH_KEYBOARD_LL and WH_MOUSE_LL hooks to capture mouse and keyboard input)
                    Win32.SetParent(Handle, workerw);
                });
            };

            Closing += async (s, e) =>
            {
                Controls.Remove(pictureBoxWallpaper);

                player.Stop();
                /*! //the below caused an assertion failed error
                await Task.Delay(1000);
                player.Dispose();
                */

                Controls.Remove(panelWallpaper);
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

            if (!IsHandleCreated) return;

            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { SetWallpaperProcess(); });
            }
            else
            {
                SetWallpaperProcess();
            }

            void SetWallpaperProcess()
            {
                if (!WallpaperManagerTools.IsSupportedVideoType(imageLocation))
                {
                    player.Stop();
                    panelWallpaper.Visible = false;
                    panelWallpaper.Enabled = false;

                    pictureBoxWallpaper.ImageLocation = imageLocation;
                    pictureBoxWallpaper.Enabled = true;
                    pictureBoxWallpaper.Visible = true;
                }
                else
                {
                    pictureBoxWallpaper.Visible = false;
                    pictureBoxWallpaper.Enabled = false;

                    panelWallpaper.Enabled = true;
                    panelWallpaper.Visible = true;
                    player.Reload(imageLocation);

                    Debug.WriteLine(imageLocation);
                    WallpaperData.VideoSettings VideoSettings = WallpaperData.GetImageData(imageLocation).VideoSettings;
                    player.Volume = VideoSettings.volume;
                    //?player.Speed = VideoSettings.playbackSpeed;

                    playerVideoPath = imageLocation;
                }
            }

            ResetWallpaperStyle(); // this needs to be readjusted with each image
        }

        public void ResetWallpaperStyle()
        {
            SetWallpaperStyle(WallpaperData.WallpaperManagerForm.GetWallpaperStyle());
        }

        public void SetWallpaperStyle(PictureStyle wallpaperStyle)
        {
            if (!IsHandleCreated) return;

            Debug.WriteLine("Setting Style");
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { SetWallpaperStyleProcess(); });
            }
            else
            {
                SetWallpaperStyleProcess();
            }

            void SetWallpaperStyleProcess()
            {
                if (pictureBoxWallpaper.Visible)
                {
                    if (pictureBoxWallpaper.ImageLocation == null) return;

                    pictureBoxWallpaper.SuspendLayout();
                    pictureBoxWallpaper.Bounds = pictureBoxBounds; // it's generally a good idea to reset this to prevent unwanted changes from previous states
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
                            pictureBoxWallpaper.SizeMode = PictureBoxSizeMode.StretchImage;
                            break;

                        case PictureStyle.Zoom:
                            pictureBoxWallpaper.Height -= TASKBAR_SIZE;
                            pictureBoxWallpaper.SizeMode = PictureBoxSizeMode.Zoom;
                            break;
                    }

                    pictureBoxWallpaper.ResumeLayout();
                }

                if (panelWallpaper.Visible)
                {
                    panelWallpaper.SuspendLayout();
                    using (VideoCapture video = new VideoCapture(playerVideoPath))
                    {
                        using (Mat m = new Mat()) video.Read(m);
                        //video.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosAviRatio, 0);

                        switch (wallpaperStyle)
                        {
                            case PictureStyle.Fill:
                                panelWallpaper.Bounds = pictureBoxBounds;
                                int heightDiff = GetFillHeightDiff(video.Width, video.Height);
                                panelWallpaper.Width = Width; // scales the image to its width
                                panelWallpaper.Height = Height + heightDiff; // any additional height will be pushed offscreen
                                panelWallpaper.Top = -heightDiff / 2; // centers the height pushed offscreen
                                break;

                            case PictureStyle.Stretch:
                                panelWallpaper.Bounds = pictureBoxBounds;
                                break;

                            case PictureStyle.Zoom:
                                panelWallpaper.Bounds = GetZoomBounds(video.Width, video.Height);
                                break;
                        }

                        panelWallpaper.ResumeLayout();
                    }

                }
            }
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
