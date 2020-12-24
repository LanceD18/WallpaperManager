using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Xml;
using LanceTools;
using Microsoft.VisualBasic.FileIO;
using WallpaperManager.Options;

namespace WallpaperManager.ApplicationData
{
    // TODO Refactor this file's major components x: | Also attempt to refactor the way ImageData is handled
    public static partial class WallpaperData
    {
        public static WallpaperManager WallpaperManagerForm;

        private static Dictionary<string, ImageData> FileData = new Dictionary<string, ImageData>();

        private static ReactiveList<ReactiveList<string>> RankData = new ReactiveList<ReactiveList<string>>(); //? Add and removal should be automated, you should only need to retrieve data from this
        private static Dictionary<int, double> modifiedRankPercentiles = new Dictionary<int, double>();
        private static double[] rankPercentiles;
        public static bool potentialWeightedRankUpdate;
        public static bool potentialRegularRankUpdate;

        private static List<string> ActiveImages = new List<string>(); //? Add and removal should be automated, you should only need to retrieve data from this
        private static Dictionary<string, bool> ImageFolders = new Dictionary<string, bool>();

        public static bool RandomizeSelection;

        private static string jpxStringPrompt = "TODO: Test jpeg files. I'm pretty sure they'll work but just to be sure.\n" +
                           "Here are your current jpeg files:\n";
        /*
        private static string jpxStringPrompt = "The following files have extensions that are unusable. However, they can be readily converted into jpg or png files.\n" +
                           "Would you like to convert the following images? \n " +
                           "[NOTE: This will move the original to the recycling bin and add in a new file, names will be adjusted if needed]\n\n";
        */

        /// <summary>
        /// Sets up and loads all necessary data
        /// </summary>
        /// <param name="wallpaperManager"></param>
        /// <param name="newRankMax"></param>
        /// <param name="useLastLoadedTheme"></param>
        public static void Initialize(WallpaperManager wallpaperManager, bool useLastLoadedTheme)
        {
            WallpaperManagerForm = wallpaperManager;

            InitializeImagesOfType();

            RankData.OnListAddItem += RankData_OnParentListAddItem;
            RankData.OnListRemoveItem += RankData_OnParentListRemoveItem;

            if (useLastLoadedTheme) LoadDefaultTheme(); //! This should be placed at the very end
        }

        public static void InitializeImagesOfType() // this needs to be reloaded whenever a theme is loaded
        {
            ImagesOfType = new Dictionary<ImageType, Dictionary<string, ImageData>>();
            ImagesOfType.Add(ImageType.Static, new Dictionary<string, ImageData>());
            ImagesOfType.Add(ImageType.GIF, new Dictionary<string, ImageData>());
            ImagesOfType.Add(ImageType.Video, new Dictionary<string, ImageData>());

            ImagesOfTypeRankData = new Dictionary<ImageType, List<List<string>>>();
            ImagesOfTypeRankData.Add(ImageType.Static, new List<List<string>>());
            ImagesOfTypeRankData.Add(ImageType.GIF, new List<List<string>>());
            ImagesOfTypeRankData.Add(ImageType.Video, new List<List<string>>());
        }

        // File Data
        #region File Data
        public static ImageData AddImage(string path, int rank = 0, bool active = false, Dictionary<string, HashSet<string>> tags = null)
        {
            tags = tags ?? new Dictionary<string, HashSet<string>>(); // if null, the right-hand side of ?? will be called
            return AddImage(new ImageData(path, rank, active, tags));
        }

        public static ImageData AddImage(ImageData newImageData)
        {
            //TODO Implement a feature that checks if the image's file type is valid and either disables the image or offers to change it depending on this
            if (File.Exists(newImageData.Path) && (ImageFolders.ContainsKey(newImageData.PathFolder) || IsLoadingData))
            {
                Tagging.LinkImageTags(newImageData);
                FileData.Add(newImageData.Path, newImageData);
                ImagesOfType[newImageData.imageType].Add(newImageData.Path, newImageData);
                return newImageData;
            }
            else
            {
                Debug.WriteLine("Attempted to create an invalid image: " + newImageData.Path);
                return null;
            }
        }

        public static void RemoveImage(string path) => RemoveImages(new[] { path });

