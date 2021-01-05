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
using LanceTools;
using Microsoft.VisualBasic;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Options
{
    public partial class OptionsForm : Form
    {
        public ThemeOptions ThemeOptions;

        public OptionsForm()
        {
            InitializeComponent();
            this.FormClosed += SaveOptionsData;

            toolTipEnableDetectionOfInactiveImages.SetToolTip(checkBoxEnableDetectionOfInactiveImages, "Allows inactive images to be detected by the image selector");

            ThemeOptions = OptionsData.ThemeOptions;

            SuspendLayout();

            // Initialize Frequency Properties
            ResetFrequencyText();
            InitializeFrequencyEvents();

            //-----Theme Settings-----
            checkBoxLargerImagesOnLargerDisplays.Checked = ThemeOptions.LargerImagesOnLargerDisplays;
            checkBoxHigherRankedImagesOnLargerDisplays.Checked = ThemeOptions.HigherRankedImagesOnLargerDisplays;
            checkBoxEnableDetectionOfInactiveImages.Checked = ThemeOptions.EnableDetectionOfInactiveImages;
            checkBoxWeightedRanks.Checked = ThemeOptions.WeightedRanks;

            checkBoxAudioPlaying.Checked = ThemeOptions.MuteIfAudioPlaying;
            checkBoxApplicationMaximized.Checked = ThemeOptions.MuteIfApplicationMaximized;
            checkBoxApplicationFocused.Checked = ThemeOptions.MuteIfApplicationFocused;

            //-----Global Settings-----
            if (File.Exists(OptionsData.DefaultTheme))
            {
                labelDefaultThemePath.Text = OptionsData.DefaultTheme;
                checkBoxEnableGlobalHotkey.Checked = OptionsData.EnableDefaultThemeHotkey;
            }
            else
            {
                checkBoxEnableGlobalHotkey.Checked = false;
                checkBoxEnableGlobalHotkey.AutoCheck = false;
                checkBoxEnableGlobalHotkey.ForeColor = Color.Gray;
                labelDefaultThemePath.Text = "No Default Theme selected";
            }

            ResumeLayout();
        }

        private void SaveOptionsData(object sender, FormClosedEventArgs e)
        {
            //-----Theme Settings-----
            ThemeOptions.LargerImagesOnLargerDisplays = checkBoxLargerImagesOnLargerDisplays.Checked;
            ThemeOptions.HigherRankedImagesOnLargerDisplays = checkBoxHigherRankedImagesOnLargerDisplays.Checked;
            ThemeOptions.EnableDetectionOfInactiveImages = checkBoxEnableDetectionOfInactiveImages.Checked;

            bool updateRankPercentiles = ThemeOptions.WeightedRanks != checkBoxWeightedRanks.Checked;
            ThemeOptions.WeightedRanks = checkBoxWeightedRanks.Checked;

            ThemeOptions.MuteIfAudioPlaying = checkBoxAudioPlaying.Checked;
            ThemeOptions.MuteIfApplicationMaximized = checkBoxApplicationMaximized.Checked;
            ThemeOptions.MuteIfApplicationFocused = checkBoxApplicationFocused.Checked;

            // note that frequency settings had their opportunity to be changed throughout the form's lifespan

            OptionsData.ThemeOptions = ThemeOptions;

            //! This won't work unless OptionsData.ThemeOptions is updated first | Used to alter weighted rank changes
            if (updateRankPercentiles) WallpaperData.UpdateRankPercentiles(ImageType.None); //! Now that image types exist this preemptive change may not be worth it

            //-----Global Settings-----
            if (File.Exists(OptionsData.DefaultTheme))
            {
                OptionsData.DefaultTheme = labelDefaultThemePath.Text;
                OptionsData.EnableDefaultThemeHotkey = checkBoxEnableGlobalHotkey.Checked;
                Properties.Settings.Default.DefaultTheme = labelDefaultThemePath.Text;
                Properties.Settings.Default.DefaultThemeHotkey = checkBoxEnableGlobalHotkey.Checked;
            }

            Properties.Settings.Default.Save();
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
                    WallpaperData.SetMaxRank(newRankMax);
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

        #region Frequency
        private void InitializeFrequencyEvents()
        {
            // Updates Frequency when losing focus of the control
            textBoxRelativeStatic.LostFocus += (s, e) => ResetFrequencyText();
            textBoxRelativeGIF.LostFocus += (s, e) => ResetFrequencyText();
            textBoxRelativeVideo.LostFocus += (s, e) => ResetFrequencyText();

            textBoxExactStatic.LostFocus += (s, e) => ResetFrequencyText();
            textBoxExactGIF.LostFocus += (s, e) => ResetFrequencyText();
            textBoxExactVideo.LostFocus += (s, e) => ResetFrequencyText();

            // Selects the entire text upon clicking the textBox
            textBoxRelativeStatic.Click += (s, e) => textBoxRelativeStatic.SelectAll();
            textBoxRelativeGIF.Click += (s, e) => textBoxRelativeGIF.SelectAll();
            textBoxRelativeVideo.Click += (s, e) => textBoxRelativeVideo.SelectAll();

            textBoxExactStatic.Click += (s, e) => textBoxExactStatic.SelectAll();
            textBoxExactGIF.Click += (s, e) => textBoxExactGIF.SelectAll();
            textBoxExactVideo.Click += (s, e) => textBoxExactVideo.SelectAll();

            // Updates Frequency on pressing the enter key
            // the SuppressKeyPress prevents the ding audio when pressing enter btw
            textBoxRelativeStatic.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    UpdateFrequency(s, ImageType.Static, FrequencyType.Relative);
                    e.SuppressKeyPress = true;
                }
            };
            textBoxRelativeGIF.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    UpdateFrequency(s, ImageType.GIF, FrequencyType.Relative);
                    e.SuppressKeyPress = true;
                }
            };
            textBoxRelativeVideo.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    UpdateFrequency(s, ImageType.Video, FrequencyType.Relative);
                    e.SuppressKeyPress = true;
                }
            };

            textBoxExactStatic.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    UpdateFrequency(s, ImageType.Static, FrequencyType.Exact);
                    e.SuppressKeyPress = true;
                }
            };
            textBoxExactGIF.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    UpdateFrequency(s, ImageType.GIF, FrequencyType.Exact);
                    e.SuppressKeyPress = true;
                }
            };
            textBoxExactVideo.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    UpdateFrequency(s, ImageType.Video, FrequencyType.Exact);
                    e.SuppressKeyPress = true;
                }
            };
        }

        private void ResetFrequencyText()
        {
            SetFrequencyText(ImageType.Static);
            SetFrequencyText(ImageType.GIF);
            SetFrequencyText(ImageType.Video);
        }

        private void SetFrequencyText(ImageType imageType)
        {
            switch (imageType)
            {
                case ImageType.Static:
                    textBoxRelativeStatic.Text = ThemeOptions.RelativeFrequency[ImageType.Static] * 100 + "%";
                    textBoxExactStatic.Text = ThemeOptions.ExactFrequency[ImageType.Static] * 100 + "%";
                    break;

                case ImageType.GIF:
                    textBoxRelativeGIF.Text = ThemeOptions.RelativeFrequency[ImageType.GIF] * 100 + "%";
                    textBoxExactGIF.Text = ThemeOptions.ExactFrequency[ImageType.GIF] * 100 + "%";
                    break;

                case ImageType.Video:
                    textBoxRelativeVideo.Text = ThemeOptions.RelativeFrequency[ImageType.Video] * 100 + "%";
                    textBoxExactVideo.Text = ThemeOptions.ExactFrequency[ImageType.Video] * 100 + "%";
                    break;
            }
        }

        private void UpdateFrequency(object sender, ImageType imageType, FrequencyType frequencyType)
        {
            FrequencyCalculator.UpdateFrequency(sender, imageType, frequencyType, ref ThemeOptions);
            ResetFrequencyText();
        }
        #endregion
    }
}
