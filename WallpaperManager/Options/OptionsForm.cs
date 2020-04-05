using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public OptionsForm()
        {
            InitializeComponent();

            labelDefaultThemePath.Text = Properties.Settings.Default["DefaultTheme"] as string;

            checkBoxLargerImagesOnLargerMonitors.Checked = OptionsData.LargerImagesOnLargerMonitors;
            checkBoxHigherRankedImagesOnLargerMonitors.Checked = OptionsData.HigherRankedImagesOnLargerMonitors;
            checkBoxEnableDetectionOfInactiveImages.Checked = OptionsData.EnableDetectionOfInactiveImages;
            checkBoxEnableGlobalHotkey.Checked = OptionsData.EnableGlobalHotkey;

            this.FormClosed += SaveOptionsData;
        }

        private void SaveOptionsData(object sender, FormClosedEventArgs e)
        {
            OptionsData.LargerImagesOnLargerMonitors = checkBoxLargerImagesOnLargerMonitors.Checked;
            OptionsData.HigherRankedImagesOnLargerMonitors = checkBoxHigherRankedImagesOnLargerMonitors.Checked;
            OptionsData.EnableDetectionOfInactiveImages = checkBoxEnableDetectionOfInactiveImages.Checked;
            OptionsData.EnableGlobalHotkey = checkBoxEnableGlobalHotkey.Checked;
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
                    WallpaperData.SetRankMax(newRankMax, false);
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
                    Properties.Settings.Default.Save();
                    labelDefaultThemePath.Text = dialog.FileName;
                }
            }
        }

        private void buttonLoadDefaultTheme_Click(object sender, EventArgs e)
        {
            WallpaperData.LoadDefaultTheme();
        }
    }
}
