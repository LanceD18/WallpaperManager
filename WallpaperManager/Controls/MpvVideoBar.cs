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
        private bool setDraggingToFalse;
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
                if (!setDraggingToFalse) // while this is true this means that the mouse is no longer being held down
                {
                    if (dragging) trackBarPosition.SetValueToMousePosition(e);
                }
            };

            trackBarPosition.MouseUp += (s, e) =>
            {
                setDraggingToFalse = true; // this delays the ability to tell the program that dragging is false, preventing quick clicks/drags from being unidentified
                if (playing) mpvPlayer.Resume();
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
            if (!InvokeRequired)
            {
                trackBarVolume.Value = mpvPlayer.Volume;
                labelVolumeValue.Text = trackBarVolume.Value.ToString();
            }
            else
            {
                Invoke((MethodInvoker) delegate
                {
                    trackBarVolume.Value = mpvPlayer.Volume;
                    labelVolumeValue.Text = trackBarVolume.Value.ToString();
                });
            }
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
                double largeDurationRatio = 1;
                trackBarPosition.SuspendLayout();
                if (trackBarPosition.Maximum != (int) mpvPlayer.Duration.Ticks && mpvPlayer.Duration.Ticks != 0) // no need to recalculate everything if the calculation is already done
                {
                    //! managing this section improperly can cause the program to freeze for an extended period of time
                    trackBarPosition.TickFrequency = Int32.MaxValue; //Math.Max(1, (int) mpvPlayer.Duration.Ticks); //? increasing this dramatically improves tick creation time

                    //? if the ratio is below 1 then no modifications are required since this means that the duration is within the int range
                    largeDurationRatio = Math.Max((double)mpvPlayer.Duration.Ticks / Int32.MaxValue, 1);
                    //xDebug.WriteLine(largeDurationRatio);

                    //! this is incredibly resource intensive with low tick frequencies
                    trackBarPosition.Maximum = mpvPlayer.Duration.Ticks <= Int32.MaxValue ? (int)mpvPlayer.Duration.Ticks : Int32.MaxValue;
                }

                //xDebug.WriteLine("Position: " + mpvPlayer.Position.Ticks);
                if (!dragging)
                {
                    if (playing && pausedByDrag)
                    {
                        pausedByDrag = false;
                        mpvPlayer.Resume();
                    }

                    if (playing && !pausedByDrag)
                    {
                        try // sometimes the previous player updates this before the track bar can be reset, causing a crash. This just ensures the intended action is performed
                        {
                            trackBarPosition.Value = (int) ((double)mpvPlayer.Position.Ticks / largeDurationRatio);
                        }
                        catch (Exception exception)
                        {
                            try
                            {
                                trackBarPosition.Value = 0;
                            }
                            catch
                            {
                                // if even that doesn't work something may be wrong with the video (or it's just taking too long to load)
                                // do nothing and see if the auto-updater can fix it
                            }
                        }
                    }
                }
                else
                {
                    if (!pausedByDrag)
                    {
                        pausedByDrag = true;
                        mpvPlayer.Pause();
                    }

                    long positionValue = (long)(trackBarPosition.Value * largeDurationRatio);

                    if (positionValue < 0 )
                    {
                        Debug.WriteLine("Resetting Position to 0");
                        positionValue = 0;
                    }

                    if (positionValue > mpvPlayer.Duration.Ticks)
                    {
                        Debug.WriteLine("Resetting Position to max");
                        positionValue = mpvPlayer.Duration.Ticks;
                    }

                    if (lastDraggedValue != positionValue) // no need to update this when it's already there
                    {
                        mpvPlayer.Position = new TimeSpan(positionValue);  //! this is incredibly resource intensive, ensure that this is not called often
                        lastDraggedValue = trackBarPosition.Value;
                    }
                }
                trackBarPosition.ResumeLayout();

                if (setDraggingToFalse)
                {
                    dragging = false; // this delays the ability to tell the program that dragging is false, preventing quick clicks/drags from being unidentified
                    setDraggingToFalse = false;
                }
            });

            //? Don't forget that the above is a BeginInvoke so anything you expect to happen after it may not occur
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
