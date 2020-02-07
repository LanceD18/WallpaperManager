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
    public static class MonitorData
    {
        public static readonly Screen[] Screens = Screen.AllScreens;
        //? Screen.AllScreens seems to do this by default due to the way it functions, I wouldn't rely on that though as there's no documentation for it
        private static int[] LargestMonitorOrderIndex =
        (
            from s in Screen.AllScreens
            orderby s.Bounds.Width + s.Bounds.Height descending
            select Screen.AllScreens.IndexOf(s)
        ).ToArray();

        private static int displayNum;
        private static List<int> displayNumbering = new List<int>();

        public static void ReorderMonitors()
        {
            Button[] monitorButtons = new Button[Screens.Length];
            for (int i = 0; i < monitorButtons.Length; i++)
            {
                monitorButtons[i] = new Button();
                monitorButtons[i].Text = (i + 1).ToString();
                monitorButtons[i].Click += OnMonitorButtonsClick;
            }

            // **Reorders MonitorData to a user selected order**
            Screen[] TempMonitorData = new Screen[Screens.Length];
            displayNumbering.Clear();
            for (int i = 0; i < Screens.Length; i++)
            {
                Screen monitor = Screens[i];
                MessageBoxDynamic.Show("Identify Display", "Identify the following display:\n" + monitor.DeviceName + "\n" + monitor.Bounds, monitorButtons, true);
                TempMonitorData[displayNum] = Screens[i];
                displayNumbering.Add(displayNum);
            }
            TempMonitorData.CopyTo(Screens, 0);

            ResetLargestMonitorIndexOrder();
        }

        public static void ResetLargestMonitorIndexOrder()
        {
            LargestMonitorOrderIndex =
            (
                from s in Screens
                orderby s.Bounds.Width + s.Bounds.Height descending
                select Screens.IndexOf(s)
            ).ToArray();
        }

        public static int[] GetLargestMonitorIndexOrder()
        {
            return LargestMonitorOrderIndex;
        }

        private static void OnMonitorButtonsClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            displayNum = int.Parse(button.Text) - 1;
            button.Visible = false;
        }
    }
}
