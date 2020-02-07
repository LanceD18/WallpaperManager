using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools.FormUtil;
using Microsoft.Win32;
using WallpaperManager.ApplicationData;
using WallpaperManager.Options;
using WallpaperManager.Tagging;

namespace WallpaperManager
{
    public partial class WallpaperManager : Form
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

        public WallpaperManager()
        {
            InitializeComponent();

            Application.ApplicationExit += OnApplicationExit;
            this.Load += OnLoad;
            this.Resize += OnResize;

            InitializeImageSelector();
            InitializeImageInspector();

            PathData.Validate(); // ensures that all needed folders exist
            WallpaperData.Initialize(this, false); // loads in all necessary data

            InitializeTimer();
            InitializeKeys();
            InitializeNotifyIcon();
        }

        private void InitializeKeys()
        {
            //? See if you actually need this, makes it to where key presses are detected across all forms, does this include while the app is closed too?
            //? If the above is true then this will need to have a setting in the options panel
            this.KeyPreview = true;

            // GlobalHotkey stuff
            /*
            ghShiftAlt = new GlobalHotkey(VirtualKey.SHIFT + VirtualKey.ALT, Keys.None, this);
            ghDivide = new GlobalHotkey(VirtualKey.NOMOD, Keys.Divide, this);
            ghMultiply = new GlobalHotkey(VirtualKey.NOMOD, Keys.Multiply, this);
            ghNumPad5 = new GlobalHotkey(VirtualKey.NOMOD, Keys.NumPad5, this);
            */
        }

        #region Keys
        /*
        private void HandleHotKey()
        {
            SetDefaultTheme();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == VirtualKey.WM_HOTKEY_MSG_ID)
            {
                HandleHotKey();
            }

            base.WndProc(ref m);
        }

        private void Form1_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                SetDefaultTheme();
            }
        } // KEYPRESS METHOD HERE!!! <----------------------
        */
        #endregion

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Debug.WriteLine("OnApplicationExit is incomplete");

            //WallpaperData.SaveDefaultData();

            // Set Default theme if enabled
            /*
            if (setDefaultThemeOnExit)
            {
                SetDefaultTheme();
            }
            */

            // Unregister keys
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

        //? Is this even necessary?
        //? Ensure that I don't overwrite Form.OnLoad | I don't think this does
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

        //? Ensure that I don't overwrite Form.OnResize | I don't think this does
        private void OnResize(object sender, EventArgs e)
        {
            // minimizes the program to the system tray, "hiding" it
            if (WindowState == FormWindowState.Minimized && minimizeToSystemTray)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void NextWallpaper()
        {
            if (!WallpaperData.FileDataIsEmpty())
            {
                //TODO Find a way to prevent the first previous wallpaper from being filled with empty strings
                ResetTimer();
                string[] previousWallpapers = new string[PathData.ActiveWallpapers.Length];
                PathData.ActiveWallpapers.CopyTo(previousWallpapers, 0);
                PathData.PreviousWallpapers.Push(previousWallpapers);

                PathData.RandomizeWallpapers();
                SetWallpaper();
            }
            else
            {
                MessageBox.Show("No wallpapers were found");
            }
        }

        private void PreviousWallpaper()
        {
            if (PathData.PreviousWallpapers.Count > 1) // the first wallpaper will be filled with empty strings
            {
                ResetTimer();
                PathData.PreviousWallpapers.Pop().CopyTo(PathData.ActiveWallpapers, 0);
                SetWallpaper();
            }
            else
            {
                MessageBox.Show("There are no more previous wallpapers");
            }
        }

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
    }
}
