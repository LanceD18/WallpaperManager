using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSCore.CoreAudioAPI;
using LanceTools.WindowUtil;
using WallpaperManager.ApplicationData;
using WallpaperManager.Options;
using WallpaperManager.Pathing;

namespace WallpaperManager
{
    public static class AudioManager
    {
        public static bool IsWallpapersMuted { get; private set; }

        public static void CheckForMuteConditions()
        {
            Thread thread = new Thread(() =>
            {
                bool muted = false;

                void ProcessMute()
                {
                    MuteWallpapers();
                    muted = true;
                }

                if (OptionsData.ThemeOptions.VideoOptions.MuteIfApplicationFocused && !muted)
                {
                    Process activeWindow = WindowTools.GetActiveWindowProcess();
                    if (activeWindow.ProcessName != Process.GetCurrentProcess().ProcessName)
                    {
                        WindowPlacementStyle windowStyle = WindowTools.GetWindowStyle(activeWindow);
                        if (windowStyle == WindowPlacementStyle.Normal || windowStyle == WindowPlacementStyle.Maximized)
                        {
                            ProcessMute();
                        }
                    }
                }

                if (OptionsData.ThemeOptions.VideoOptions.MuteIfApplicationMaximized && !muted) // every window needs to be checked for maximization
                {
                    //xStopwatch test = new Stopwatch();
                    //xtest.Start();
                    foreach (Process p in Process.GetProcesses()) //? has the potential to take up a decent CPU load, not noticeable on the thread but still impactful
                    {
                        WindowPlacementStyle windowStyle = WindowTools.GetWindowStyle(p);

                        if (windowStyle == WindowPlacementStyle.Maximized)
                        {
                            ProcessMute();
                            break;
                        }
                    }
                    //xtest.Stop();
                    //xDebug.WriteLine("Ms taken to check for maximized app: " + test.ElapsedMilliseconds);
                }

                if (OptionsData.ThemeOptions.VideoOptions.MuteIfAudioPlaying && !muted) muted = CheckForExternalAudio();  //? CheckForExternalAudio cannot be done on the UI thread | async doesn't fix this

                if (IsWallpapersMuted && !muted) UnmuteWallpapers();
            });
            thread.Start();

            //x while (thread.IsAlive) { /* do nothing | Thread.Join() will just freeze the application */ } << this is only needed if you're returning something
        }

        private static bool CheckForExternalAudio()
        {
            WallpaperManagerForm wallpaperManagerForm = WallpaperData.WallpaperManagerForm;

            using (var sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
            {
                using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                {
                    //? The name of the video playing on the WallpaperForm will definitely be given, so check if
                    //? anything BUT those are playing and if so mute the wallpaper
                    foreach (var session in sessionEnumerator)
                    {
                        List<string> potentialNames = new List<string>();
                        foreach (string wallpaper in WallpaperPathing.ActiveWallpapers)
                        {
                            if (File.Exists(wallpaper))
                            {
                                if (WallpaperManagerTools.IsSupportedVideoType(wallpaper)) // only videos should be checked
                                {
                                    potentialNames.Add(new FileInfo(wallpaper).Name);
                                }
                            }
                        }

                        // format of session.DisplayName for videos: videoName.extension - extension | We only want videoName.extension, cut off the first space
                        string sessionName = session.DisplayName;
                        string sessionVideoName = !sessionName.Contains(' ') ? sessionName : sessionName.Substring(0, sessionName.IndexOf(' '));

                        if (!potentialNames.Contains(sessionVideoName)) // checking an audio source that doesn't match up with to the active wallpapers
                        {
                            using (var audioMeterInformation = session.QueryInterface<AudioMeterInformation>())
                            {
                                if (audioMeterInformation.GetPeakValue() > 0) // if the volume of this application is greater than 0, mute all wallpapers
                                {
                                    Debug.WriteLine("External Audio Playing: " + session.DisplayName);
                                    //xDebug.WriteLine(audioMeterInformation.GetPeakValue());
                                    MuteWallpapers();
                                    return true;
                                }
                            }
                        }
                        else if (wallpaperManagerForm.IsViewingInspector &&
                                 new FileInfo(wallpaperManagerForm.InspectedImage).Name == sessionVideoName) // this indicates that the inspector is viewing an active wallpaper
                        {
                            using (var audioMeterInformation = session.QueryInterface<AudioMeterInformation>())
                            {
                                if (audioMeterInformation.GetPeakValue() > 0) // if the volume of the current inspector video is greater than 0, mute all wallpapers
                                {
                                    Debug.WriteLine("Inspector Audio Playing: " + session.DisplayName);
                                    //xDebug.WriteLine(audioMeterInformation.GetPeakValue());
                                    MuteWallpapers();
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static AudioSessionManager2 GetDefaultAudioSessionManager2(DataFlow dataFlow)
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(dataFlow, Role.Multimedia))
                {
                    //xDebug.WriteLine("DefaultDevice: " + device.FriendlyName);
                    var sessionManager = AudioSessionManager2.FromMMDevice(device);
                    return sessionManager;
                }
            }
        }

        private static void MuteWallpapers()
        {
            Debug.WriteLine("Mute");
            foreach (var wallpaper in WallpaperData.WallpaperManagerForm.GetWallpapers()) wallpaper.Mute();
            IsWallpapersMuted = true;
        }

        private static void UnmuteWallpapers()
        {
            Debug.WriteLine("Unmute");
            foreach (var wallpaper in WallpaperData.WallpaperManagerForm.GetWallpapers()) wallpaper.Unmute();
            IsWallpapersMuted = false;
        }
    }
}