        public static void RemoveImages(string folderPath) => RemoveImages(GetImagesOfFolder(folderPath));

        public static void RemoveImages(string[] paths)
        {
            foreach (string path in paths)
            {
                Tagging.UnlinkImageTags(FileData[path]);

                ImageData image = FileData[path];
                ImageType imageType = image.imageType;

                ImagesOfType[imageType].Remove(path);
                ImagesOfTypeRankData[imageType][image.Rank].Remove(path);

                RankData[image.Rank].Remove(path);
                FileData.Remove(path);

                ActiveImages.Remove(path);
            }
        }

        public static ImageData GetImageData(string path) => FileData[path];

        public static ImageData[] GetAllImageData() => FileData.Values.ToArray();

        public static string[] GetAllImages() => FileData.Keys.ToArray();

        public static int GetImageRank(string path) => ContainsImage(path) ? FileData[path].Rank : -1;

        public static bool ContainsImage(string path) => FileData.ContainsKey(path);

        public static bool FileDataIsEmpty() => FileData.Count == 0;

        #endregion File Data

        // Rank Data
        #region Rank Data
        public static string[] GetImagesOfRank(int rank) => RankData[rank].ToArray();

        public static string[] GetAllRankedImages()
        {
            string[] imagePaths = new string[0];
            int arraySize = 0;

            for (int i = 0; i < RankData.Count; i++)
            {
                if (i != 0 && RankData[i].Count != 0) // ensures that rank 0 images are not included since they are 'unranked'
                {
                    string[] currentRankArray = RankData[i].ToArray();
                    int tempArraySize = arraySize + currentRankArray.Length;

                    string[] tempImagePaths = new string[tempArraySize];
                    imagePaths.CopyTo(tempImagePaths, 0);

                    imagePaths = new string[tempArraySize];
                    tempImagePaths.CopyTo(imagePaths, 0);

                    Array.Copy(currentRankArray, 0, imagePaths, arraySize, currentRankArray.Length);
                    arraySize += currentRankArray.Length; // remember that destinationIndex needs to start at the previous array size, a temporary array size is used above
                }
            }

            string[] finalImagePaths = new string[arraySize];
            imagePaths.CopyTo(finalImagePaths, 0);

            return finalImagePaths;
        }

        public static ImageData[] GetAllRankedImageData()
        {
            string[] images = GetAllRankedImages(); //? note that this ensures that rank 0 images are not included since they are 'unranked'
            ImageData[] rankedImageData = new ImageData[images.Length];

            for (int i = 0; i < rankedImageData.Length; i++)
            {
                rankedImageData[i] = FileData[images[i]];
            }

            return rankedImageData;
        }

        public static string GetRandomImageOfRank(int rank, ref Random rand, ImageType imageType)
        {
            if (imageType == ImageType.None) // regular procedure, select any image
            {
                int randomImage = rand.Next(0, RankData[rank].Count);
                return RankData[rank][randomImage];
            }
            else // limits selection to an image type
            {
                int randomImage = rand.Next(0, ImagesOfTypeRankData[imageType][rank].Count);
                return ImagesOfTypeRankData[imageType][rank][randomImage];
            }
        }

        public static int GetMaxRank() => RankData.Count - 1;

        public static void SetMaxRank(int newRankMax) //? This is the primary initializer for Rank Data
        {
            if (newRankMax > 0) // note that rank 0 is reserved for unranked images
            {
                SetRankData(newRankMax);
                SetRankPercentiles(newRankMax);
            }
            else
            {
                MessageBox.Show("The max rank cannot be equal to or less than 0");
            }
        }

