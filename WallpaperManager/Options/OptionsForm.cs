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

            ThemeOptions = OptionsData.ThemeOptions;

            SuspendLayout();

            // Initialize Frequency Properties
            ResetFrequencyText();
            InitializeFrequencyEvents();

            // Theme Settings
            checkBoxLargerImagesOnLargerMonitors.Checked = ThemeOptions.LargerImagesOnLargerMonitors;
            checkBoxHigherRankedImagesOnLargerMonitors.Checked = ThemeOptions.HigherRankedImagesOnLargerMonitors;
            checkBoxEnableDetectionOfInactiveImages.Checked = ThemeOptions.EnableDetectionOfInactiveImages;
            checkBoxWeightedRanks.Checked = ThemeOptions.WeightedRanks;

            // Global Settings
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
            ThemeOptions.LargerImagesOnLargerMonitors = checkBoxLargerImagesOnLargerMonitors.Checked;
            ThemeOptions.HigherRankedImagesOnLargerMonitors = checkBoxHigherRankedImagesOnLargerMonitors.Checked;
            ThemeOptions.EnableDetectionOfInactiveImages = checkBoxEnableDetectionOfInactiveImages.Checked;

            bool updateRankPercentiles = ThemeOptions.WeightedRanks != checkBoxWeightedRanks.Checked;
            ThemeOptions.WeightedRanks = checkBoxWeightedRanks.Checked;

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
            TextBox sourceTextBox = sender as TextBox;

            double input = 0;

            // Process the Input
            try
            {
                string inputText = sourceTextBox.Text;
                if (inputText.Contains('%')) inputText = inputText.Substring(0, inputText.IndexOf('%')); // removes % from input if it was left

                if (frequencyType == FrequencyType.Relative)
                {
                    input = Math.Max(0, double.Parse(inputText));
                }
                else if (frequencyType == FrequencyType.Exact)
                {
                    input = MathE.Clamp(double.Parse(inputText), 0, 100);
                }
            }
            catch(Exception exception)
            {
                // incorrect value entered, end update and reset text
                ResetFrequencyText();
                return;
            }

            // Update a Frequency
            if (frequencyType == FrequencyType.Relative) // set the relative chance & recalculate exact chances to represent said change
            {
                Debug.WriteLine("Relative");

                if (input == 0)
                {
                    int zeroCount = 0;
                    if (ThemeOptions.RelativeFrequency[ImageType.Static] == 0) zeroCount++;
                    if (ThemeOptions.RelativeFrequency[ImageType.GIF] == 0) zeroCount++;
                    if (ThemeOptions.RelativeFrequency[ImageType.Video] == 0) zeroCount++;

                    if (zeroCount >= 2) // attempted to make all frequencies 0%, cancel this change
                    {
                        Debug.WriteLine("Cannot have 0% probability across all entries. Change cancelled");
                        ResetFrequencyText();
                        return;
                    }
                }

                ThemeOptions.RelativeFrequency[imageType] = input / 100; // the actual value is a percentage

                RecalculateExactFrequency();
            }
            else if (frequencyType == FrequencyType.Exact) // set a new exact chance, recalculating the remaining exact chances & also the relative chances to represent this change
            {
                Debug.WriteLine("Exact");
                ThemeOptions.ExactFrequency[imageType] = input / 100; // the actual value is a percentage
                
                if (input < 100 && input > 0)
                {
                    CalculateExactFrequency(imageType);
                    RecalculateRelativeFrequency(imageType, false);
                }
                else if (input >= 100) // exact chance of 1, set everything else to 0
                {
                    if (imageType != ImageType.Static) ThemeOptions.ExactFrequency[ImageType.Static] = 0;
                    if (imageType != ImageType.GIF) ThemeOptions.ExactFrequency[ImageType.GIF] = 0;
                    if (imageType != ImageType.Video) ThemeOptions.ExactFrequency[ImageType.Video] = 0;
                    RecalculateRelativeFrequency(imageType, true);
                }
                else if (input <= 0) // exact chance of 0, set everything else to 0.5
                {
                    if (imageType != ImageType.Static) ThemeOptions.ExactFrequency[ImageType.Static] = 0.5;
                    if (imageType != ImageType.GIF) ThemeOptions.ExactFrequency[ImageType.GIF] = 0.5;
                    if (imageType != ImageType.Video) ThemeOptions.ExactFrequency[ImageType.Video] = 0.5;
                    RecalculateRelativeFrequency(imageType, true);
                }

            }

            ResetFrequencyText();
        }

        // Recalculate Relative Frequency to account for changes to Exact Frequency
        // (The recalculation for this can vary wildly depending on how its programmed, in this case, the changed exact value will be
        // displays as 100% while the remaining values will display how likely they are to appear relative to that 100% value)
        private void RecalculateRelativeFrequency(ImageType changedImageType, bool absolutePercentage)
        {
            ThemeOptions.RelativeFrequency[changedImageType] = 1;

            if (!absolutePercentage) // exact values have chances anywhere between 0 & 100 exclusive
            {
                if (changedImageType != ImageType.Static)
                    ThemeOptions.RelativeFrequency[ImageType.Static] = ThemeOptions.ExactFrequency[ImageType.Static] / ThemeOptions.ExactFrequency[changedImageType];
                if (changedImageType != ImageType.GIF)
                    ThemeOptions.RelativeFrequency[ImageType.GIF] = ThemeOptions.ExactFrequency[ImageType.GIF] / ThemeOptions.ExactFrequency[changedImageType];
                if (changedImageType != ImageType.Video)
                    ThemeOptions.RelativeFrequency[ImageType.Video] = ThemeOptions.ExactFrequency[ImageType.Video] / ThemeOptions.ExactFrequency[changedImageType];
            }
            else // some exact value has a chance of 0 or 100, this needs its own separate calculation
            {
                ThemeOptions.RelativeFrequency[ImageType.Static] = 1 * ThemeOptions.ExactFrequency[ImageType.Static];
                ThemeOptions.RelativeFrequency[ImageType.GIF] = 1 * ThemeOptions.ExactFrequency[ImageType.GIF];
                ThemeOptions.RelativeFrequency[ImageType.Video] = 1 * ThemeOptions.ExactFrequency[ImageType.Video];
            }
        }

        // Recalculate Exact Frequency to account for changes to Relative Frequency
        // (This also displays to the user what the exact chance even is)
        private void RecalculateExactFrequency()
        {
            double chanceTotal = ThemeOptions.RelativeFrequency[ImageType.Static] + ThemeOptions.RelativeFrequency[ImageType.GIF] + ThemeOptions.RelativeFrequency[ImageType.Video];

            Debug.WriteLine("chanceTotal: " + chanceTotal);

            Debug.WriteLine(ThemeOptions.RelativeFrequency[ImageType.Static] / chanceTotal);
            Debug.WriteLine(ThemeOptions.RelativeFrequency[ImageType.GIF] / chanceTotal);
            Debug.WriteLine(ThemeOptions.RelativeFrequency[ImageType.Video] / chanceTotal);

            ThemeOptions.ExactFrequency[ImageType.Static] = ThemeOptions.RelativeFrequency[ImageType.Static] / chanceTotal;
            ThemeOptions.ExactFrequency[ImageType.GIF] = ThemeOptions.RelativeFrequency[ImageType.GIF] / chanceTotal;
            ThemeOptions.ExactFrequency[ImageType.Video] = ThemeOptions.RelativeFrequency[ImageType.Video] / chanceTotal;
        }

        private void CalculateExactFrequency(ImageType changedImageType)
        {
            // Readjust Exact Frequency to account for the new changes
            double chanceTotal = ThemeOptions.ExactFrequency[ImageType.Static] + ThemeOptions.ExactFrequency[ImageType.GIF] + ThemeOptions.ExactFrequency[ImageType.Video];
            Debug.WriteLine("chanceTotal: " + chanceTotal);

            // Leave the changed frequency and readjust the remaining two according to the value difference and their own relative values
            double valueDiff = chanceTotal - 1;
            Debug.WriteLine("valueDiff: " + valueDiff);

            double relativeChanceTotal = 0;

            if (changedImageType != ImageType.Static) relativeChanceTotal += ThemeOptions.ExactFrequency[ImageType.Static];
            if (changedImageType != ImageType.GIF) relativeChanceTotal += ThemeOptions.ExactFrequency[ImageType.GIF];
            if (changedImageType != ImageType.Video) relativeChanceTotal += ThemeOptions.ExactFrequency[ImageType.Video];
            Debug.WriteLine("relativeChanceTotal: " + relativeChanceTotal);

            double adjustedRelativeChanceTotal = relativeChanceTotal - valueDiff;
            Debug.WriteLine("adjustedRelativeChanceTotal: " + adjustedRelativeChanceTotal);

            double staticChance = 1;
            double gifChance = 1;
            double videoChance = 1;

            // calculate a multiplier for the image types that are *not* in use
            switch (changedImageType)
            {
                case ImageType.Static:
                    gifChance = ThemeOptions.ExactFrequency[ImageType.GIF] / relativeChanceTotal;
                    videoChance = ThemeOptions.ExactFrequency[ImageType.Video] / relativeChanceTotal;
                    break;

                case ImageType.GIF:
                    staticChance = ThemeOptions.ExactFrequency[ImageType.Static] / relativeChanceTotal;
                    videoChance = ThemeOptions.ExactFrequency[ImageType.Video] / relativeChanceTotal;
                    break;

                case ImageType.Video:
                    staticChance = ThemeOptions.ExactFrequency[ImageType.Static] / relativeChanceTotal;
                    gifChance = ThemeOptions.ExactFrequency[ImageType.GIF] / relativeChanceTotal;
                    break;
            }

            // readjust percentages
            if (changedImageType != ImageType.Static) ThemeOptions.ExactFrequency[ImageType.Static] = staticChance * adjustedRelativeChanceTotal;
            if (changedImageType != ImageType.GIF) ThemeOptions.ExactFrequency[ImageType.GIF] = gifChance * adjustedRelativeChanceTotal;
            if (changedImageType != ImageType.Video) ThemeOptions.ExactFrequency[ImageType.Video] = videoChance * adjustedRelativeChanceTotal;
        }
        #endregion

        private void checkBoxMonitorSpecificSettings_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
