using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using LanceTools;
using WallpaperManager.ImageSelector;
using WallpaperManager.Options;

namespace WallpaperManager.ApplicationData
{
    public static partial class PathData
    {
        public static string[] ActiveWallpapers = new string[Screen.AllScreens.Length]; // holds paths of the currently active wallpapers
        public static Stack<string[]> PreviousWallpapers = new Stack<string[]>(); // allows you to return back to every wallpaper encountered during the current session
        public static readonly string WallpaperDataDirectory = Path.GetDirectoryName(Application.ExecutablePath) + @"\WallpaperData";
        public static readonly string ActiveWallpaperImage = Path.GetDirectoryName(Application.ExecutablePath) + @"\WallpaperData\ActiveWallpaperImage.bmp";
        public static readonly string DefaultWallpaperData = Path.GetDirectoryName(Application.ExecutablePath) + @"\WallpaperData\DefaultWallpaperData.json";
        public static readonly string TempImageLocation = Path.GetDirectoryName(Application.ExecutablePath) + @"\WallpaperData\TempImageLocation.file";
        public static string ActiveWallpaperTheme; // the currently loaded wallpaper theme

        /// <summary>
        /// Ensures that all necessary folders exist
        /// </summary>
        public static void Validate()
        {
            if (!Directory.Exists(WallpaperDataDirectory))
            {
                Directory.CreateDirectory(WallpaperDataDirectory);
            }
        }

        public static bool IsWallpapersValid()
        {
            foreach (string wallpaperPath in ActiveWallpapers)
            {
                if (!File.Exists(wallpaperPath))
                {
                    return false;
                }
            }

            return true;
        }

        public static void RandomizeWallpapers()
        {
            Dictionary<int, double> modifiedRankPercentiles = WallpaperData.GetModifiedRankPercentiles();
            int[] rankPercentageKeys = modifiedRankPercentiles.Keys.ToArray();
            double[] rankPercentageValues = modifiedRankPercentiles.Values.ToArray();

            Random rand = new Random();

            for (int i = 0; i < MonitorData.Screens.Length; i++)
            {
                double percentage = rand.NextDouble(); // determines which rank is chosen
                double percentageCounter = 1.0; // used to "traverse" through rank percentages
                int randomRank = -1;

                // Find random rank based on percentiles
                for (int j = rankPercentageValues.Length - 1; j >= 0; j--)
                {
                    if (percentage.InRange(percentageCounter - rankPercentageValues[j], percentageCounter)) // First Value
                    {
                        randomRank = rankPercentageKeys[j];
                        break;
                    }

                    percentageCounter -= rankPercentageValues[j];
                }

                // Find random image path
                if (randomRank == -1)
                {
                    Debug.WriteLine("-1 rank selected | Fix Code | This will occur if all ranks are 0");
                }
                else
                {
                    ActiveWallpapers[i] = WallpaperData.GetRandomImageOfRank(randomRank, ref rand);

                    if (!WallpaperData.GetImageData(ActiveWallpapers[i]).Active && !OptionsData.EnableDetectionOfInactiveImages)
                    {
                        //TODO TEST THIS CODE SEGMENT!!!
                        MessageBox.Show("Attempted to set monitor " + i + " to an inactive wallpaper | A new wallpaper has been chosen" +
                                        "\nThis error message can be deleted after you confirm that this works");
                        i--; // find another wallpaper, the selected wallpaper is inactive
                    }
                }
            }

            ModifyWallpaperOrder();
        }

