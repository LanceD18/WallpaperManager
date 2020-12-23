﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WallpaperManager.Options
{
    public static class OptionsData
    {
        // Theme Options
        public static ThemeOptions ThemeOptions;

        // Global Options
        public static string DefaultTheme;
        public static bool EnableDefaultThemeHotkey;

        public static void Initialize()
        {
            DefaultTheme = Properties.Settings.Default.DefaultTheme;
            EnableDefaultThemeHotkey = Properties.Settings.Default.DefaultThemeHotkey;

            InitializePotentialNulls();
        }

        public static void InitializePotentialNulls()
        {
            // Initialize Relative Frequency if it hasn't been initialized from loading the theme
            if (ThemeOptions.RelativeFrequency == null)
            {
                ThemeOptions.RelativeFrequency = new Dictionary<ImageType, double>()
                {
                    {ImageType.Static, 1},
                    {ImageType.GIF, 1},
                    {ImageType.Video, 1}
                };
            }

            // Initialize Exact Frequency if it hasn't been initialized from loading the theme
            if (ThemeOptions.ExactFrequency == null)
            {
                ThemeOptions.ExactFrequency = new Dictionary<ImageType, double>()
                {
                    {ImageType.Static, 0.33},
                    {ImageType.GIF, 0.33},
                    {ImageType.Video, 0.33}
                };
            }
        }

        public static bool IsFrequencyEqual()
        {
            return OptionsData.ThemeOptions.RelativeFrequency[ImageType.Static] == 1 &&
                   OptionsData.ThemeOptions.RelativeFrequency[ImageType.GIF] == 1 &&
                   OptionsData.ThemeOptions.RelativeFrequency[ImageType.Video] == 1;
        }

        public static double GetExactFrequency(ImageType imageType) => ThemeOptions.ExactFrequency[imageType];
    }

    public struct ThemeOptions
    {
        public bool LargerImagesOnLargerMonitors;
        public bool HigherRankedImagesOnLargerMonitors;
        public bool EnableDetectionOfInactiveImages;
        public bool WeightedRanks;

        public Dictionary<ImageType, double> RelativeFrequency;
        public Dictionary<ImageType, double> ExactFrequency;
    }
}
