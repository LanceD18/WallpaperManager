using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using WallpaperManager.Options;
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
                themeOptions = OptionsData.ThemeOptions;
                miscData = new MiscData(); // values are updated in the constructor
                this.imageFolders = imageFolders;
                tagData = TaggingInfo.GetAllCategories();
                this.imageData = imageData;
            }
        }

        public class MiscData
        {
            public PictureStyle wallpaperStyle;
            public int timerIndex;
            public bool randomizeSelection;
            public int maxRank;

            public string tagSortOption;

            public MiscData()
            {
                wallpaperStyle = WallpaperManagerForm.GetWallpaperStyle();
                timerIndex = WallpaperManagerForm.GetTimerIndex();
                randomizeSelection = RandomizeSelection;
                maxRank = GetMaxRank();
                tagSortOption = TagSortOption;
            }
        }

        // Save Data
        public static void SaveData(string path)
        {
            if (path != null)
            {
                JsonWallpaperData jsonWallpaperData = new JsonWallpaperData(FileData.Values.ToArray(), ImageFolders);

                using (StreamWriter file = File.CreateText(path))
                {
                    new JsonSerializer { Formatting = Formatting.Indented }.Serialize(file, jsonWallpaperData);
                }

                PathData.ActiveWallpaperTheme = path;
            }
            else
            {
                Debug.WriteLine("Attempted to save to a null path");
            }
        }

        // Load Data
        public static bool LoadData(string path)
        {
            if (File.Exists(path))
            {
                IsLoadingData = true; // used to speed up the loading process by preventing unnecessary calls
                jpxToJpgWarning = "";

                ResetWallpaperManager();

                //! This must be called before loading JsonWallpaperData to avoid issues
                ResetCoreData();

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

                LoadCoreData(jsonWallpaperData);
                LoadOptionsData(jsonWallpaperData);
                LoadMiscData(jsonWallpaperData);

                if (jpxToJpgWarning != "")
                {
                    MessageBox.Show(jpxStringPrompt + jpxToJpgWarning);
                }

                IsLoadingData = false;
                PathData.ActiveWallpaperTheme = path;
                UpdateRankPercentiles();
                return true;
            }
            else  //! MessageBox warnings for non-existant files should not be used in this method but rather the ones that call it
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
        }

        private static void LoadMiscData(JsonWallpaperData jsonWallpaperData)
        {
            WallpaperManagerForm.UpdateWallpaperStyle(jsonWallpaperData.miscData.wallpaperStyle);
            WallpaperManagerForm.SetTimerIndex(jsonWallpaperData.miscData.timerIndex);
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
