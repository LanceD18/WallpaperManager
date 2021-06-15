using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Navigation;
using LanceTools;
using Newtonsoft.Json;
using WallpaperManager.Tagging;

namespace WallpaperManager.ApplicationData
{
    public static partial class WallpaperData
    {
        // TODO Consider merging ImagesOfType & ImagesOfTypeRankData with FileData & RankData [NOTE, this will add more loops to your general functions so I'd honestly advise against the merge]

        //! These ImageOfType variables are initialized under WallpaperData.InitializeImagesOfType() due to the fact that every time a theme is loaded these will be cleared so its
        //! best to have them initialized there instead of here

        // used to give the user more selection options
        private static Dictionary<ImageType, Dictionary<string, ImageData>> ImagesOfType;

        //? this doesn't need to be reactive lists since the regular RankData does enough to handle the issue presented (Checking if rank percentiles should be updated)
        private static Dictionary<ImageType, List<List<string>>> ImagesOfTypeRankData;

        private static Dictionary<ImageType, double> ImageTypeWeights = new Dictionary<ImageType, double>()
        {
            {ImageType.Static, 0},
            {ImageType.GIF, 0},
            {ImageType.Video, 0}
        };

        //! No longer needed currently, consider removing this in the future
        private static Dictionary<ImageType, List<string>> ActiveImagesOfType;

        public static string[] GetAllImagesOfType(ImageType imageType) => ImagesOfType[imageType].Keys.ToArray();

        public static int GetImagesOfTypeRankSum(ImageType imageType)
        {
            int count = 0;
            for (var i = 1; i < ImagesOfTypeRankData[imageType].Count; i++) //? i starts at 1 since rank 0 images are not included [Although they are likely inactive anyways]
            {
                List<string> rank = ImagesOfTypeRankData[imageType][i];
                count += rank.Count * i; // i = rank
            }

            return count;
        }

        public static void UpdateImageTypeWeights()
        {
            int totalSum = 0;
            Dictionary<ImageType, int> ImageTypeRankSum = new Dictionary<ImageType, int>();
            foreach (ImageType imageType in ImageTypeWeights.Keys)
            {
                int sum = GetImagesOfTypeRankSum(imageType);
                ImageTypeRankSum.Add(imageType, sum);
                totalSum += sum;
            }

            foreach (ImageType imageType in ImageTypeRankSum.Keys)
            {
                ImageTypeWeights[imageType] = (double)ImageTypeRankSum[imageType] / totalSum;
            }
        }

        public static double GetImageOfTypeWeight(ImageType imageType) => ImageTypeWeights[imageType];

        public static bool IsAllImagesOfTypeUnranked(ImageType imageType) => ImagesOfTypeRankData[imageType][0].Count == ImagesOfType[imageType].Count;

        public struct VideoSettings
        {
            public int Volume;
            public double PlaybackSpeed;

            public VideoSettings(int Volume, double PlaybackSpeed)
            {
                this.Volume = Volume;
                this.PlaybackSpeed = PlaybackSpeed;
            }
        }

        public class ImageData
        {
            [DataMember(Name = "Path")]
            public string Path { get; private set; } //? If you need to change this, FileData's field must be changed into List<ImageData>

            [JsonIgnore]
            public string PathFolder { get; private set; }
            
            [DataMember(Name = "Rank")]
            private int _Rank;
            public int Rank
            {
                get => _Rank;

                set
                {
                    if (value <= GetMaxRank() && value >= 0) // prevents stepping out of valid rank bounds
                    {
                        if (Active) // Rank Data does not include inactive images
                        {
                            RankData[_Rank].Remove(Path);
                            RankData[value].Add(Path);

                            ImagesOfTypeRankData[imageType][_Rank].Remove(Path);
                            ImagesOfTypeRankData[imageType][value].Add(Path);
                        }

                        _Rank = value; // place this after the above if statement to ensure that the right image file path is found
                    }
                }
            }

