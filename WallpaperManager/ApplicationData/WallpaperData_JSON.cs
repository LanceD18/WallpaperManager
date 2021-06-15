using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using WallpaperManager.Controls;
using WallpaperManager.Options;
using WallpaperManager.Pathing;
using WallpaperManager.Tagging;
using Formatting = Newtonsoft.Json.Formatting;

namespace WallpaperManager.ApplicationData
{
    public static partial class WallpaperData
    {
        public static bool IsLoadingData { get; private set; } = false;
        public static bool IsLoadingImageFolders { get; private set; } = false;
        public static readonly int LargestMaxRank = 1000;
        private static string jpxToJpgWarning;

        public static Thread SavingThread;

        public class JsonWallpaperData
        {
            [JsonProperty("ThemeOptions")] public ThemeOptions themeOptions;

            [JsonProperty("MiscData")] public MiscData miscData;

            [JsonProperty("ImageFolders")] public Dictionary<string, bool> imageFolders;

            [JsonProperty("TagData")] public CategoryData[] tagData;

            //! ImageData MUST remain at the bottom at all times to ensure that the needed info above is loaded first
            //! (This allows you to initialize more data in the constructor like the EvaluateActiveState() method)
            [JsonProperty("ImageData")] public ImageData[] imageData;

            public JsonWallpaperData(ImageData[] imageData, Dictionary<string, bool> imageFolders)
            {
                //! This handles SAVING!!! | Don't go to this code segment for modifying how data is loaded!
                miscData = new MiscData(); // values are updated in the constructor
                themeOptions = OptionsData.ThemeOptions;
                this.imageFolders = imageFolders;
                tagData = TaggingInfo.GetAllCategories();
                this.imageData = imageData;
            }
        }

        public class MiscData
        {
            // Display Settings
            public DisplaySettings displaySettings;

            // Options
            public bool randomizeSelection;
            public int maxRank;

            // Tagging Settings
            public string tagSortOption;

            public MiscData()
            {
                // Display Settings
                displaySettings.WallpaperStyles = WallpaperManagerForm.GetWallpaperStyles();
                displaySettings.WallpaperIntervals = WallpaperManagerForm.GetTimerIndexes();
                displaySettings.Synced = WallpaperManagerForm.DisplaySettingsSynced;

                // Options
                randomizeSelection = RandomizeSelection;
                maxRank = GetMaxRank();

                // Tagging Settings
                tagSortOption = TagSortOption;
            }
        }

        // Save Data
        public static void SaveData(string path)
        {
            //xif (SavingThread != null && SavingThread.IsAlive) return;

            if (path != null)
            {
                //-----Backup-----
                // Create a temporary backup in the save file's directory for just in case something goes wrong during the saving process
                FileInfo pathFile = new FileInfo(path);
                string tempPathName = pathFile.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(pathFile.Name) + "_TEMP_BACKUP";
                string tempPath = tempPathName + pathFile.Extension;

                // for just in case the user ends up with multiple accidents, we don't want to overwrite any backups with the damaged file
                if (File.Exists(tempPath))
                {
                    int tempPathCount = 1;
                    string newTempPath = tempPath;
                    while (File.Exists(newTempPath))
                    {
                        newTempPath = tempPathName + "_" + tempPathCount + pathFile.Extension; // updates the temp path name with a number
                        tempPathCount++;
                    }

                    tempPath = newTempPath;
                }

                Debug.WriteLine("Temp File Location: " + tempPath);
                File.Copy(path, tempPath);

                //-----Write to JSON File-----
                //x using a regular Task.Run process here will cause the program to crash (and save to be incomplete)
                //x if this method is accessed too rapidly this allows this method to only be accessed if the thread is done
                //xSavingThread = new Thread(() =>
                //x{
                    //? The thread was removed so that any potential changes made to the program would be impossible while saving
                    //? If you can make this safer in the future, go ahead and put it back in but for now it should stay like this
                    //! Also NOTE: Some of the data accessed require using controls, which will cause an error involving accessing a control from the wrong stream
                    //! Another NOTE: I believe making this a thread may have caused the Default Theme save errors

                    Debug.WriteLine("Saving to: " + path);

                    JsonWallpaperData jsonWallpaperData = new JsonWallpaperData(FileData.Values.ToArray(), ImageFolders);
                    
                    Debug.WriteLine("Writing to JSON file");
                    using (StreamWriter file = File.CreateText(path))
                    {
                        new JsonSerializer {Formatting = Formatting.Indented}.Serialize(file, jsonWallpaperData);
                    }
                //x});
                //xSavingThread.Start();

                //-----Remove the backup-----
                File.Delete(tempPath);
            }
            else
            {
                Debug.WriteLine("Attempted to save to a null path");
            }
        }

