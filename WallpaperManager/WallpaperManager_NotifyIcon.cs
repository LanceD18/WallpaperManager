using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager
{
    public partial class WallpaperManager : Form
    {
        //? Note that OnResize() is used to open the notifyIcon

        private bool minimizeToSystemTray = true;
        private MenuItem[] wallpaperMenuItems = new MenuItem[Screen.AllScreens.Length];

        private void InitializeNotifyIcon()
        {
            // Create notifyIcon1 ContextMenu
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Open", notifyIcon1_Open);

            for (int i = 0; i < MonitorData.Screens.Length; i++)
            {
                wallpaperMenuItems[i] = cm.MenuItems.Add(i + 1 + " | Awaiting Wallpaper");
            }

            cm.MenuItems.Add("Flip Wallpapers", notifyIcon1_FlipWallpapers);
            cm.MenuItems.Add("Next Wallpaper", notifyIcon1_NextWallpaper);
            cm.MenuItems.Add("Previous Wallpaper", notifyIcon1_PreviousWallpaper);
            cm.MenuItems.Add("Set Default Theme", notifyIcon1_SetDefaultTheme);
            cm.MenuItems.Add("Exit", notifyIcon1_Exit);

            // notifyIcon1 Properties
            notifyIconWallpaperManager.ContextMenu = cm;
            notifyIconWallpaperManager.Icon = SystemIcons.WinLogo;
        }

        /// <summary>
        /// Re-opens the application and hides the notifyIcon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_Open(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIconWallpaperManager.Visible = false;
        }

        private void notifyIcon1_NextWallpaper(object sender, EventArgs e) { NextWallpaper(); }

        private void notifyIcon1_PreviousWallpaper(object sender, EventArgs e) { PreviousWallpaper(); }

        private void notifyIcon1_FlipWallpapers(object sender, EventArgs e) { Debug.WriteLine("notifyIcon1_FlipWallpapers is incomplete"); /*FlipWallpapers();*/ }

        private void notifyIcon1_SetDefaultTheme(object sender, EventArgs e) { Debug.WriteLine("notifyIcon1_SetDefaultTheme is incomplete"); /*SetDefaultTheme();*/ }

        private void notifyIcon1_Exit(object sender, EventArgs e) { Application.Exit(); }
    }
}
