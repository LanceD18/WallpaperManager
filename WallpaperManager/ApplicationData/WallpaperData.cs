using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using WallpaperManager.Tagging;
using Formatting = Newtonsoft.Json.Formatting;

namespace WallpaperManager.ApplicationData
{
    public static partial class WallpaperData
    {
        public static WallpaperManager WallpaperManagerForm;

        private static Dictionary<string, ImageData> FileData = new Dictionary<string, ImageData>();
        private static Dictionary<int, List<string>> RankData = new Dictionary<int, List<string>>(); //? Add and removal should be automated, you should only need to retrieve data from this
        private static double[] rankPercentiles;

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

            if (useLastLoadedTheme)
            {
                LoadDefaultTheme();
            }
        }

        // File Data
        #region File Data
        public static bool AddImage(ImageData newImageData)
        {
            if (File.Exists(newImageData.Path))
            {
                Tagging.LinkImageTags(newImageData);
                FileData.Add(newImageData.Path, newImageData);
                return true;
            }
            else
            {
                Debug.WriteLine("Attempted to create an invalid image: " + newImageData.Path);
                return false;
            }
        }

        public static bool AddImage(string path, int rank = 0, bool active = false, Dictionary<string, HashSet<string>> tags = null)
        {
            tags = tags ?? new Dictionary<string, HashSet<string>>(); // if null, the right-hand side of ?? will be called
            return AddImage(new ImageData(path, rank, active, tags));
        }

        public static void RemoveImage(string path)
        {
            RemoveImages(new string[] {path});
        }

        public static void RemoveImages(string folderPath)
        {
            RemoveImages(GetImagesOfFolder(folderPath));
        }

        public static void RemoveImages(string[] paths)
        {
            foreach (string path in paths)
            {
                Tagging.UnlinkImageTags(FileData[path]);

                RankData[FileData[path].Rank].Remove(path);
                FileData.Remove(path);
                ActiveImages.Remove(path);
            }
        }

        public static ImageData GetImageData(string path)
        {
            return FileData[path];
        }

        public static ImageData[] GetAllImageData()
        {
            return FileData.Values.ToArray();
        }

        public static int GetImageRank(string path)
        {
            return ContainsImage(path) ? FileData[path].Rank : -1;
        }

        public static bool ContainsImage(string path)
        {
            return FileData.ContainsKey(path);
        }

        public static bool FileDataIsEmpty()
        {
            return FileData.Count == 0;
        }
        #endregion File Data

        // Rank Data
        #region Rank Data
        public static string[] GetImagesOfRank(int rank)
        {
            return RankData.ContainsKey(rank) ? RankData[rank].ToArray() : null;
        }

