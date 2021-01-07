using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools;

namespace WallpaperManager.Options
{
    public static class FrequencyCalculator
    {
        public static void UpdateFrequency(object sender, ImageType imageType, FrequencyType frequencyType, ref ThemeOptions ThemeOptions)
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
            catch (Exception exception)
            {
                // incorrect value entered, end update and reset text (reset externally)
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
                        return;
                    }
                }

                ThemeOptions.RelativeFrequency[imageType] = input / 100; // the actual value is a percentage

                RecalculateExactFrequency(ref ThemeOptions);
            }
            else if (frequencyType == FrequencyType.Exact) // set a new exact chance, recalculating the remaining exact chances & also the relative chances to represent this change
            {
                Debug.WriteLine("Exact");
                ThemeOptions.ExactFrequency[imageType] = input / 100; // the actual value is a percentage

                if (input < 100 && input > 0)
                {
                    CalculateExactFrequency(imageType, ref ThemeOptions);
                    RecalculateRelativeFrequency(imageType, false, ref ThemeOptions);
                }
                else if (input >= 100) // exact chance of 1, set everything else to 0
                {
                    if (imageType != ImageType.Static) ThemeOptions.ExactFrequency[ImageType.Static] = 0;
                    if (imageType != ImageType.GIF) ThemeOptions.ExactFrequency[ImageType.GIF] = 0;
                    if (imageType != ImageType.Video) ThemeOptions.ExactFrequency[ImageType.Video] = 0;
                    RecalculateRelativeFrequency(imageType, true, ref ThemeOptions);
                }
                else if (input <= 0) // exact chance of 0, set everything else to 0.5
                {
                    if (imageType != ImageType.Static) ThemeOptions.ExactFrequency[ImageType.Static] = 0.5;
                    if (imageType != ImageType.GIF) ThemeOptions.ExactFrequency[ImageType.GIF] = 0.5;
                    if (imageType != ImageType.Video) ThemeOptions.ExactFrequency[ImageType.Video] = 0.5;
                    RecalculateRelativeFrequency(imageType, true, ref ThemeOptions);
                }

            }
        }

        // Recalculate Relative Frequency to account for changes to Exact Frequency
        // (The recalculation for this can vary wildly depending on how its programmed, in this case, the changed exact value will be
        // displays as 100% while the remaining values will display how likely they are to appear relative to that 100% value)
        private static void RecalculateRelativeFrequency(ImageType changedImageType, bool absolutePercentage, ref ThemeOptions ThemeOptions)
        {
            ThemeOptions.RelativeFrequency[changedImageType] = 1;

            if (!absolutePercentage) // exact values have chances anywhere between 0 & 100 exclusive
            {
                if (changedImageType != ImageType.Static)
                    ThemeOptions.RelativeFrequency[ImageType.Static] = 
                        ThemeOptions.ExactFrequency[ImageType.Static] / ThemeOptions.ExactFrequency[changedImageType];

                if (changedImageType != ImageType.GIF)
                    ThemeOptions.RelativeFrequency[ImageType.GIF] = 
                        ThemeOptions.ExactFrequency[ImageType.GIF] / ThemeOptions.ExactFrequency[changedImageType];

                if (changedImageType != ImageType.Video)
                    ThemeOptions.RelativeFrequency[ImageType.Video] = 
                        ThemeOptions.ExactFrequency[ImageType.Video] / ThemeOptions.ExactFrequency[changedImageType];
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
        private static void RecalculateExactFrequency(ref ThemeOptions ThemeOptions)
        {
            double chanceTotal = ThemeOptions.RelativeFrequency[ImageType.Static] + 
                                 ThemeOptions.RelativeFrequency[ImageType.GIF] + 
                                 ThemeOptions.RelativeFrequency[ImageType.Video];

            Debug.WriteLine("chanceTotal: " + chanceTotal);

            Debug.WriteLine(ThemeOptions.RelativeFrequency[ImageType.Static] / chanceTotal);
            Debug.WriteLine(ThemeOptions.RelativeFrequency[ImageType.GIF] / chanceTotal);
            Debug.WriteLine(ThemeOptions.RelativeFrequency[ImageType.Video] / chanceTotal);

            ThemeOptions.ExactFrequency[ImageType.Static] = ThemeOptions.RelativeFrequency[ImageType.Static] / chanceTotal;
            ThemeOptions.ExactFrequency[ImageType.GIF] = ThemeOptions.RelativeFrequency[ImageType.GIF] / chanceTotal;
            ThemeOptions.ExactFrequency[ImageType.Video] = ThemeOptions.RelativeFrequency[ImageType.Video] / chanceTotal;
        }

        private static void CalculateExactFrequency(ImageType changedImageType, ref ThemeOptions ThemeOptions)
        {
            // Readjust Exact Frequency to account for the new changes
            double chanceTotal = ThemeOptions.ExactFrequency[ImageType.Static] + 
                                 ThemeOptions.ExactFrequency[ImageType.GIF] + 
                                 ThemeOptions.ExactFrequency[ImageType.Video];
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

    }
}
