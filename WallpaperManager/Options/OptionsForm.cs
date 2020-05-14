using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Options
{
    public partial class OptionsForm : Form
    {
        public ThemeOptions ThemeSettings = OptionsData.ThemeOptions;

        public OptionsForm()
        {
            InitializeComponent();
            this.FormClosed += SaveOptionsData;

            // Theme Settings
            checkBoxLargerImagesOnLargerMonitors.Checked = ThemeSettings.LargerImagesOnLargerMonitors;
            checkBoxHigherRankedImagesOnLargerMonitors.Checked = ThemeSettings.HigherRankedImagesOnLargerMonitors;
            checkBoxEnableDetectionOfInactiveImages.Checked = ThemeSettings.EnableDetectionOfInactiveImages;
            checkBoxWeightedRanks.Checked = ThemeSettings.WeightedRanks;

            // Global Settings
            labelDefaultThemePath.Text = OptionsData.DefaultTheme;

            if (File.Exists(OptionsData.DefaultTheme))
            {
                checkBoxEnableGlobalHotkey.Checked = OptionsData.EnableDefaultThemeHotkey;
            }
            else
            {
                checkBoxEnableGlobalHotkey.Checked = false;
                checkBoxEnableGlobalHotkey.AutoCheck = false;
                checkBoxEnableGlobalHotkey.ForeColor = Color.Gray;
                labelDefaultThemePath.Text = "No Default Theme selected";
            }
        }

        private void SaveOptionsData(object sender, FormClosedEventArgs e)
        {
            // Theme Settings
            ThemeSettings.LargerImagesOnLargerMonitors = checkBoxLargerImagesOnLargerMonitors.Checked;
            ThemeSettings.HigherRankedImagesOnLargerMonitors = checkBoxHigherRankedImagesOnLargerMonitors.Checked;
            ThemeSettings.EnableDetectionOfInactiveImages = checkBoxEnableDetectionOfInactiveImages.Checked;

            bool updateRankPercentiles = ThemeSettings.WeightedRanks != checkBoxWeightedRanks.Checked;
            ThemeSettings.WeightedRanks = checkBoxWeightedRanks.Checked;

            OptionsData.ThemeOptions = ThemeSettings;

            //? this won't work unless OptionsData.ThemeOptions is updated first
            if (updateRankPercentiles)
            {
                WallpaperData.UpdateRankPercentiles();
            }

            // Global Settings
            if (File.Exists(OptionsData.DefaultTheme))
            {
                OptionsData.DefaultTheme = labelDefaultThemePath.Text;
                OptionsData.EnableDefaultThemeHotkey = checkBoxEnableGlobalHotkey.Checked;
            }
        }

        private void buttonInspectRankDistribution_Click(object sender, EventArgs e)
        {
            new RankDistributionChart().Show();
        }

        private void buttonModifyMaxRank_Click(object sender, EventArgs e)
        {
            int newRankMax;
            try
            {
                newRankMax = int.Parse(Interaction.InputBox("Enter a new max rank", "Modify Max Rank", WallpaperData.GetMaxRank().ToString()));
            }
            catch // no response
            {
                return;
            }

            if (newRankMax > 0 && newRankMax != WallpaperData.GetMaxRank()) // cannot be equal to or less than 0 and this will not change anything if the same rank is chosen
            {
                if (newRankMax <= WallpaperData.LargestMaxRank)
                {
                    WallpaperData.SetMaxRank(newRankMax, false);
                }
                else
                {
                    MessageBox.Show("Cannot be larger than 1000");
                }
            }
            else
            {
                MessageBox.Show("Invalid Input");
            }
        }

        private void buttonSetDefaultTheme_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                // dialog properties
                dialog.Title = "Select a compatible json file to load";
                dialog.Filter = "Wallpaper Data (*.json) | *.json";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default["DefaultTheme"] = dialog.FileName;
                    labelDefaultThemePath.Text = dialog.FileName;

                    Properties.Settings.Default["DefaultThemeHotkey"] = true;
                    checkBoxEnableGlobalHotkey.AutoCheck = true;
                    checkBoxEnableGlobalHotkey.ForeColor = Color.White;

                    Properties.Settings.Default.Save();
                }
            }
        }

        private void buttonLoadDefaultTheme_Click(object sender, EventArgs e)
        {
            WallpaperData.LoadDefaultTheme();
        }
    }
}
