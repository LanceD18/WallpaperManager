using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;
using LanceTools;
using LanceTools.FormUtil;
using Mpv.NET.Player;

namespace WallpaperManager.Controls
{
    public partial class MpvVideoBar : UserControl
    {
        private MpvPlayer mpvPlayer;
        private Action updater;

        private const int MARGIN = 5;
        private const int DEFAULT_TRACKBARPOSITION_WIDTH = 250;

        private bool playing = true;
        private bool dragging;
        private bool pausedByDrag;
        private int lastDraggedValue;

        public MpvVideoBar()
        {
            InitializeComponent();

            this.Resize += OnResize;

            trackBarPosition.MouseDown += (s, e) =>
            {
                dragging = true;
                mpvPlayer.Pause();

                trackBarPosition.SetValueToMousePosition(e);
            };

            trackBarPosition.MouseMove += (s, e) =>
            {
                if (dragging) trackBarPosition.SetValueToMousePosition(e);
            };

            trackBarPosition.MouseUp += (s, e) =>
            {
                dragging = false;
                mpvPlayer.Resume();

                //double dblValue = (Convert.ToDouble(e.X) / Convert.ToDouble(trackBarPosition.Width)) * (trackBarPosition.Maximum - trackBarPosition.Minimum);
                //trackBarPosition.Value = Convert.ToInt32(dblValue);
            };
        }

        public void LinkPlayer(MpvPlayer player, Action updater)
        {
            this.mpvPlayer = player;
            this.updater = updater;
            //?timerVideo.Start(); //? placing this in the constructor will cause timerVideo to activate on design time, causing a crash | Auto-enabled by control now, oops
        }

        // only the volume is updated here because the video won't update fast enough for the duration to be ready, it'll have to get included elsewhere
        public void UpdatePlayerVolume()
        {
            trackBarVolume.Value = mpvPlayer.Volume;
            labelVolumeValue.Text = trackBarVolume.Value.ToString();
        }

        public int GetVolume() => mpvPlayer.Volume;

        public double GetSpeed() => mpvPlayer.Speed;

        private void OnResize(object sender, EventArgs e)
        {
            int rescaledTrackBarHeight = (int)(Height * 0.85f);
            int newTrackBarY = (Height - rescaledTrackBarHeight) / 2;

            // Volume Track Bar
            trackBarVolume.Height = rescaledTrackBarHeight;
            //trackBarVolume.Width = Width - trackBarVolume.Location.X - RIGHT_EDGE_MARGIN;
            trackBarVolume.Location = new Point(Width - trackBarVolume.Width - MARGIN, newTrackBarY);

            // Volume Labels
            int volumeHeightDiff = labelVolumeValue.Location.Y - labelVolume.Location.Y; // used to recalculate the value height later
            labelVolume.Location = new Point(trackBarVolume.Location.X - labelVolume.Width - MARGIN, newTrackBarY);
            labelVolumeValue.Location = new Point(trackBarVolume.Location.X - labelVolumeValue.Width - MARGIN, newTrackBarY + volumeHeightDiff);

            // Position Track Bar & Play Pause Button
            int pausePlayHeightDiff = buttonPlayPause.Location.Y - trackBarPosition.Location.Y;
            trackBarPosition.Height = rescaledTrackBarHeight;
            trackBarPosition.Location = new Point(trackBarPosition.Location.X, newTrackBarY);

            trackBarPosition.Width = Math.Min(DEFAULT_TRACKBARPOSITION_WIDTH, labelVolume.Location.X - trackBarPosition.Location.X - MARGIN);

            buttonPlayPause.Location = new Point(buttonPlayPause.Location.X, newTrackBarY + pausePlayHeightDiff);
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            labelVolumeValue.Text = trackBarVolume.Value.ToString();
            mpvPlayer.Volume = trackBarVolume.Value;

            updater?.Invoke();
        }

        private void trackBarPosition_Scroll(object sender, EventArgs e)
        {
            //! Doing this here will use up a great deal of cpu if scrolling too quickly (Done under timer instead)
            //player.Position = new TimeSpan(trackBarPosition.Value);
        }

        private void timerVideo_Tick(object sender, EventArgs e)
        {
            trackBarPosition.BeginInvoke((MethodInvoker) delegate
            {
                trackBarPosition.SuspendLayout();
                if (trackBarPosition.Maximum != (int) mpvPlayer.Duration.Ticks && mpvPlayer.Duration.Ticks != 0) // no need to recalculate everything if the calculation is already done
                {
                    //! managing this section improperly can cause the program to freeze for an extended period of time
                    trackBarPosition.TickFrequency = Math.Max(1, (int) mpvPlayer.Duration.Ticks); //? reducing this dramatically improves tick creation time
                    trackBarPosition.Maximum = (int) mpvPlayer.Duration.Ticks; //! this is incredibly resource intensive with low tick frequencies
                }

                if (!dragging)
                {
                    if (playing && pausedByDrag)
                    {
                        pausedByDrag = false;
                        mpvPlayer.Resume();
                    }

                    trackBarPosition.Value = (int) mpvPlayer.Position.Ticks;
                }
                else
                {
                    if (!pausedByDrag)
                    {
                        pausedByDrag = true;
                        mpvPlayer.Pause();
                    }

                    int positionValue = trackBarPosition.Value;
                    if (lastDraggedValue != positionValue) // no need to update this when it's already there
                    {
                        Task.Run(() => mpvPlayer.Position = new TimeSpan(positionValue)); //! this is incredibly resource intensive, ensure that this is not called often
                        lastDraggedValue = trackBarPosition.Value;
                    }
                }
                trackBarPosition.ResumeLayout();
            });
        }

        private void buttonPlayPause_Click(object sender, EventArgs e)
        {
            playing = !playing;
            buttonPlayPause.Text = playing ? "| |" : ">";

            if (playing) mpvPlayer.Resume();
            if (!playing) mpvPlayer.Pause();
        }
    }
}
