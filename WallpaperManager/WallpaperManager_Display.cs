using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;
using WallpaperManager.Controls;

namespace WallpaperManager
{
    public partial class WallpaperManagerForm : Form
    {
        private WallpaperManagerDisplaySettings[] displaySettingForms;
        private WallpaperStyle[] wallpaperStyles;

        public bool DisplaySettingsSynced { get; private set; }

        private void InitializeDisplayTabControl()
        {
            int displayCount = DisplayData.Displays.Length;
            displaySettingForms = new WallpaperManagerDisplaySettings[displayCount];
            wallpaperStyles = new WallpaperStyle[displayCount];

            tabControlDisplaySettings.TabPages.Clear(); // removes the default page shown in the designer for clarity purposes
            for (int i = 0; i < displayCount; i++) // skipping the first page since it's already done
            {
                TabPage tabPage = new TabPage();
                tabPage.Text = "Display " + (i + 1);
                tabPage.BackColor = Color.Black;

                WallpaperManagerDisplaySettings displaySettings = new WallpaperManagerDisplaySettings(i);
                displaySettings.Bounds = tabPage.Bounds;
                tabPage.Controls.Add(displaySettings);
                displaySettingForms[i] = displaySettings;

                tabControlDisplaySettings.TabPages.Add(tabPage);
            }
        }

        private void buttonSyncDisplaySettings_Click(object sender, EventArgs e)
        {
            int activeIndex = tabControlDisplaySettings.SelectedIndex;
            WallpaperManagerDisplaySettings activeDisplaySettings = displaySettingForms[activeIndex];

            for (int i = 0; i < displaySettingForms.Length; i++)
            {
                if (i != activeIndex)
                {
                    displaySettingForms[i].SetWallpaperStyle(activeDisplaySettings.GetWallpaperStyle());
                    displaySettingForms[i].SetWallpaperIntervalIndex(activeDisplaySettings.GetSelectedWallpaperIntervalIndex());
                }

                ResetTimer(i);
            }

            DisplaySettingsSynced = true;
        }
    }
}
