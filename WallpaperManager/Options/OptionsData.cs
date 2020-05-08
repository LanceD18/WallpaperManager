using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }

    public struct ThemeOptions
    {
        public bool LargerImagesOnLargerMonitors;
        public bool HigherRankedImagesOnLargerMonitors;
        public bool EnableDetectionOfInactiveImages;
        public bool WeightedRanking;
    }
}
