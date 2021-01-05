using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools.Mpv;
using Mpv.NET.Player;
using WallpaperManager.ApplicationData;
using WallpaperManager.Wallpaper;

namespace WallpaperManager
{
    //? Implementation of wallpaper via placing a borderless form behind the desktop icons
    public partial class WallpaperManagerForm : Form
    {
        private WallpaperForm[] wallpapers;

        // Derived from: https://www.codeproject.com/Articles/856020/Draw-Behind-Desktop-Icons-in-Windows-plus

        public static IntPtr GetDesktopWorkerW()
        {
            //?-----Fetch the Program window-----
            IntPtr progman = Win32.FindWindow("Progman", null); // progman (not program) allows the form to be represented as a child window of the desktop itself

            //?-----Spawn a WorkerW behind the desktop icons (If it is already there, nothing happens)-----
            IntPtr result = IntPtr.Zero;
            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            Win32.SendMessageTimeout(progman,
                0x052C,
                new IntPtr(0),
                IntPtr.Zero,
                Win32.SendMessageTimeoutFlags.SMTO_NORMAL,
                1000,
                out result);

            //?-----Find the Window that's underneath the desktop icons-----
            // Spy++ output
            // .....
            // 0x00010190 "" WorkerW
            //   ...
            //   0x000100EE "" SHELLDLL_DefView
            //     0x000100F0 "FolderView" SysListView32
            // 0x00100B8A "" WorkerW       <-- This is the WorkerW instance we are after!
            // 0x000100EC "Program Manager" Progman

            IntPtr workerw = IntPtr.Zero;

            // We enumerate all Windows, until we find one, that has the SHELLDLL_DefView 
            // as a child. 
            // If we found that window, we take its next sibling and assign it to workerw.
            Win32.EnumWindows(new Win32.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = Win32.FindWindowEx(tophandle,
                    IntPtr.Zero,
                    "SHELLDLL_DefView",
                    String.Empty);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = Win32.FindWindowEx(IntPtr.Zero,
                        tophandle,
                        "WorkerW",
                        String.Empty);
                }

                return true;
            }), IntPtr.Zero);

            return workerw;
        }


        //? Note: For this function to work, the form has to be already created. The form.Load event seems to be the right place for it.
        private void InitializeWallpapers()
        {
            IntPtr workerw = GetDesktopWorkerW();

            int monitorCount = DisplayData.Displays.Length;
            wallpapers = new WallpaperForm[monitorCount];
            for (int i = 0; i < monitorCount; i++)
            {
                wallpapers[i] = new WallpaperForm(DisplayData.Displays[i], workerw);
            }
        }

        private void SetWallpaper(int index)
        {
            //-----Update Notify Icons-----
            string wallpaperPath = PathData.ActiveWallpapers[index];
            string wallpaperName = new FileInfo(wallpaperPath).Name; // pathless string of file name

            wallpaperMenuItems[index].Text = WallpaperData.ContainsImage(wallpaperPath) ?
                index + 1 + " | R: " + WallpaperData.GetImageRank(wallpaperPath) + " | " + wallpaperName :
                index + 1 + " | [NOT FOUND]" + wallpaperName; wallpaperMenuItems[index].Text = WallpaperData.ContainsImage(wallpaperPath) ? index + 1 + " | R: " + WallpaperData.GetImageRank(wallpaperPath) + " | " + wallpaperName : index + 1 + " | [NOT FOUND]" + wallpaperName;

            //? without this, if the inspector wasn't open prior to setting a new wallpaper it would never allow it's MvpPlayer to display
            // TODO find a permanent solution to this | Using a different player such as VLC did not work
            WallpaperData.WallpaperManagerForm.FixInspectorPlayer(PathData.ActiveWallpapers[index]);
            //-----Update Wallpaper Forms-----
            //? This needs to be above the call to WallpaperForum's SetWallpaper() otherwise the form will call its load event second & override some settings
            //! the moment this is shown, any new mpvplayers will stop working, only the ones that were player previously will continue to function
            if (!wallpapers[index].Visible) wallpapers[index].Show(); // this is processed only once after the first wallpaper change
            wallpapers[index].SetWallpaper(PathData.ActiveWallpapers[index]);
        }

        private bool fixAdministered = false;
        public bool FixInspectorPlayer(string video)
        {
            if (!inspector_mpvPlayer.IsPlaying && !fixAdministered)
            {
                InspectedImage = video;
                ActivateImageInspector();
                DeactivateImageInspector();
                fixAdministered = true;
                return true;
            }

            return false;
        }

        public void NextWallpaper()
        {
            // sets all wallpapers to their next wallpaper
            for (int i = 0; i < DisplayData.Displays.Length; i++)
            {
                NextWallpaper(false, i);
            }
        }

        public void NextWallpaper(bool ignoreErrorMessages)
        {
            // sets all wallpapers to their next wallpaper
            for (int i = 0; i < DisplayData.Displays.Length; i++)
            {
                NextWallpaper(ignoreErrorMessages, i);
            }
        }

        public void NextWallpaper(bool ignoreErrorMessages, int wallpaperIndex)
        {
            Debug.WriteLine("Next Wallpaper Index: " + wallpaperIndex);
            if (!WallpaperData.IsLoadingData) // Rank Percentiles won't be properly set-up until after a theme is loaded, which can cause a crash is NextWallpaper is called
            {
                if (!WallpaperData.FileDataIsEmpty())
                {
                    if (!WallpaperData.NoImagesActive() && WallpaperData.GetAllRankedImages().Length != 0)
                    {
                        //TODO Prevent the first previous wallpaper from being filled with empty strings
                        ResetTimer(wallpaperIndex);
                        string[] previousWallpapers = new string[PathData.ActiveWallpapers.Length];
                        PathData.ActiveWallpapers.CopyTo(previousWallpapers, 0);
                        PathData.PreviousWallpapers.Push(previousWallpapers);

                        if (PathData.RandomizeWallpapers()) SetWallpaper(wallpaperIndex); // randomize wallpaper will check if it even can randomize the wallpapers first
                    }
                    else
                    {
                        if (!ignoreErrorMessages) MessageBox.Show("No active wallpapers were found");
                    }
                }
                else
                {
                    if (!ignoreErrorMessages) MessageBox.Show("Add some wallpapers first! Use the Add Folder button to add a collection of images that'll be used as potential wallpapers");
                }
            }
        }

        public void PreviousWallpaper()
        {
            // sets all wallpapers to their previous wallpaper
            for (int i = 0; i < DisplayData.Displays.Length; i++)
            {
                PreviousWallpaper(i);
            }
        }

        public void PreviousWallpaper(int wallpaperIndex)
        {
            if (PathData.PreviousWallpapers.Count > 1) // the first wallpaper will be filled with empty strings
            {
                ResetTimer(wallpaperIndex);
                PathData.PreviousWallpapers.Pop().CopyTo(PathData.ActiveWallpapers, 0);
                SetWallpaper(wallpaperIndex);
            }
            else
            {
                MessageBox.Show("There are no more previous wallpapers");
            }
        }
    }
}