        #region Wallpaper Order Modifiers
        private static void ModifyWallpaperOrder()
        {
            /*
            if (IsWallpapersValid())
            {
                string[] test = LargestImagesWithCustomFilePath(ActiveWallpapers);

                int[] largestMonitorIndexOrder = MonitorData.GetLargestMonitorIndexOrder();
                for (int i = 0; i < ActiveWallpapers.Length; i++)
                {
                    Debug.WriteLine(largestMonitorIndexOrder[i]);
                    ActiveWallpapers[largestMonitorIndexOrder[i]] = test[i];
                }
            }

            if (IsWallpapersValid())
            {
                if (OptionsData.HigherRankedImagesOnLargerMonitors)
                {
                    if (OptionsData.LargerImagesOnLargerMonitors)
                    {

                    }
                }
                else if (OptionsData.LargerImagesOnLargerMonitors)
                {

                }
            }
            */

            if (false && IsWallpapersValid())
            {
                if (OptionsData.HigherRankedImagesOnLargerMonitors && OptionsData.LargerImagesOnLargerMonitors)
                {
                    int[] testRanks = new int[ActiveWallpapers.Length];
                    bool checkForSize = false;
                    int identicalRankCount = 0;

                    for (int i = 0; i < ActiveWallpapers.Length; i++)
                    {
                        int curRank = WallpaperData.GetImageRank(ActiveWallpapers[i]);

                        if (testRanks.Contains(curRank))
                        {
                            checkForSize = true; // at least 2 images will have their size compared
                            identicalRankCount++;
                        }

                        testRanks[identicalRankCount] = curRank;
                    }

                    if (identicalRankCount != ActiveWallpapers.Length - 1)
                    {
                        HigherRankedImagesOnLargerMonitors(checkForSize);
                    }
                    else // All ranks for the current wallpapers are identical
                    {
                        LargerImagesOnLargerMonitors();
                    }
                }

                if (OptionsData.HigherRankedImagesOnLargerMonitors && !OptionsData.LargerImagesOnLargerMonitors)
                {
                    HigherRankedImagesOnLargerMonitors(false);
                }

                if (OptionsData.LargerImagesOnLargerMonitors && !OptionsData.HigherRankedImagesOnLargerMonitors)
                {
                    LargerImagesOnLargerMonitors();
                }
            }
        }

        private static void HigherRankedImagesOnLargerMonitors(bool alsoCheckingForSize)
        {
            string[] tempFilePath = (from f in ActiveWallpapers orderby WallpaperData.GetImageRank(f) descending select f).ToArray();

            ReOrderFilePathWithLargestMonitor(tempFilePath, alsoCheckingForSize);
        }

        private static void LargerImagesOnLargerMonitors()
        {
            string[] tempFilePath = LargestImagesWithCustomFilePath(ActiveWallpapers);

            ApplyNewPathOrder(tempFilePath);
        }

        private static string[] LargestImagesWithCustomFilePath(string[] CustomFilePath)
        {
            Image[] images = (from f in CustomFilePath select Image.FromFile(f)).ToArray();

            for (int i = 0; i < CustomFilePath.Length; i++) // sets file path for image objects
            {
                images[i].Tag = CustomFilePath[i];
            }

            CustomFilePath = (from f in images orderby f.Width + f.Height descending select f.Tag.ToString()).ToArray();

            foreach (Image image in images)
            {
                image.Dispose();
            }

            return CustomFilePath;
        }

        private static void ReOrderFilePathWithLargestMonitor(string[] tempFilePath, bool checkingForIdenticalRank)
        {
            if (checkingForIdenticalRank)
            {
                // so by now we have the images ordered by their ranks
                // but there are at least 2 images with the same rank
                // single these images out and reorder them based on size
                // they should be right next to each other in the array, so just ensure that their positions are cycled appropriately

                Dictionary<int, List<string>> rankInfo = new Dictionary<int, List<string>>();

                foreach (string path in tempFilePath)
                {
                    int curRank = WallpaperData.GetImageRank(path);

                    if (!rankInfo.Keys.Contains(curRank))
                    {
                        rankInfo.Add(curRank, new List<string>() {path});
                    }
                    else
                    {
                        rankInfo[curRank].Add(path);
                    }
                }

                // take into account that the ranks are already ordered, you just need to order the identical sections
                List<string> reorderedFilePath = new List<string>();
                foreach (int rank in rankInfo.Keys)
                {
                    if (rankInfo[rank].Count > 1) // duplicates exist at this rank
                    {
                        string[] identicalRankOrder = LargestImagesWithCustomFilePath(rankInfo[rank].ToArray());

                        foreach (string path in identicalRankOrder)
                        {
                            reorderedFilePath.Add(path);
                        }
                    }
                    else
                    {
                        reorderedFilePath.Add(rankInfo[rank][0]);
                    }
                }

                ApplyNewPathOrder(reorderedFilePath.ToArray());
            }
            else
            {
                ApplyNewPathOrder(tempFilePath);
            }
        }

        private static void ApplyNewPathOrder(string[] reorderedFilePath)
        {
            int[] largestMonitorIndexOrder = MonitorData.GetLargestMonitorIndexOrder();

            for (int i = 0; i < ActiveWallpapers.Length; i++)
            {
                ActiveWallpapers[largestMonitorIndexOrder[i]] = reorderedFilePath[i];
            }
        }
        #endregion
    }
}
