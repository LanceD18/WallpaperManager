using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using LanceTools;
using LanceTools.Mpv;
using Mpv.NET.Player;
using WallpaperManager.ApplicationData;
using Color = System.Drawing.Color;

namespace WallpaperManager.Wallpaper
{
    public partial class WallpaperForm : Form
    {
        private Rectangle pictureBoxBounds;

        private const int TASKBAR_SIZE = 40;
        private const double FRAME_LENGTH = (double)1 / 60;

        private MpvPlayer player;
        private string activeImagePath;

        public int Loops { get; private set; }
        public Stopwatch WallpaperUptime { get; private set; } = new Stopwatch();
        public bool IsPlayingVideo { get; private set; }

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
                    Left = monitor.Bounds.X + DisplayData.DisplayXAdjustment;
                    Top = monitor.Bounds.Y + DisplayData.MinDisplayY;
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
                    player.MediaEndedSeeking += (sender, args) => Loops++;

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
            Loops = 0;
            WallpaperUptime.Stop();

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
                    IsPlayingVideo = false;

                    player.Stop();
                    panelWallpaper.Visible = false;
                    panelWallpaper.Enabled = false;

                    pictureBoxWallpaper.ImageLocation = imageLocation;
                    pictureBoxWallpaper.Enabled = true;
                    pictureBoxWallpaper.Visible = true;
                }
                else
                {
                    IsPlayingVideo = true;
                    WallpaperUptime.Restart();

                    pictureBoxWallpaper.Visible = false;
                    pictureBoxWallpaper.Enabled = false;

                    panelWallpaper.Enabled = true;
                    panelWallpaper.Visible = true;
                    player.Reload(imageLocation);

                    //xDebug.WriteLine(imageLocation);
                    WallpaperData.VideoSettings videoSettings = WallpaperData.GetImageData(imageLocation).VideoSettings;
                    player.Volume = AudioManager.IsWallpapersMuted ? 0 : videoSettings.Volume;
                    player.Speed = videoSettings.PlaybackSpeed;

                    activeImagePath = imageLocation;
                }
            }

            ResetWallpaperStyle(); // this needs to be readjusted with each image
        }

        public void ResetWallpaperStyle()
        {
            SetWallpaperStyle(WallpaperData.WallpaperManagerForm.GetWallpaperStyle(WallpaperData.WallpaperManagerForm.GetWallpapers().IndexOf(this)));
        }

        public void SetWallpaperStyle(WallpaperStyle wallpaperStyle)
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
                        case WallpaperStyle.Fill:
                            using (Image image = Image.FromFile(pictureBoxWallpaper.ImageLocation))
                            {
                                int heightDiff = GetFillHeightDiff(image.Width, image.Height);
                                pictureBoxWallpaper.Width = Width; // scales the image to its width
                                pictureBoxWallpaper.Height = Height + heightDiff; // any additional height will be pushed offscreen
                                pictureBoxWallpaper.Top = -heightDiff / 2; // centers the height pushed offscreen
                                pictureBoxWallpaper.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                            break;

                        case WallpaperStyle.Stretch:
                            pictureBoxWallpaper.SizeMode = PictureBoxSizeMode.StretchImage;
                            break;

                        case WallpaperStyle.Zoom:
                            pictureBoxWallpaper.Height -= TASKBAR_SIZE;
                            pictureBoxWallpaper.SizeMode = PictureBoxSizeMode.Zoom;
                            break;
                    }

                    pictureBoxWallpaper.ResumeLayout();
                }

                if (panelWallpaper.Visible)
                {
                    panelWallpaper.SuspendLayout();
                    using (VideoCapture video = new VideoCapture(activeImagePath))
                    {
                        using (Mat m = new Mat()) video.Read(m);
                        //video.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosAviRatio, 0);

                        switch (wallpaperStyle)
                        {
                            case WallpaperStyle.Fill:
                                panelWallpaper.Bounds = pictureBoxBounds;
                                int heightDiff = GetFillHeightDiff(video.Width, video.Height);
                                panelWallpaper.Width = Width; // scales the image to its width
                                panelWallpaper.Height = Height + heightDiff; // any additional height will be pushed offscreen
                                panelWallpaper.Top = -heightDiff / 2; // centers the height pushed offscreen
                                break;

                            case WallpaperStyle.Stretch:
                                panelWallpaper.Bounds = pictureBoxBounds;
                                break;

                            case WallpaperStyle.Zoom:
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

        public void SetVolume(int volume)
        {
            if (player != null)
            {
                player.Volume = volume;
            }
        }

        public void Mute()
        {
            if (player != null)
            {
                player.Volume = 0;
            }
        }

        public void Unmute()
        {
            if (player != null)
            {
                player.Volume = WallpaperData.GetImageData(activeImagePath).VideoSettings.Volume;
            }
        }

        public void AwaitingEnd(int loops, float maxMilliseconds, Action<int> endEvent)
        {

        }
    }
}