        // Load Data
        public static bool LoadData(string path)
        {
            Debug.WriteLine("Load Data");

            if (File.Exists(path))
            {
                IsLoadingData = true; // used to speed up the loading process by preventing unnecessary calls
                jpxToJpgWarning = "";

                Debug.WriteLine("Resetting Wallpaper Manager");

                ResetWallpaperManager();

                Debug.WriteLine("Resetting Core Data");
                //! This must be called before loading JsonWallpaperData to avoid issues
                ResetCoreData();

                Debug.WriteLine("Loading JSON Data");
                //? RankData and ActiveImages will both be automatically set when jsonWallpaperData is loaded as the constructors for ImageData is what sets them
                JsonWallpaperData jsonWallpaperData;
                using (StreamReader file = File.OpenText(path))
                {
                    jsonWallpaperData = new JsonSerializer().Deserialize(file, typeof(JsonWallpaperData)) as JsonWallpaperData;
                }

                if (jsonWallpaperData == null)
                {
                    MessageBox.Show("Load failed");
                    return false;
                }

                Debug.WriteLine("Inserting JSON Data");
                LoadCoreData(jsonWallpaperData);
                LoadOptionsData(jsonWallpaperData);
                LoadMiscData(jsonWallpaperData);

                if (jpxToJpgWarning != "")
                {
                    MessageBox.Show(jpxStringPrompt + jpxToJpgWarning);
                }

                IsLoadingData = false;
                WallpaperPathing.ActiveWallpaperTheme = path;
                UpdateRankPercentiles(ImageType.None); //! Now that image types exist this preemptive change may not be worth it

                Debug.WriteLine("Finished Loading");
                return true;
            }
            else //! MessageBox warnings for non-existant files should not be used in this method but rather the ones that call it
            {
                Debug.WriteLine("Attempted to load a non-existant file");
                return false;
            }
        }

        private static void ResetCoreData()
        {
            int oldRankMax = RankData.Count - 1;

            FileData.Clear(); // AddImage handles most of FileData
            RankData.Clear(); //? Loaded in when jsonWallpaperData is created
            ActiveImages.Clear(); //? Loaded in when jsonWallpaperData is created
            ImageFolders.Clear();
            TaggingInfo = new TaggingInfo();

            ImagesOfType.Clear();
            ImagesOfTypeRankData.Clear();
            ActiveImagesOfType.Clear();

            WallpaperPathing.Reset();

            InitializeImagesOfType();

            // This is needed if loading otherwise images with invalid ranks will crash the program
            SetRankData(LargestMaxRank);
        }

        private static void ResetWallpaperManager()
        {
            WallpaperManagerForm.ResetWallpaperManager();
        }

        private static void LoadCoreData(JsonWallpaperData jsonWallpaperData)
        {
            SetMaxRank(jsonWallpaperData.miscData.maxRank);

            //? Must be set before the foreach loop where AddImage is called so that the available tags and categories can exist
            TaggingInfo = new TaggingInfo(jsonWallpaperData.tagData.ToList());

            foreach (CategoryData category in TaggingInfo.GetAllCategories())
            {
                category.Initialize(false);
            }

            // All tags will be linked through the AddImage method
            string invalidImagesString = "A few image files for your theme appear to be missing.\nThe following image's will not be saved to your theme: \n";
            foreach (ImageData image in jsonWallpaperData.imageData)
            {
                if (AddImage(image) == null)
                {
                    invalidImagesString += "\n" + image.Path;
                }
            }

            if (invalidImagesString.Contains("\n\n"))
            {
                MessageBox.Show(invalidImagesString);
            }

            // since activating an image folder also adds undeteced images, this needs to be loaded last
            IsLoadingImageFolders = true; // this is used to override the IsLoading bool for new images added when loading image folders
            WallpaperManagerForm.LoadImageFolders(jsonWallpaperData.imageFolders);
            IsLoadingImageFolders = false;
        }

        private static void LoadOptionsData(JsonWallpaperData jsonWallpaperData)
        {
            OptionsData.ThemeOptions = jsonWallpaperData.themeOptions;

            // this is only really needed when adding in new options, a minor convenience feature to prevent errors when loading the default theme
            OptionsData.InitializePotentialNulls();
        }

        private static void LoadMiscData(JsonWallpaperData jsonWallpaperData)
        {
            WallpaperManagerForm.UpdateWallpaperStyle(jsonWallpaperData.miscData.displaySettings.WallpaperStyles);
            WallpaperManagerForm.SetTimerIndex(jsonWallpaperData.miscData.displaySettings.WallpaperIntervals, true);
            RandomizeSelection = jsonWallpaperData.miscData.randomizeSelection;
            TagSortOption = jsonWallpaperData.miscData.tagSortOption;
        }

        public static void LoadDefaultTheme()
        {
            if (LoadData(Properties.Settings.Default["DefaultTheme"] as string))
            {
                WallpaperManagerForm.NextWallpaper();
            }
        }
    }
}