            [DataMember(Name = "Active")]
            private bool _Active;
            public bool Active
            {
                get => _Active;

                private set
                {
                    if (_Active != value) // Prevents element duplication whenever active is set to the same value
                    {
                        _Active = value;

                        if (value)
                        {
                            RankData[_Rank].Add(Path);
                            ImagesOfTypeRankData[imageType][_Rank].Add(Path);

                            ActiveImages.Add(Path);
                            ActiveImagesOfType[imageType].Add(Path);
                        }
                        else  // Note that Rank Data does not include inactive images
                        {
                            RankData[_Rank].Remove(Path);
                            ImagesOfTypeRankData[imageType][_Rank].Remove(Path);

                            ActiveImages.Remove(Path);
                            ActiveImagesOfType[imageType].Remove(Path);
                        }
                    }
                }
            }

            [DataMember(Name = "Enabled")] 
            private bool _Enabled = true;
            public bool Enabled // this is the image's individual enabled state, if this is false then nothing else can make the image active
            {
                get => _Enabled;

                set
                {
                    _Enabled = value;
                    Active = value;
                }
            }

            [DataMember(Name = "Tags")] public Dictionary<string, HashSet<string>> Tags; // this should stay as a string for saving to JSON | Represents: Dictionary<CategoryName, HashSet<TagName>>

            [DataMember(Name = "Tag Naming Exceptions")] public HashSet<Tuple<string, string>> TagNamingExceptions; // these tags be used for naming regardless of constraints

            [DataMember(Name = "Image Type")] public ImageType imageType;

            [DataMember(Name = "Video Settings")] public VideoSettings VideoSettings = new VideoSettings(100, 1); // only applicable to images with the corresponding image type

            public ImageData(string path, int rank, bool active, Dictionary<string, HashSet<string>> tags = null, HashSet<Tuple<string, string>> tagNamingExceptions = null)
            {
                FileInfo file = new FileInfo(path);

                InitializeImageType(file, path); //? needs to be done before a rank is set

                Path = path;
                PathFolder = file.Directory.FullName;
                Rank = rank;
                Active = active;
                Tags = tags ?? new Dictionary<string, HashSet<string>>();
                TagNamingExceptions = tagNamingExceptions ?? new HashSet<Tuple<string, string>>();

                if (!IsLoadingData || IsLoadingImageFolders) // image that are loaded-in already have the proper settings | IsLoadingImageFolders overrides this for actual new images
                {
                    EvaluateActiveState(false);
                }
            }

            private void InitializeImageType(FileInfo file, string path)
            {
                if (imageType == ImageType.None)
                {
                    if (!WallpaperManagerTools.IsSupportedVideoType(file))
                    {
                        if (file.Extension != ".gif")
                        {
                            imageType = ImageType.Static;
                        }
                        else
                        {
                            imageType = ImageType.GIF;
                        }
                    }
                    else
                    {
                        imageType = ImageType.Video;
                    }
                }

                //? This has been moved to AddImage() alongside FileData, without doing this you'll end up accidentally adding the same object twice, causing a crash
                //?ImagesOfType[imageType].Add(path, this);
            }

            public void UpdatePath(string newPath)
            {
                RemoveImage(Path);

                Path = newPath;
                PathFolder = new FileInfo(Path).Directory.FullName;

                AddImage(this);
            }

            public void AddTag(CategoryData category, string tag) => AddTag(TaggingInfo.GetTag(category, tag));

