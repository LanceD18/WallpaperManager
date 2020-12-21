using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
            //TODO wallpapers[0].BackgroundImage

            //?-----Fetch the Program window-----
            IntPtr program = Win32.FindWindow("Program");

            //?-----Spawn a WorkerW behind the desktop icons (If it is already there, nothing happens)-----
            IntPtr result = IntPtr.Zero;
            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            Win32.SendMessageTimeout(program,
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
                wallpapers[i] = new WallpaperForm(MonitorData.Screens[i], workerw, WallpaperStyle);
            }
        }

        private void SetFormWallpaper()
        {
            for (int i = 0; i < wallpapers.Length; i++)
            {
                //wallpapers[i].SetWallpaper(Image.FromFile(PathData.ActiveWallpapers[i]));
                wallpapers[i].SetWallpaper(PathData.ActiveWallpapers[i]);

                Debug.WriteLine("Is Wallpaper " + i + " visible? " + wallpapers[i].Visible);
                if (!wallpapers[i].Visible)
                {
                    Debug.WriteLine("Showing Wallpaper " + i);
                    wallpapers[i].Show();
                }
            }
        }
    }
}
