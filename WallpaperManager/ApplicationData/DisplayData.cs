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
using LanceTools;
using LanceTools.FormUtil;

namespace WallpaperManager.ApplicationData
{
    public static class DisplayData
    {
        public static readonly Screen[] Displays = Screen.AllScreens;
        //? Screen.AllScreens seems to do this by default due to the way it functions, I wouldn't rely on that though as there's no documentation for it
        private static int[] LargestDisplayOrderIndex =
        (
            from s in Screen.AllScreens
            orderby s.Bounds.Width + s.Bounds.Height descending
            select Screen.AllScreens.IndexOf(s)
        ).ToArray();

        private static int displayNum;
        private static List<int> displayNumbering = new List<int>();

        // Used to help set up monitor bounds & adjustments
        public static int TotalDisplayWidth { get; private set; } // total width of all monitors combined
        public static int MaxDisplayHeight { get; private set; } // height of the tallest monitor, maximum possible height
        public static int DisplayXAdjustment { get; private set; }
        public static int MinDisplayY { get; private set; } = int.MaxValue;

        public static void Initialize()
        {
            // Set up monitor bounds & adjustments
            foreach (Screen display in Displays)
            {
                if (display.Bounds.X < 0) // used to prevent wallpapers from being drawn off the screen
                {
                    DisplayXAdjustment += Math.Abs(display.Bounds.X);
                }

                if (display.Bounds.Y < MinDisplayY)
                {
                    MinDisplayY = display.Bounds.Y;
                }

                TotalDisplayWidth += display.Bounds.Width;
                MaxDisplayHeight = Math.Max(MaxDisplayHeight, display.Bounds.Height);
            }
        }

        public static void ReorderDisplays()
        {
            Button[] displayButtons = new Button[Displays.Length];
            for (int i = 0; i < displayButtons.Length; i++)
            {
                displayButtons[i] = new Button();
                displayButtons[i].Text = (i + 1).ToString();
                displayButtons[i].Click += OnMonitorButtonsClick;
            }

            // **Reorders MonitorData to a user selected order**
            Screen[] TempDisplayData = new Screen[Displays.Length];
            displayNumbering.Clear();
            for (int i = 0; i < Displays.Length; i++)
            {
                Screen display = Displays[i];
                MessageBoxDynamic.Show("Identify Display", "Identify the following display:\n" + display.DeviceName + "\n" + display.Bounds, displayButtons, true);
                TempDisplayData[displayNum] = Displays[i];
                displayNumbering.Add(displayNum);
            }
            TempDisplayData.CopyTo(Displays, 0);

            ResetLargestDisplayIndexOrder();
        }

        public static void ResetLargestDisplayIndexOrder()
        {
            LargestDisplayOrderIndex =
            (
                from s in Displays
                orderby s.Bounds.Width + s.Bounds.Height descending
                select Displays.IndexOf(s)
            ).ToArray();
        }

        public static int[] GetLargestDisplayIndexOrder()
        {
            return LargestDisplayOrderIndex;
        }

        private static void OnMonitorButtonsClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            displayNum = int.Parse(button.Text) - 1;
            button.Visible = false;
        }
    }
}
