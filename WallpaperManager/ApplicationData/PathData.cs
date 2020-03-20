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
            if (IsWallpapersValid())
            {
                string[] reorderedWallpapers = new string[0];
                if (OptionsData.HigherRankedImagesOnLargerMonitors || OptionsData.LargerImagesOnLargerMonitors)
                {
                    int[] largestMonitorIndexOrder = MonitorData.GetLargestMonitorIndexOrder();

                    if (OptionsData.HigherRankedImagesOnLargerMonitors)
                    {
                        reorderedWallpapers = (from f in ActiveWallpapers orderby WallpaperData.GetImageRank(f) descending select f).ToArray();

                        // both ranking and size are now a factor so first an image's rank will determine their index and then afterwards
                        // any ranking conflicts have their indexes determined by size rather than being random
                        if (OptionsData.LargerImagesOnLargerMonitors)
                        {
                            ConflictResolveIdenticalRanks(ref reorderedWallpapers);
                        }
                    }
                    else if (OptionsData.LargerImagesOnLargerMonitors)
                    {
                        reorderedWallpapers = LargestImagesWithCustomFilePath(ActiveWallpapers);
                    }

                    ApplyNewPathOrder(reorderedWallpapers, largestMonitorIndexOrder);
                }
            }
        }

        private static void ConflictResolveIdenticalRanks(ref string[] reorderedWallpapers)
        {
            bool conflictFound = false;
            Dictionary<int, List<string>> rankConflicts = new Dictionary<int, List<string>>();
            foreach (string wallpaper in reorderedWallpapers)
            {
                int wallpaperRank = WallpaperData.GetImageRank(wallpaper);
                if (!rankConflicts.ContainsKey(wallpaperRank))
                {
                    rankConflicts.Add(wallpaperRank, new List<string> { wallpaper });
                }
                else // more than one wallpaper contains the same rank, they'll have to have their conflicts resolved below
                {
                    rankConflicts[wallpaperRank].Add(wallpaper);
                    conflictFound = true;
                }
            }

            if (conflictFound) // if this is false then nothing will happen and the original reorderedWallpapers value will persist
            {
                List<string> conflictResolvedOrder = new List<string>();
                foreach (int rank in rankConflicts.Keys)
                {
                    if (rankConflicts[rank].Count > 1) // conflict present, fix it by comparing image sizes and placing the largest image first
                    {
                        string[] conflictResolvedRank = LargestImagesWithCustomFilePath(rankConflicts[rank].ToArray());
                        foreach (string wallpaper in conflictResolvedRank)
                        {
                            conflictResolvedOrder.Add(wallpaper);
                        }
                    }
                    else
                    {
                        conflictResolvedOrder.Add(rankConflicts[rank][0]);
                    }
                }

                reorderedWallpapers = conflictResolvedOrder.ToArray();
            }
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

        private static void ApplyNewPathOrder(string[] reorderedWallpapers, int[] reorderedIndexes)
        {
            for (int i = 0; i < ActiveWallpapers.Length; i++)
            {
                ActiveWallpapers[reorderedIndexes[i]] = reorderedWallpapers[i];
            }
        }
        #endregion
    }
}
