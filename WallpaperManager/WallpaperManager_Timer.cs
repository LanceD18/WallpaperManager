using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Timers;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using WallpaperManager.ApplicationData;
using Timer = System.Timers.Timer;
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.Sandbox;
using AudioSwitcher.AudioApi.Session;
using SharpDX.DXGI;
using System.Runtime.InteropServices;

namespace WallpaperManager
{
    public partial class WallpaperManagerForm : Form
    {
        private Timer timer;
        private Stopwatch timerStopWatch = new Stopwatch();
        private int timerMaxInterval;

        private void InitializeTimer()
        {
            timer = new Timer(1);
            timer.Elapsed += IntervalElapsed;
        }

        private int GetTimerInterval()
        {
            int intervalNum = 1;
            string selectedTimer = comboBoxWallpaperInterval.SelectedItem.ToString();

            if (selectedTimer.IndexOfAny("0123456789".ToCharArray()) != -1)
            {
                intervalNum = Int32.Parse(selectedTimer.Substring(0, selectedTimer.IndexOf(' ')));
                string intervalType = selectedTimer.Substring(selectedTimer.IndexOf(' ') + 1);

                switch (intervalType)
                {
                    case "sec":
                        intervalNum *= 1000;
                        break;

                    case "min":
                        intervalNum *= 60000;
                        break;

                    case "hour":
                    case "hours":
                        intervalNum *= 3600000;
                        break;

                    case "day":
                    case "days":
                        intervalNum *= 86400000;
                        break;

                    case "week":
                    case "weeks":
                        intervalNum *= 604800000;
                        break;
                }
            }

            return intervalNum;
        }

        private void IntervalElapsed(object sender, ElapsedEventArgs e)
        {
            timerStopWatch.Restart();
            NextWallpaper(true);
        }

        // when the user change's their preferred timer
        private void comboBoxWallpaperInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timerStopWatch.Restart();
                timer.Interval = timerMaxInterval = GetTimerInterval();

                if (timer.Interval <= 1)
                {
                    labelTimeLeft.Text = "Awaiting Timer";
                    timer.Stop();
                }
                else
                {
                    timer.Start();
                    timerStopWatch.Start();
                }
            }
        }

        private void timerVisualUpdater_Tick(object sender, EventArgs e)
        {
            if (timer != null && timer.Interval > 1)
            {
                labelTimeLeft.Text = (((int)(timer.Interval - timerStopWatch.ElapsedMilliseconds) / 1000) + 1).ToString();
            }
        }

        public int GetTimerIndex()
        {
            return comboBoxWallpaperInterval.SelectedIndex;
        }

        public void SetTimerIndex(int newIndex)
        {
            if (newIndex < comboBoxWallpaperInterval.Items.Count)
            {
                comboBoxWallpaperInterval.SelectedIndex = newIndex;
            }
            else
            {
                comboBoxWallpaperInterval.SelectedIndex = comboBoxWallpaperInterval.Items.Count - 1;
            }
            //? this will carry over to the SelectedIndexChanged event and change the timer itself

            ResetTimer(); // even if the same index is given the timer should reset back to the default value for this index
        }

        public void ResetTimer()
        {
            if (timerMaxInterval != 0) // if this equals 0 then there is no timer
            {
                timer.Interval = timerMaxInterval;
                timerStopWatch.Restart();
            }
        }

        private void timerAudioChecker_Tick(object sender, EventArgs e) => Task.Run(() => AudioManager.CheckForMuteConditions());
    }
}
