using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LanceTools;
using WallpaperManager.ApplicationData;
using WallpaperManager.Options;

namespace WallpaperManager.Pathing
{
    public static class WallpaperPathing
    {
        public static string[] ActiveWallpapers = new string[Screen.AllScreens.Length]; // holds paths of the currently active wallpapers
        public static string[] NextWallpapers = new string[Screen.AllScreens.Length]; // derived from UpcomingWallpapers, holds the next set of wallpapers
        public static Stack<string>[] PreviousWallpapers = new Stack<string>[Screen.AllScreens.Length]; // allows you to return back to every wallpaper encountered during the current session
        public static Queue<string[]> UpcomingWallpapers = new Queue<string[]>(); // allows display-dependent wallpaper orders to be set without synced displays
        public static readonly string WallpaperDataDirectory = Path.GetDirectoryName(Application.ExecutablePath) + @"\WallpaperData";
        public static readonly string ActiveWallpaperImageFile = Path.GetDirectoryName(Application.ExecutablePath) + @"\WallpaperData\ActiveWallpaperImage.bmp";
        //?public static readonly string TempVideoThumbnailFile = Path.GetDirectoryName(Application.ExecutablePath) + @"\WallpaperData\TempVideoThumbnail.bmp";
        public static string ActiveWallpaperTheme; // the currently loaded wallpaper theme

        /// <summary>
        /// Ensures that all necessary folders exist & creates the stacks for previous wallpapers
        /// </summary>
        public static void Validate()
        {
            if (!Directory.Exists(WallpaperDataDirectory))
            {
                Directory.CreateDirectory(WallpaperDataDirectory);
            }

            InitializePreviousWallpapers();
        }

        public static void Reset() // resets all theme-related data to their default state
        {
            ActiveWallpapers = new string[Screen.AllScreens.Length];
            NextWallpapers = new string[Screen.AllScreens.Length];
            PreviousWallpapers = new Stack<string>[Screen.AllScreens.Length];
            UpcomingWallpapers = new Queue<string[]>(); 

            InitializePreviousWallpapers();
        }

        private static void InitializePreviousWallpapers()
        {
            for (int i = 0; i < PreviousWallpapers.Length; i++) PreviousWallpapers[i] = new Stack<string>();
        }

        public static bool IsWallpapersValid(string[] wallpapers)
        {
            foreach (string wallpaperPath in wallpapers)
            {
                if (!File.Exists(wallpaperPath))
                {
                    return false;
                }
            }

            return true;
        }

        public static string SetNextWallpaperOrder(int index, bool ignoreIdenticalWallpapers)
        {
            // this indicates that it's time to search for a new set of upcoming wallpapers
            if (ActiveWallpapers[index] == NextWallpapers[index] && !ignoreIdenticalWallpapers)
            {
                RandomizeWallpapers(); // enqueues next set of upcoming wallpapers
                NextWallpapers = UpcomingWallpapers.Dequeue();
            }

            return ActiveWallpapers[index] = NextWallpapers[index];
        }

        private static bool RandomizeWallpapers()
        {
            Random rand = new Random();

            // Gather potential wallpapers

            string[] potentialWallpapers = new string[DisplayData.Displays.Length];
            for (int i = 0; i < DisplayData.Displays.Length; i++)
            {
                ImageType imageTypeToSearchFor = ImageType.None;
                
                double staticChance = OptionsData.GetExactFrequency(ImageType.Static);
                double gifChance = OptionsData.GetExactFrequency(ImageType.GIF);
                double videoChance = OptionsData.GetExactFrequency(ImageType.Video);

                ImageType[] imageTypeIndexes = {ImageType.Static, ImageType.GIF, ImageType.Video};
                double[] imageTypePercentages = {staticChance, gifChance, videoChance};

                imageTypeToSearchFor = rand.NextInWeightedArray(imageTypeIndexes, imageTypePercentages);

                if (WallpaperData.IsAllImagesOfTypeUnranked(imageTypeToSearchFor))
                {
                    MessageBox.Show("Attempted to set a wallpaper to an image type with no valid/ranked images. Wallpaper Change Cancelled [IMAGE TYPE: " + imageTypeToSearchFor + "]" +
                                    "\n\nEither change relative frequency chance of the above image type to 0% (Under Frequency in the options menu)" +
                                    "or activate some wallpapers of the above image type (Unranked images with a rank of 0 are inactive");
                    return false;
                }

                int randomRank = GetRandomRank(ref rand, imageTypeToSearchFor);

                // Find random image path
                if (randomRank != -1)
                {
                    Debug.WriteLine("Setting Wallpaper: " + i);
                    potentialWallpapers[i] = WallpaperData.GetRandomImageOfRank(randomRank, ref rand, imageTypeToSearchFor);

                    if (!WallpaperData.GetImageData(potentialWallpapers[i]).Active)
                    {
                        //! This shouldn't happen, if this does you have a bug to fix
                        MessageBox.Show("ERROR: Attempted to set display " + i + " to an inactive wallpaper | A new wallpaper has been chosen");
                        i--; // find another wallpaper, the selected wallpaper is inactive
                    }
                }
                else
                {
                    Debug.WriteLine("-1 rank selected | Fix Code | This will occur if all ranks are 0");
                }
            }

            ModifyWallpaperOrder(ref potentialWallpapers);
            UpcomingWallpapers.Enqueue(potentialWallpapers);

            return true;
        }