        //TODO Pretty sure this can just be split into 2 methods
        private static void SetRankData(int newRankMax)
        {
            // Set RankData
            if (RankData.Count == 0) // Initialize RankData
            {
                RankData.Add(new ReactiveList<string>());

                for (int i = 0; i < newRankMax; i++)
                {
                    RankData.Add(new ReactiveList<string>());
                }
            }
            else // Update RankData
            {
                if (IsLoadingData || MessageBox.Show("Are you sure you want to change the max rank?\n(All images will have their ranks adjusted according to this change)",
                    "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    UpdateMaxRank(newRankMax);
                }
            }
        }

        private static void UpdateMaxRank(int newRankMax)
        {
            int oldRankMax = GetMaxRank();
            float rankChangeRatio = (float)newRankMax / oldRankMax;

            //! This needs to be placed right here otherwise ImageData will crash on trying to add the image to an unknown rank
            // Increase RankData's possible ranks if needed
            if (rankChangeRatio > 1) // newest rank max is higher than the current rank max
            {
                for (int i = oldRankMax; i < newRankMax; i++)
                {
                    RankData.Add(new ReactiveList<string>());
                }
            }

            if (!IsLoadingData) // no need to update ranks if you aren't actually changing anything
            {
                // Re-rank existing images
                string[] images = FileData.Keys.ToArray();
                foreach (string image in images)
                {
                    if (FileData[image].Rank != 0) // due to the Math.Max() used below this if statement is needed otherwise all rank 0 images would be set to 1
                    {
                        int newRank = Math.Max((int)Math.Round((double)FileData[image].Rank * rankChangeRatio), 1); // the Math.Max is used to ensure that no images are set to 0 (unranked)
                        FileData[image].Rank = newRank;
                    }
                }

                WallpaperManagerForm.UpdateImageRanks();
            }

            //! This needs to be placed right here otherwise ImageData will crash on trying to remove the image from an unknown rank
            // Decrease RankData's possible ranks if needed
            if (rankChangeRatio < 1)
            {
                for (int i = oldRankMax; i > newRankMax; i--)
                {
                    RankData.RemoveAt(RankData.Count - 1);
                }
            }

            //Debug.WriteLine(GetMaxRank());
        }

        public static bool ContainsRank(int rank) => rank >= 0 && rank < RankData.Count;

        #region Percentiles
        private static void SetRankPercentiles(int newRankMax)
        {
            // Set Rank Percentiles
            rankPercentiles = new double[newRankMax];
            double rankMultiplier = 10.0 / newRankMax;

            for (int i = 0; i < newRankMax; i++)
            {
                //TODO Add a system for modifying percentile ranges directly or changing up the formula
                // This is the default formula for rank percentiles, where each 10% of ranks has twice the probability of the previous 10%
                // Due to the rank multiplier, the max rank will always have a probability of 1024
                // ex: if the max rank is 100, rank 100 will have a probability of 1024 while rank 90 will have a probability of 512. These same numbers apply to 45 and 50 if the max is 50
                //? Note that the below formula does not include rank 0 | 0 * rankMultiplier is rank 1
                rankPercentiles[i] = Math.Pow(2, i * rankMultiplier);
            }
        }


        /// <summary>
        /// Modifies rank percentiles to represent the actual percentage chance of the rank appearing
        /// (The percentages of each rank will be modified to exclude images with a rank of 0)
        /// </summary>
        /// <returns></returns>
        //? You should call UpdateRankPercentiles instead if that's what's you need
        private static Dictionary<int, double> GetModifiedRankPercentiles(ImageType imageType)
        {
            double rankPercentagesTotal = 0;
            List<int> validRanks = new List<int>();
            for (int i = 0; i < RankData.Count; i++) // i == rank
            {
                if (RankData[i].Count != 0 && i != 0) // The use of i != 0 excludes unranked images
                {
                    if (imageType != ImageType.None) // if an image type is being searched for, check if contains any values
                    {
                        if (ImagesOfTypeRankData[imageType][i].Count == 0)
                        {
                            continue; // this rank is not valid since the selected image type is not present
                        }
                    }

                    rankPercentagesTotal += rankPercentiles[i - 1];
                    validRanks.Add(i);
                }
            }

            Dictionary<int, double> modifiedRankPercentiles = new Dictionary<int, double>();

            // scales the percentages to account for ranks that weren't included
            foreach (int rank in validRanks)
            {
                modifiedRankPercentiles.Add(rank, rankPercentiles[rank - 1] / rankPercentagesTotal);
                //xDebug.WriteLine("Rank: " + rank + " | Percentile: " + modifiedRankPercentiles[rank]);
            }

            return modifiedRankPercentiles;
        }

        /// <summary>
        /// Weights rank percentiles on both how high the rank is and how many images are in a rank
        /// </summary>
        //? You should call UpdateRankPercentiles instead if that's what's you need
        // TODO Modify this in a way that doesn't need GetModifiedRankPercentiles(), removing the need to loop twice. I doubt this will have much of a performance
        // TODO impact however considering how fast it already is. It may be best to just leave it as is for convenience
        private static Dictionary<int, double> GetWeightedRankPercentiles(ImageType imageType)
        {
            Dictionary<int, double> modifiedRankPercentiles = GetModifiedRankPercentiles(imageType);
            int[] validRanks = modifiedRankPercentiles.Keys.ToArray();

            int rankedImageCount = GetAllRankedImages().Length;
            double newRankPercentageTotal = 0;

            // sets the individual weighted percentage of each rank
            foreach (int rank in validRanks)
            {
                // If an image type is being searched for then only include the number of images from said image type
                double percentileModifier = imageType == ImageType.None ? 
                    ((double) RankData[rank].Count / rankedImageCount) : 
                    ((double) ImagesOfTypeRankData[imageType][rank].Count / rankedImageCount);

                modifiedRankPercentiles[rank] *= percentileModifier;
                newRankPercentageTotal += modifiedRankPercentiles[rank];
            }

            // rescales the percentages to account for weighting
            foreach (int rank in validRanks)
            {
                modifiedRankPercentiles[rank] /= newRankPercentageTotal;
                //xDebug.WriteLine("Rank: " + rank + " | Weighted Percentile: " + modifiedRankPercentiles[rank]);
            }

            return modifiedRankPercentiles;
        }

        public static Dictionary<int, double> GetRankPercentiles(ImageType imageType)
        {
            // sets up the modifiedRankPercentiles variable if it's empty
            if (modifiedRankPercentiles.Count == 0)
            {
                UpdateRankPercentiles(imageType);
            }

            return modifiedRankPercentiles;
        }

        public static void UpdateRankPercentiles(ImageType imageType)
        {
            potentialWeightedRankUpdate = false;
            potentialRegularRankUpdate = false;
            modifiedRankPercentiles = OptionsData.ThemeOptions.WeightedRanks ? GetWeightedRankPercentiles(imageType) : GetModifiedRankPercentiles(imageType);
        }
        #endregion

        #region Events
        private static void RankData_OnParentListAddItem(object sender, ListChangedEventArgs<ReactiveList<string>> e)
        {
            e.Item.OnListAddItem += RankData_OnListAddItem;
            e.Item.OnListRemoveItem += RankData_OnListRemoveItem;

            ImagesOfTypeRankData[ImageType.Static].Insert(e.Index, new List<string>());
            ImagesOfTypeRankData[ImageType.GIF].Insert(e.Index, new List<string>());
            ImagesOfTypeRankData[ImageType.Video].Insert(e.Index, new List<string>());
        }

        private static void RankData_OnParentListRemoveItem(object sender, ListChangedEventArgs<ReactiveList<string>> e)
        {
            e.Item.OnListAddItem -= RankData_OnListAddItem;
            e.Item.OnListRemoveItem -= RankData_OnListRemoveItem;

            ImagesOfTypeRankData[ImageType.Static].RemoveAt(e.Index);
            ImagesOfTypeRankData[ImageType.GIF].RemoveAt(e.Index);
            ImagesOfTypeRankData[ImageType.Video].RemoveAt(e.Index);
        }

        private static void RankData_OnListAddItem(object sender, ListChangedEventArgs<string> e)
        {
            if (!IsLoadingData) // UpdateRankPercentiles will be called once the loading ends
            {
                potentialWeightedRankUpdate = true;
                if ((sender as ReactiveList<string>).Count == 1) // allows the now unempty rank to be selected
                {
                    potentialRegularRankUpdate = true;
                }
            }
        }

        private static void RankData_OnListRemoveItem(object sender, ListChangedEventArgs<string> e)
        {
            if (!IsLoadingData) // UpdateRankPercentiles will be called once the loading ends
            {
                potentialWeightedRankUpdate = true;
                if ((sender as ReactiveList<string>).Count == 0) // prevents the empty rank from being selected
                {
                    potentialRegularRankUpdate = true;
                }
            }
        }
        #endregion
        #endregion Rank Data

        // Active Images
        #region Active Images
        public static bool IsActiveImage(string path) => ActiveImages.Contains(path);

        public static bool NoImagesActive() => ActiveImages.Count == 0;

        /// <summary>
        /// Activate all images within the given folder
        /// </summary>
        /// <param name="folderPath"></param>
        public static void ActivateFolder(string folderPath)
        {
            //! NOTE that this also serves as an initializer for all image's active state on load
            ImageFolders[folderPath] = true; // sets the folder's Active state to true
            EvaluateImageActiveStates(GetImagesOfFolder(folderPath), folderPath, false);
        }

        /// <summary>
        /// Deactivate all images within the given folder
        /// </summary>
        /// <param name="folderPath"></param>
        public static void DeactivateFolder(string folderPath)
        {
            //! NOTE that this also serves as an initializer for all image's active state on load
            ImageFolders[folderPath] = false; // sets the folder's Active state to false
            EvaluateImageActiveStates(GetImagesOfFolder(folderPath), folderPath, true);
        }

        public static void EvaluateImageActiveStates(string[] imagePaths, bool forceDisable) => EvaluateImageActiveStates(imagePaths, null, forceDisable);

        public static void EvaluateImageActiveStates(string[] imagePaths, string folderPath, bool forceDisable)
        {
            foreach (string path in imagePaths)
            {
                if (FileData.ContainsKey(path)) // newly added images that are not included can be detected too
                {
                    if (!IsLoadingData) // calling this is redundant while loading data
                    {
                        FileData[path].EvaluateActiveState(forceDisable);
                    }
                }
                else
                {
                    AddImage(path); // inserts newly added images into the theme, their active state will be determined in the constructor
                }
            }
        }
        #endregion Active Images

        // Image Folders
        #region Image Folders
        public static bool AddFolder(string path)
        {
            return AddFolder(path, true);
        }

        public static bool AddFolder(string path, bool active)
        {
            if (!ImageFolders.ContainsKey(path))
            {
                ImageFolders.Add(path, active);

                if (RankData.Count == 0) //? This is where RankData is initialized for new themes (when the first folder is added)
                {
                    Debug.WriteLine("Initializing RankData for new theme");
                    SetMaxRank(10); // default max rank
                }

                if (active) //? Note that this also adds images to FileData if they have not yet been added
                {
                    ActivateFolder(path);
                }
                else
                {
                    DeactivateFolder(path);
                }

                return true;
            }

            return false;
        }

        public static bool RemoveFolder(string path)
        {
            if (ImageFolders.ContainsKey(path))
            {
                RemoveImages(path);
                ImageFolders.Remove(path);
                return true;
            }

            return false;
        }

        public static string[] GetImagesOfFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                DirectoryInfo directory = new DirectoryInfo(folderPath);
                return (from f in directory.GetFiles() orderby f.FullName descending select f.FullName).ToArray();
            }
            else
            {
                Debug.WriteLine("Attempted to access invalid folder path");
            }

            return new string[0];
        }

        public static void EvaluateImageFolders() // scans for potential new images
        {
            foreach (string folderPath in ImageFolders.Keys)
            {
                foreach (string imagePath in GetImagesOfFolder(folderPath))
                {
                    if (!FileData.ContainsKey(imagePath))
                    {
                        AddImage(imagePath);
                    }
                }
            }
        }
        #endregion Image Folders

        // Misc
        #region Misc
        private static void jpxToJpg(List<FileInfo> jpxFiles)
        {
            foreach (FileInfo file in jpxFiles)
            {
                string fileName = file.FullName;

                if (File.Exists(fileName))
                {
                    Debug.WriteLine("Converting: " + fileName);
                    string extension = ".jpg";

                    string newFileName = Path.ChangeExtension(fileName, extension);

                    if (!File.Exists(newFileName))
                    {
                        File.Copy(fileName, newFileName, true);
                        FileSystem.DeleteFile(fileName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                    else
                    {
                        //TODO Implement this with a more general system that'll be similar to whatever's used for tagging & moving images
                        MessageBox.Show("Name adjustments for duplicate/overlapping file tags have not yet been implemented");
                    }
                }
            }
        }
        #endregion Misc
    }
}
