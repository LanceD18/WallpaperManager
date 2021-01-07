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
using LanceTools;
using WallpaperManager.Controls;
using WallpaperManager.Options;

namespace WallpaperManager
{
    public partial class WallpaperManagerForm : Form
    {
        private Timer[] timers;
        private Stopwatch[] timerStopWatches;
        private int[] timerMaxIntervals;

        private void InitializeTimer()
        {
            int displayCount = DisplayData.Displays.Length;
            timers = new Timer[displayCount];
            timerStopWatches = new Stopwatch[DisplayData.Displays.Length];
            timerMaxIntervals = new int[displayCount];

            for (var i = 0; i < displayCount; i++)
            {
                timers[i] = new Timer(1);
                timers[i].Elapsed += IntervalElapsed;

                timerStopWatches[i] = new Stopwatch();
            }
        }

        private int GetTimerInterval(int index)
        {
            int intervalNum = 1;
            string selectedTimer = displaySettingForms[index].GetSelectedWallpaperInterval();

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
            //? Without the BeginInvoke similar timers will be guaranteed to de-sync due to latency (I think...)
            int index = timers.IndexOf(sender as Timer);

            if (!DisplaySettingsSynced)
            {
                timerStopWatches[index].Restart();
                IntervalWallpaperSetter(index);
            }
            else if (index == 0)
            {
                for (int i = 0; i < DisplayData.Displays.Length; i++)
                {
                    timerStopWatches[i].Restart();

                    // Restart timer
                    timers[i].Stop();
                    timers[i].Start();

                    int indexForInvoke = i; // without doing this, i may change within the outside of the invoke, causing errors
                    IntervalWallpaperSetter(indexForInvoke);
                }
            }
        }

        private void IntervalWallpaperSetter(int index)
        {
            if (wallpapers[index].IsPlayingVideo)
            {
                //xDebug.WriteLine("Loops: " + wallpapers[index].Loops + " | Loop Min : " + OptionsData.ThemeOptions.VideoOptions.MinimumVideoLoops);
                //xDebug.WriteLine("Timer: " + wallpapers[index].WallpaperUptime.ElapsedMilliseconds + " | Timer Max: " + OptionsData.ThemeOptions.VideoOptions.MaximumVideoTime * 1000);
                if (wallpapers[index].Loops >= OptionsData.ThemeOptions.VideoOptions.MinimumVideoLoops ||
                    (wallpapers[index].WallpaperUptime.ElapsedMilliseconds >= OptionsData.ThemeOptions.VideoOptions.MaximumVideoTime * 1000 && // 1 second = 1000 milliseconds
                     OptionsData.ThemeOptions.VideoOptions.MaximumVideoTime > 0))
                {
                    this.BeginInvoke((MethodInvoker) delegate { NextWallpaper(index, true); });
                }
            }
            else
            {
                this.BeginInvoke((MethodInvoker) delegate { NextWallpaper(index, true); });
            }
        }

        // this is just a visual, no need to worry about updating all controls
        private void timerVisualUpdater_Tick(object sender, EventArgs e)
        {
            for (var i = 0; i < timers.Length; i++)
            {
                int timerIndex = DisplaySettingsSynced ? 0 : i; // if synced just use the 0 index
                Timer timer = timers[timerIndex];
                if (timer != null && timer.Interval > 1)
                {
                    displaySettingForms[i].SetTimeLeft((((int)(timer.Interval - timerStopWatches[timerIndex].ElapsedMilliseconds) / 1000) + 1));
                }
            }
        }

        public int GetTimerIndex(int index) => displaySettingForms[index].GetSelectedWallpaperIntervalIndex();

        public int[] GetTimerIndexes()
        {
            int[] indexes = new int[displaySettingForms.Length];

            for (int i = 0; i < indexes.Length; i++)
            {
                indexes[i] = GetTimerIndex(i);
            }

            return indexes;
        }

        // directly sets the timer index via code
        public void SetTimerIndex(int callingTimerIndex, int newIntervalIndex)
        {
            // TODO test the usefulness of this if statement again, confirm the index value of both the first and last spots
            if (newIntervalIndex < displaySettingForms[callingTimerIndex].GetIntervalCount())
            {
                displaySettingForms[callingTimerIndex].SetWallpaperIntervalIndex(newIntervalIndex);
            }
            else
            {
                displaySettingForms[callingTimerIndex].SetWallpaperIntervalIndex(displaySettingForms[callingTimerIndex].GetIntervalCount() - 1);
            }
            //? this will carry over to the SelectedIndexChanged event and change the timer itself
        }

        public void SetTimerIndex(int[] newIntervalIndexes, bool maintainSync)
        {
            for (int i = 0; i < newIntervalIndexes.Length; i++)
            {
                SetTimerIndex(i, newIntervalIndexes[i]);
            }

            if (maintainSync) { DisplaySettingsSynced = true; }
        }

        // updates timer index based on the combo box state
        public void UpdateTimerIndex(int index)
        {
            DisplaySettingsSynced = false;

            if (timers[index] != null)
            {
                timerStopWatches[index].Restart();
                timers[index].Interval = timerMaxIntervals[index] = GetTimerInterval(index);

                if (timers[index].Interval <= 1)
                {
                    displaySettingForms[index].SetTimeLeft("Awaiting Timer");
                    timers[index].Stop();
                }
                else
                {
                    timers[index].Start();
                    timerStopWatches[index].Start();
                }
            }

            ResetTimer(index);
        }

        public void ResetTimer(int index)
        {
            //xDebug.WriteLine("Resetting timer");
            if (timerMaxIntervals[index] != 0) // if this equals 0 then there is no timer
            {
                //xDebug.WriteLine("Timer Set");
                timers[index].Interval = timerMaxIntervals[index];
                timerStopWatches[index].Restart();
            }
        }

        private void timerAudioChecker_Tick(object sender, EventArgs e) => Task.Run(() => AudioManager.CheckForMuteConditions());
    }
}