        // Picks ranks based on their default percentiles (Where the highest rank is the most likely to appear and it goes down from there)
        private static int GetRandomRank(ref Random rand, ImageType imageType)
        {
            Debug.WriteLine("Searching for: " + imageType);
            // the percentiles for weighted ranks change everytime an image's rank is altered or if the image type is not none
            if ((WallpaperData.potentialWeightedRankUpdate && OptionsData.ThemeOptions.WeightedRanks) || WallpaperData.potentialRegularRankUpdate || imageType != ImageType.None)
            {
                WallpaperData.UpdateRankPercentiles(imageType); //? this method sets the above booleans to false
            }

            Dictionary<int, double> modifiedRankPercentiles = WallpaperData.GetRankPercentiles(imageType);

            return rand.NextInWeightedArray(modifiedRankPercentiles.Keys.ToArray(), modifiedRankPercentiles.Values.ToArray());
        }

        #region Wallpaper Order Modifiers
        private static void ModifyWallpaperOrder(ref string[] wallpapersToModify)
        {
            /* TODO
           // request next 3 wallpapers, determine their preferred setting
           // set first display with said preferred setting
           // request next 3 wallpapers
           // set second display with preferred setting
           // no request
           // set first wallpaper to next wallpaper (Using second set of requested wallpapers)
           // request next 3 wallpapers, this changes the third wallpaper setting
           // set third display, this will use the *second* preferred setting (Using third set of requested wallpapers)
           // essentially, there will always be an upcoming set of preferred wallpapers, once that set surpassed, a new set will be made that all displays will have to follow
           */

            if (IsWallpapersValid(wallpapersToModify))
            {
                string[] reorderedWallpapers = new string[0];
                if (OptionsData.ThemeOptions.HigherRankedImagesOnLargerDisplays || OptionsData.ThemeOptions.LargerImagesOnLargerDisplays)
                {
                    int[] largestMonitorIndexOrder = DisplayData.GetLargestDisplayIndexOrder();

                    if (OptionsData.ThemeOptions.HigherRankedImagesOnLargerDisplays)
                    {
                        reorderedWallpapers = (from f in wallpapersToModify orderby WallpaperData.GetImageRank(f) descending select f).ToArray();

                        // both ranking and size are now a factor so first an image's rank will determine their index and then afterwards
                        // any ranking conflicts have their indexes determined by size rather than being random
                        if (OptionsData.ThemeOptions.LargerImagesOnLargerDisplays)
                        {
                            ConflictResolveIdenticalRanks(ref reorderedWallpapers);
                        }
                    }
                    else if (OptionsData.ThemeOptions.LargerImagesOnLargerDisplays)
                    {
                        reorderedWallpapers = LargestImagesWithCustomFilePath(wallpapersToModify);
                    }

                    //? Applies the final modification
                    wallpapersToModify = ApplyModifiedPathOrder(reorderedWallpapers, largestMonitorIndexOrder);
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
        
        private static string[] LargestImagesWithCustomFilePath(string[] customFilePath)
        {
            Image[] images = (from f in customFilePath select WallpaperManagerTools.GetImageFromFile(f)).ToArray();

            for (int i = 0; i < customFilePath.Length; i++) // sets file path for image objects
            {
                //? Note that the tag is empty beforehand | This is used to organize the images below based on their width and height
                images[i].Tag = customFilePath[i];
            }

            customFilePath = (from f in images orderby f.Width + f.Height descending select f.Tag.ToString()).ToArray();

            foreach (Image image in images) image.Dispose();

            return customFilePath;
        }

        private static string[] ApplyModifiedPathOrder(string[] reorderedWallpapers, int[] reorderedIndexes)
        {
            string[] newOrder = new string[reorderedWallpapers.Length];
            for (int i = 0; i < newOrder.Length; i++)
            {
                newOrder[reorderedIndexes[i]] = reorderedWallpapers[i];
            }
            Debug.WriteLine("Modified Path Order Set");
            return newOrder;
        }
        #endregion
    }
}