            public void AddTag(TagData tag)
            {
                string categoryName = tag.ParentCategoryName;

                if (TaggingInfo.ContainsCategory(TaggingInfo.GetTagParentCategory(tag)))
                {
                    if (TaggingInfo.ContainsTag(tag))
                    {
                        if (!Tags.ContainsKey(categoryName)) // the category exists, however this image has not yet included it
                        {
                            Tags.Add(categoryName, new HashSet<string>());
                        }

                        Tags[categoryName].Add(tag.Name);
                        tag.LinkImage(this);

                        // Link all parent tags of the current tag
                        foreach (Tuple<string, string> parentTag in tag.ParentTags)
                        {
                            AddTag(TaggingInfo.GetTag(parentTag.Item1, parentTag.Item2));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Category [" + categoryName + "] does not contain tag [" + tag.Name + "]");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Category: " + categoryName);
                }

                EvaluateActiveState(false);
            }

            public void RemoveTag(TagData tag)
            {
                if (!CheckIfTagIsParent(tag)) // if this tag is the parent of another tag it cannot be removed until the child tag is removed
                {
                    string categoryName = tag.ParentCategoryName;
                    string tagName = tag.Name;

                    if (Tags.ContainsKey(categoryName))
                    {
                        Tags[categoryName].Remove(tagName);
                        TaggingInfo.GetTag(tag.ParentCategoryName, tagName).UnlinkImage(this);
                    }
                }

                EvaluateActiveState(false);
            }

            public TagData[] GetTags()
            {
                List<TagData> foundTags = new List<TagData>();
                foreach (string category in Tags.Keys)
                {
                    foreach (string tag in Tags[category])
                    {
                        foundTags.Add(TaggingInfo.GetTag(category, tag));
                    }
                }

                return foundTags.ToArray();
            }

            public bool ContainsTag(TagData tag)
            {
                foreach (string curCategory in Tags.Keys)
                {
                    if (tag.ParentCategoryName == curCategory)
                    {
                        foreach (string curTag in Tags[curCategory])
                        {
                            if (curTag == tag.Name)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }

            public bool CheckIfTagIsParent(TagData tag) => CheckIfTagIsParent(tag, out var dummyChildTagName);

            public bool CheckIfTagIsParent(TagData tag, out string childTagName)
            {
                string categoryName = tag.ParentCategoryName;
                string tagName = tag.Name;

                if (Tags.ContainsKey(categoryName))
                {
                    if (Tags[categoryName].Contains(tagName))
                    {
                        foreach (Tuple<string, string> tagInfo in tag.ChildTags)
                        {
                            if (Tags.ContainsKey(tagInfo.Item1)) // without this child tags from different categories will cause a crash if the image does not have said tag
                            {
                                if (Tags[tagInfo.Item1].Contains(tagInfo.Item2))
                                {
                                    childTagName = tagInfo.Item2;
                                    return true;
                                }
                            }
                        }
                    }
                }

                childTagName = "";
                return false;
            }

            public void RenameCategory(string oldName, string newName)
            {
                Tags.Add(newName, Tags[oldName]);
                Tags.Remove(oldName);
            }

            public void RenameTag(string category, string oldName, string newName)
            {
                HashSet<string> tempTagList = new HashSet<string>();

                foreach (string tag in Tags[category])
                {
                    tempTagList.Add(tag == oldName ? newName : tag);
                }

                Tags[category] = tempTagList;
            }
            
            public Dictionary<string, HashSet<string>> OrderTags()
            {
                Dictionary<string, HashSet<string>> orderedTags = new Dictionary<string, HashSet<string>>();

                foreach (CategoryData category in TaggingInfo.GetAllCategories())
                {
                    string categoryName = category.Name;
                    if (Tags.ContainsKey(categoryName))
                    {
                        orderedTags.Add(categoryName, Tags[categoryName]);
                    }
                }

                return orderedTags;
            }

            /// <summary>
            /// Generates a name based on an image's tags. Note that spaces will be converted into _
            /// </summary>
            /// <returns></returns>
            public string GetTaggedName()
            {
                string taggedName = "";

                foreach (CategoryData category in TaggingInfo.GetAllCategories()) //? ordering matters here so we'll have to get the proper order
                {
                    string categoryName = category.Name;
                    if (Tags.ContainsKey(categoryName)) //? remember that imageData.Tags may be ordered improperly
                    {
                        //? This block will set the alphabetic order of every tag contained within this category before moving onto the next category

                        //List<string> tagsOrderedByCount = Tags[category].OrderBy(t => TaggingInfo.GetTag(category, t).GetLinkedImageCount()).ToList();
                        List<string> alphabeticTags = Tags[categoryName].OrderBy(t => t).ToList();

                        foreach (TagData tag in GetTags())
                        {
                            //? don't allow the contains check to cover this entire block as it'll cause issues with handling parent tag removal
                            if (alphabeticTags.Contains(tag.Name))
                            {
                                Tuple<string, string> curTagInfo = new Tuple<string, string>(categoryName, alphabeticTags[alphabeticTags.IndexOf(tag.Name)]);

                                // tags with a naming exception can be used regardless of constraints
                                if (TagNamingExceptions.Contains(curTagInfo)) continue;
                            }

                            // removes tags that are specifically disabled from renaming
                            if (tag.ParentCategoryName == categoryName && !tag.UseForNaming)
                            {
                                alphabeticTags.Remove(tag.Name);
                                continue;
                            }

                            // remove parent tags
                            //? this loop should continue for the entire list as a tag can have multiple parents
                            for (int i = alphabeticTags.Count - 1; i >= 0; i--)
                            {
                                Tuple<string, string> tagInfo = new Tuple<string, string>(categoryName, alphabeticTags[i]);
                                // this prevents the parent tags of a tag from being included
                                //? yes TagNamingExceptions should be checked here too otherwise some parent tags may be removed
                                if (tag.ParentTags.Contains(tagInfo) && !TagNamingExceptions.Contains(tagInfo))
                                {
                                    alphabeticTags.RemoveAt(i);
                                }
                            }
                        }

                        foreach (string tag in alphabeticTags)
                        {
                            taggedName += tag;
                        }
                    }
                }

                return taggedName.Replace(' ', '_');
            }

            public bool ToggleTagNamingException(string tag)
            {
                foreach (string category in Tags.Keys)
                {
                    if (Tags[category].Contains(tag))
                    {
                        Tuple<string, string> tagInfo = new Tuple<string, string>(category, tag);
                        if (TagNamingExceptions.Contains(tagInfo))
                        {
                            TagNamingExceptions.Remove(tagInfo);
                            return false;
                        }
                        else
                        {
                            TagNamingExceptions.Add(tagInfo);
                            return true;
                        }
                    }
                }

                return false;
            }

            public void EvaluateActiveState(bool forceDisable) //? you CAN'T forceEnable, the image will have to go through the evaluation to be enabled
            {
                // the function that called this method intentionally disabled the image
                if (forceDisable)
                {
                    Active = false;
                    return;
                }

                // Check the image's individual Active State
                if (!Enabled)
                {
                    Active = false;
                    return;
                }

                // Check Active State of Image Folders
                if (!ImageFolders[PathFolder])
                { 
                    Active = false;
                    return;
                }

                List<string> categoriesToRemove = new List<string>();
                // Check Active State of Tags
                foreach (string category in Tags.Keys)
                {
                    CategoryData categoryData = TaggingInfo.GetCategory(category);
                    if (Tags[category].Count != 0)
                    {
                        if (categoryData.Enabled)
                        {
                            // the category is enabled, however individual tags within that category may be disabled
                            foreach (string tag in Tags[category])
                            {
                                if (!categoryData.GetTag(tag).Enabled)
                                {
                                    Active = false;
                                    return;
                                }
                            }
                        }
                        else // the category itself is not enabled so all tags here are disabled
                        {
                            Active = false;
                            return;
                        }
                    }
                    else // this image no longer has any tags for this category, remove it
                    {
                        categoriesToRemove.Add(category);
                    }
                }

                foreach (string category in categoriesToRemove) // this won't be removed until the image gets enabled at some point, but that shouldn't cause an issue
                {
                    Tags.Remove(category);
                }

                Active = true; // if the program manages to get through the entire evaluation, then the image is active
            }
        }
    }
}
