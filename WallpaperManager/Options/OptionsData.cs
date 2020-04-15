using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperManager.Options
{
    public static class OptionsData
    {
        public static bool LargerImagesOnLargerMonitors;
        public static bool HigherRankedImagesOnLargerMonitors;
        public static bool EnableDetectionOfInactiveImages;

        public static string DefaultTheme;
        public static bool EnableDefaultThemeHotkey;

        public static void Initialize()
        {
            DefaultTheme = Properties.Settings.Default.DefaultTheme;
            EnableDefaultThemeHotkey = Properties.Settings.Default.DefaultThemeHotkey;
        }
    }
}