        public static string[] GetAllRankedImages()
        {
            string[] imagePaths = new string[0];
            int arraySize = 0;

            foreach (int rank in RankData.Keys)
            {
                if (rank != 0 && RankData[rank].Count != 0) // ensures that rank 0 images are not included since they are 'unranked'
                {
                    string[] currentRankArray = RankData[rank].ToArray();
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

        public static string GetRandomImageOfRank(int rank, ref Random rand)
        {
            //! Remove this if statement at some point, once percentiles are properly implemented you won't need to check this. Don't forget to reference this error however!
            if (RankData[rank].Count != 0) // without this if statement empty ranks will go out of bounds
            {
                int randomImage = rand.Next(0, RankData[rank].Count);
                return RankData[rank][randomImage];
            }

            return "";
        }

        public static bool ContainsRank(int rank)
        {
            return RankData.ContainsKey(rank);
        }

        public static void SetRankMax(int newRankMax, bool loadingData) //? This is the primary initializer for Rank Data
        {
            if (newRankMax > 0) // note that rank 0 is reserved for unranked images
            {
                SetRankData(newRankMax, loadingData);
                SetRankPercentiles(newRankMax);
            }
            else
            {
                MessageBox.Show("The max rank cannot be equal to or less than 0");
            }
        }

        private static void SetRankData(int newRankMax, bool loadingData)
        {
            // Set RankData
            if (RankData.Count == 0) // Initialize RankData
            {
                RankData.Add(0, new List<string>());

                for (int i = 0; i < newRankMax; i++)
                {
                    RankData.Add(i + 1, new List<string>());
                }
            }
            else // Update RankData
            {
                if (loadingData || MessageBox.Show("Are you sure you want to change the max rank?\n(All images will have their ranks adjusted according to this change)",
                        "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    UpdateMaxRank(newRankMax, loadingData);
                }
            }
        }

        private static void UpdateMaxRank(int newRankMax, bool loadingData)
        {
            int oldRankMax = GetMaxRank();
            float rankChangeRatio = (float)newRankMax / oldRankMax;

            //! This needs to be placed right here otherwise ImageData will crash on trying to add the image to an unknown rank
            // Increase RankData's possible ranks if needed
            if (rankChangeRatio > 1) // newest rank max is higher than the current rank max
            {
                for (int i = oldRankMax; i < newRankMax; i++)
                {
                    RankData.Add(i + 1, new List<string>());
                }
            }

            if (!loadingData) // no need to update ranks if you aren't actually changing anything
            {
                // Re-rank existing images
                string[] images = FileData.Keys.ToArray();
                foreach (string image in images)
                {
                    if (FileData[image].Rank != 0) // due to the Math.Max() used below this if statement is needed otherwise all rank 0 images would be set to 1
                    {
                        int newRank = Math.Max((int) Math.Round((double) FileData[image].Rank * rankChangeRatio), 1); // the Math.Max is used to ensure that no images are set to 0 (unranked)
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
                    RankData.Remove(i);
                }
            }

            //Debug.WriteLine(GetMaxRank());
        }

        private static void SetRankPercentiles(int newRankMax)
        {
            // Set Rank Percentiles
            rankPercentiles = new double[newRankMax];
            double rankMultiplier = 10.0 / newRankMax;

            for (int i = 0; i < newRankMax; i++)
            {
                //TODO Add a system for modifying percentile ranges directly or changing up the formula
                // This is the default formula for rank percentiles, where each percentile has twice the probability of the previous one
                //? Note that the below formula does not include rank 0 | 0 * rankMultiplier is rank 1
                rankPercentiles[i] = Math.Pow(2, i * rankMultiplier);
            }
        }

        public static Dictionary<int, double> GetModifiedRankPercentiles()
        {
            double rankPercentagesTotal = 0;
            List<int> validRanks = new List<int>();
            foreach (int rank in RankData.Keys)
            {
                if (RankData[rank].Count != 0 && rank != 0)
                {
                    rankPercentagesTotal += rankPercentiles[rank - 1];
                    validRanks.Add(rank);
                }
            }

            Dictionary<int, double> modifiedRankPercentiles = new Dictionary<int, double>();

            foreach (int rank in validRanks)
            {
                modifiedRankPercentiles.Add(rank, rankPercentiles[rank - 1] / rankPercentagesTotal);
            }

            return modifiedRankPercentiles;
        }

        public static int GetMaxRank()
        {
            return RankData.Count - 1; // note that RankData.Count includes rank 0 which makes this 1 higher than the actual max rank
        }
        #endregion Rank Data

        // Active Images
        #region Active Images
        public static bool IsActiveImage(string path)
        {
            return ActiveImages.Contains(path);
        }

        public static bool NoImagesActive()
        {
            return ActiveImages.Count == 0;
        }

        /// <summary>
        /// Activate the given image
        /// </summary>
        /// <param name="path"></param>
        public static void ActivateImage(string path)
        {
            ActivateImages(new string[] { path });
        }

        /// <summary>
        /// Activate all images within the given folder
        /// </summary>
        /// <param name="folderPath"></param>
        public static void ActivateImages(string folderPath)
        {
            ActivateImages(GetImagesOfFolder(folderPath));
        }

        /// <summary>
        /// Activate all given images
        /// </summary>
        /// <param name="paths"></param>
        public static void ActivateImages(string[] paths)
        {
            //? Not yet needed, I have yet to find any file types I'd deem unsupported
            /*
            string unsupportedFileTypes = "The following files are unusable due to their file types: \n";
            string unsupportedFileTypesDefault = unsupportedFileTypes;
            */

            List<FileInfo> jpxFiles = new List<FileInfo>();
            string jpxString = "";

            //TODO Ensure that the message box doesn't go off screen!
            /*TODO
             * Also ensure that you don't load data for multiple folders:
             * To do this, check if you are currently loading data, if so save the message and display it with all results combined after loading your data
             * If not loafing, then it can just display one folder as normal since more than likely this won't be an issue
            */

            foreach (string path in paths)
            {
                if (path.Contains(".jpeg") || path.Contains(".jpg_large"))
                {
                    jpxFiles.Add(new FileInfo(path));
                    jpxString += "\n" + path;
                }

                if (FileData.ContainsKey(path))
                {
                    bool canEnable = true;
                    ImageData image = FileData[path];

                    foreach (string category in image.Tags.Keys)
                    {
                        foreach (string tag in image.Tags[category])
                        {
                            if (!TaggingInfo.GetTag(category, tag).Enabled)
                            {
                                canEnable = false;
                                break;
                            }
                        }

                        if (!canEnable) { break; }
                    }

                    FileData[path].Active = canEnable;
                }
                else
                {
                    AddImage(path, 0, true);
                }
            }

            //? Not yet needed, I have yet to find any file types I'd deem unsupported
            /*
            if (unsupportedFileTypesDefault == unsupportedFileTypes)
            {
                MessageBox.Show(unsupportedFileTypes);
            }
            */

            if (jpxFiles.Count > 0)
            {
                /*
                if (IsLoadingData)
                {
                    jpxToJpgWarning += jpxString;
                }
                else
                {
                    MessageBox.Show(jpxStringPrompt + jpxString);
                }

                DialogResult result = MessageBox.Show(jpxString, "Choose an option", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    jpxToJpg(jpxFiles);
                }
                */
            }
        }

        /// <summary>
        /// Deactivate the given image
        /// </summary>
        /// <param name="path"></param>
        public static void DeactivateImage(string path)
        {
            DeactivateImages(new string[] { path });
        }

        /// <summary>
        /// Deactivate all images within the given folder
        /// </summary>
        /// <param name="folderPath"></param>
        public static void DeactivateImages(string folderPath)
        {
            DeactivateImages(GetImagesOfFolder(folderPath));
        }

        /// <summary>
        /// Deactivate all given images
        /// </summary>
        /// <param name="paths"></param>
        public static void DeactivateImages(string[] paths)
        {
            foreach (string path in paths)
            {
                if (FileData.ContainsKey(path))
                {
                    FileData[path].Active = false;
                }
                else
                {
                    AddImage(path);
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
                    SetRankMax(10, false); // default max rank
                }

                if (active) //? Note that this also adds images to FileData if they have not yet been added
                {
                    ActivateImages(path);
                }
                else
                {
                    DeactivateImages(path);
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
                        //TODO Implement this with a more general system that'll be similar to whatever's used for taggin & moving images
                        MessageBox.Show("Name adjustments for duplicate/overlapping file tags have not yet been implemented");
                    }
                }
            }
        }
        #endregion Misc
    }
}
