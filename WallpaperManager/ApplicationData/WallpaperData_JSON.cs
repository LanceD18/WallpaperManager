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
using WallpaperManager.Tagging;
using Formatting = Newtonsoft.Json.Formatting;

namespace WallpaperManager.ApplicationData
{
    public static partial class WallpaperData
    {
        public static bool IsLoadingData { get; private set; }
        public static readonly int LargestMaxRank = 1000;
        private static string jpxToJpgWarning;

        public class JsonWallpaperData
        {
            //? Smaller file size, will have to traverse for output
            [JsonProperty("OptionsData")] public OptionsData optionsData;

            [JsonProperty("MiscData")] public MiscData miscData;

            [JsonProperty("ImageFolders")] public Dictionary<string, bool> imageFolders;

            [JsonProperty("TagData")] public CategoryData[] tagData;

            [JsonProperty("ImageData")] public ImageData[] imageData;

            public JsonWallpaperData(ImageData[] imageData, Dictionary<string, bool> imageFolders)
            {
                //? This handles saving | Don't go to this code segment for modifying how data is loaded
                optionsData = new OptionsData();
                miscData = new MiscData();
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
                randomizeSelection = WallpaperData.RandomizeSelection;
                maxRank = GetMaxRank();
                tagSortOption = TagSortOption;
            }
        }

        public class OptionsData
        {
            public bool largerImagesOnLargerMonitors;
            public bool higherRankedImagesOnLargerMonitors;
            public bool enableDetectionOfInactiveImages;

            public OptionsData()
            {
                largerImagesOnLargerMonitors = Options.OptionsData.LargerImagesOnLargerMonitors;
                higherRankedImagesOnLargerMonitors = Options.OptionsData.HigherRankedImagesOnLargerMonitors;
                enableDetectionOfInactiveImages = Options.OptionsData.EnableDetectionOfInactiveImages;
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
        public static void LoadData(string path)
        {
            if (File.Exists(path))
            {
                IsLoadingData = true;
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
                    return;
                }

                LoadCoreData(jsonWallpaperData);
                LoadOptionsData(jsonWallpaperData);
                LoadMiscData(jsonWallpaperData);

                if (jpxToJpgWarning != "")
                {
                    MessageBox.Show(jpxStringPrompt + jpxToJpgWarning);
                }

                IsLoadingData = false;
            }
            else  // MessageBox warnings for non-existant files should not be used in this method but rather the ones that call it
            {
                Debug.WriteLine("Attempted to load a non-existant file");
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
            SetRankData(LargestMaxRank, true);
        }

        private static void ResetWallpaperManager()
        {
            WallpaperManagerForm.ClearImageSelector();
        }

        private static void LoadCoreData(JsonWallpaperData jsonWallpaperData)
        {
            SetRankMax(jsonWallpaperData.miscData.maxRank, true);

            // Must be set before the foreach loop where AddImage is called so that the available tags and categories can exist
            TaggingInfo = new TaggingInfo(jsonWallpaperData.tagData.ToList());

            foreach (CategoryData category in TaggingInfo.GetAllCategories())
            {
                category.Initialize(false);
            }

            // All tags will be linked through the AddImage method
            string invalidImagesString = "A few image files for your theme appear to be missing.\nThe following image's will not be saved to your theme: \n";
            foreach (ImageData image in jsonWallpaperData.imageData)
            {
                if (!AddImage(image))
                {
                    invalidImagesString += "\n" + image.Path;
                }
            }

            if (invalidImagesString.Contains("\n\n"))
            {
                MessageBox.Show(invalidImagesString);
            }

            // Activates Images
            WallpaperManagerForm.LoadImageFolders(jsonWallpaperData.imageFolders);
        }

        private static void LoadOptionsData(JsonWallpaperData jsonWallpaperData)
        {
            Options.OptionsData.LargerImagesOnLargerMonitors = jsonWallpaperData.optionsData.largerImagesOnLargerMonitors;
            Options.OptionsData.HigherRankedImagesOnLargerMonitors = jsonWallpaperData.optionsData.higherRankedImagesOnLargerMonitors;
            Options.OptionsData.EnableDetectionOfInactiveImages = jsonWallpaperData.optionsData.enableDetectionOfInactiveImages;
        }

        private static void LoadMiscData(JsonWallpaperData jsonWallpaperData)
        {
            WallpaperManagerForm.UpdateWallpaperStyle(jsonWallpaperData.miscData.wallpaperStyle);
            WallpaperManagerForm.SetTimerIndex(jsonWallpaperData.miscData.timerIndex);
            RandomizeSelection = jsonWallpaperData.miscData.randomizeSelection;
            TagSortOption = jsonWallpaperData.miscData.tagSortOption;
        }

        public static void SaveDefaultData()
        {
            SaveData(PathData.DefaultWallpaperData);
        }

        public static void LoadDefaultData()
        {
            LoadData(PathData.DefaultWallpaperData);
        }
    }
}
