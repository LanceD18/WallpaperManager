using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager
{
    //? Implementation of wallpaper via placing a borderless form behind the desktop icons
    public partial class WallpaperManager : Form
    {
        private WallpaperForm[] wallpapers;

        // Derived from: https://www.codeproject.com/Articles/856020/Draw-Behind-Desktop-Icons-in-Windows-plus
        private IntPtr GetWorkerW()
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
                    string.Empty);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = Win32.FindWindowEx(IntPtr.Zero,
                        tophandle,
                        "WorkerW",
                        string.Empty);
                }

                return true;
            }), IntPtr.Zero);

            return workerw;
        }

        //? Note: For this function to work, the form has to be already created. The form.Load event seems to be the right place for it.
        private void InitializeWallpapers()
        {
            IntPtr workerw = GetWorkerW();

            int monitorCount = MonitorData.Screens.Length;
            wallpapers = new WallpaperForm[monitorCount];
            for (int i = 0; i < monitorCount; i++)
            {
                wallpapers[i] = new WallpaperForm(MonitorData.Screens[i], workerw);
            }
        }

        private void SetWallpaper()
        {
            for (int i = 0; i < PathData.ActiveWallpapers.Length; i++)
            {
                //-----Update Notify Icons-----
                string wallpaperPath = PathData.ActiveWallpapers[i];
                string wallpaperName = new FileInfo(wallpaperPath).Name; // pathless string of file name

                wallpaperMenuItems[i].Text = WallpaperData.ContainsImage(wallpaperPath) ? 
                    i + 1 + " | R: " + WallpaperData.GetImageRank(wallpaperPath) + " | " + wallpaperName : 
                    i + 1 + " | [NOT FOUND]" + wallpaperName;

                //-----Update Wallpaper Forms-----
                wallpapers[i].Invoke((MethodInvoker) delegate
                {
                    //? This needs to be above the call to WallpaperForum's SetWallpaper() otherwise the form will call its load event second & override some settings
                    if (!wallpapers[i].Visible) wallpapers[i].Show(); // this is processed only once after the first wallpaper change
                    wallpapers[i].SetWallpaper(PathData.ActiveWallpapers[i]);
                });
            }
        }
    }
}
