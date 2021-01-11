using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Options
{
    // Theme-Wide Settings
    public struct ThemeOptions
    {
        // General Options
        public bool LargerImagesOnLargerDisplays;
        public bool HigherRankedImagesOnLargerDisplays;
        public bool EnableDetectionOfInactiveImages;
        public bool WeightedRanks;
        public bool WeightedFrequency;
        public bool AllowTagBasedRenamingForMovedImages;

        public bool ExcludeRenamingStatic;
        public bool ExcludeRenamingGif;
        public bool ExcludeRenamingVideo;

        public Dictionary<ImageType, double> RelativeFrequency;
        public Dictionary<ImageType, double> ExactFrequency;

        public VideoOptions VideoOptions;
    }

    public struct VideoOptions
    {
        public bool MuteIfAudioPlaying;
        public bool MuteIfApplicationMaximized;
        public bool MuteIfApplicationFocused;
        public int MinimumVideoLoops;
        public float MaximumVideoTime;
    }

    // TODO Set me up later, this will be used to diverge displays into independent sets of options
    // Display-Bound Settings
    public struct DisplayOptions
    {
        // General Settings
        public bool WeightedRanks;

        // Video Settings
        public Dictionary<ImageType, double> RelativeFrequency;
        public Dictionary<ImageType, double> ExactFrequency;
    }

    public static class OptionsData
    {
        // Theme Options
        public static ThemeOptions ThemeOptions;

        // Monitor Options
        public static DisplayOptions[] DisplayOptions;

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

        public static double GetExactFrequency(ImageType imageType) => ThemeOptions.ExactFrequency[imageType];
    }
}
