using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioSwitcher.AudioApi.CoreAudio;
using LanceTools.FormUtil;
using LibVLCSharp.Shared;
using Microsoft.Win32;
using Mpv.NET.Player;
using Vlc.DotNet.Forms;
using WallpaperManager.ApplicationData;
using WallpaperManager.Controls;
using WallpaperManager.Options;
using WallpaperManager.Pathing;
using WallpaperManager.Tagging;
using WallpaperManager.WallpaperForm;

namespace WallpaperManager
{
    public partial class WallpaperManagerForm : Form
    {
        // Essentials
        private const int SetDeskWallpaper = 20;
        private const int UpdateIniFile = 0x01;
        private const int SendWinIniChange = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        #region Wallpaper Fade
        // Fade Wallpaper | Not yet implemented
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessageTimeout(
            IntPtr hWnd,      // handle to destination window
            uint Msg,       // message
            IntPtr wParam,  // first message parameter
            IntPtr lParam,   // second message parameter
            uint fuFlags,
            uint uTimeout,
            out IntPtr result

        );
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, IntPtr ZeroOnly);

        IntPtr result = IntPtr.Zero;
        //? The below should allow for wallpaper fading with the above code, test this out sometime
        //? SendMessageTimeout(FindWindow("Program", IntPtr.Zero), 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 500, out result);
        #endregion

        private GlobalHotkey ghShiftAlt;

        public WallpaperManagerForm()
        {
            WallpaperData.WallpaperManagerForm = this; //? this needs to be at the very front for a few other initializers

            InitializeComponent();

            Application.ApplicationExit += OnApplicationExit;
            this.Load += OnLoad;
            this.Resize += OnResize;
            this.Closing += OnClosing;

            DisplayData.Initialize();
            InitializeDisplayTabControl();

            InitializeImageSelector();
            InitializeImageInspector();

            WallpaperPathSetter.Validate(); // ensures that all needed folders exist
            WallpaperData.Initialize(false); // loads in all necessary data
            OptionsData.Initialize();

            InitializeWallpapers();
            InitializeTimer();
            InitializeKeys();
            InitializeNotifyIcon();
        }

        private void OnClosing(object sender, EventArgs e)
        {
            inspector_mpvPlayer.Stop(); //? Processing this in OnApplicationExit will cause a crash, not processing it at all will send an error on exit
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            // Upon closing the application you'll revert back to your default, windows wallpapers
            SystemParametersInfo(SetDeskWallpaper, 0, null, UpdateIniFile | SendWinIniChange);

            foreach (WallpaperForm.WallpaperForm wallpaper in wallpapers)
            {
                if (!wallpaper.InvokeRequired)
                {
                    wallpaper.Close();
                }
                else
                {
                    wallpaper.Invoke((MethodInvoker) delegate { wallpaper.Close(); });
                }
            }

            // With the current method, SystemParametersInfo() is no longer needed as it is never touched
            /*x
            // Upon closing the application you'll revert back to your default, windows wallpapers
            //? Pretty sure you don't need this but I'm posting this here for just in case the invoke fails and the wallpaper continues to draw (This happened twice)
            // TODO Thoroughly test the use of this copy
            SystemParametersInfo(SetDeskWallpaper, 0, null, UpdateIniFile | SendWinIniChange);
            */

            // TODO Unregister keys (Not sure if this is needed actually but won't hurt to add)
            /*
            foreach (GlobalHotKey key in hotkeys)
            {
                if (key.Unregister())
                {
                    //! You still need to implement the ToString method for GlobalHotKey!!!
                    MessageBox.Show("Hotkey " + key.ToString() + " failed to unregister!");
                }
            }
            */
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Debug.WriteLine("OnLoad is incomplete");

            /*
            //? You should probably move this to InitializeKeys!
            //? Also check if this even needs to be placed in OnLoad as InitializeKeys() seems more appropriate
            foreach (GlobalHotKey key in hotkeys)
            {
                if (!key.Register())
                {
                    //! You still need to implement the ToString method for GlobalHotKey!!!
                    //? Consider adding an option for auto-loading the default theme if a hotkey fails to register
                    MessageBox.Show("Hotkey " + key.ToString() + " failed to register!");
                }
            }
            */
        }

        private void OnResize(object sender, EventArgs e)
        {
            // minimizes the program to the system tray, "hiding" it
            if (WindowState == FormWindowState.Minimized && minimizeToSystemTray)
            {
                Hide();
                notifyIconWallpaperManager.Visible = true;
                DeactivateImageInspector(); // turn off the inspector if it's on, otherwise videos will continue to play in the background (with audio)
            }
        }

        #region Hotkeys
        private void InitializeKeys()
        {
            //? See if you actually need this, makes it to where key presses are detected across all forms, does this include while the app is closed too?
            //? If the above is true then this will need to have a setting in the options panel
            this.KeyPreview = true;

            // GlobalHotkey
            ghShiftAlt = new GlobalHotkey(VirtualKey.SHIFT + VirtualKey.ALT, Keys.None, this);
            //ghDivide = new GlobalHotkey(VirtualKey.NOMOD, Keys.Divide, this);
            //ghMultiply = new GlobalHotkey(VirtualKey.NOMOD, Keys.Multiply, this);
            //ghNumPad5 = new GlobalHotkey(VirtualKey.NOMOD, Keys.NumPad5, this);

            if (!ghShiftAlt.Register())
            {
                MessageBox.Show("Hotkey failed to register!");
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == VirtualKey.WM_HOTKEY_MSG_ID)
            {
                HandleHotKey();
            }

            base.WndProc(ref m);
        }

        private void HandleHotKey()
        {
            // opens the default theme
            if (OptionsData.EnableDefaultThemeHotkey)
            {
                WallpaperData.SaveData(WallpaperPathSetter.ActiveWallpaperTheme);
                WallpaperData.LoadDefaultTheme();
            }
        }
        #endregion

        #region Button Events
        private void buttonNextWallpaper_Click(object sender, EventArgs e)
        {
            NextWallpaper();
        }

        private void buttonPreviousWallpaper_Click(object sender, EventArgs e)
        {
            PreviousWallpaper();
        }

        private void buttonOptions_Click(object sender, EventArgs e)
        {
            using (OptionsForm f = new OptionsForm())
            {
                f.ShowDialog();
            }
        }

        private TagForm activeTagForm;
        private void buttonTagSettings_Click(object sender, EventArgs e)
        {
            // Show() will dispose itself on close, ShowDialog() will not however | Show() will allow you to click on other controls while ShowDialog() will not
            if (activeTagForm == null || activeTagForm.IsDisposed)
            {
                activeTagForm = new TagForm();
                activeTagForm.Show();
            }
        }
        #endregion

        private void WallpaperManager_Click(object sender, EventArgs e)
        {
            labelImageSize.Focus(); // you can't focus onto the form but this acts like what you'd want
        }

        public void ResetWallpaperManager()
        {
            ClearImageSelector();
            ClearImageFolders();
        }

        public WallpaperForm.WallpaperForm[] GetWallpapers() => wallpapers;
    }
}
