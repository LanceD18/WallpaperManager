using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        //! No longer needed currently, consider removing this in the future
        private static Dictionary<ImageType, List<string>> ActiveImagesOfType;

        public static string[] GetAllImagesOfType(ImageType imageType) => ImagesOfType[imageType].Keys.ToArray();

        public static bool IsAllImagesOfTypeUnranked(ImageType imageType) => ImagesOfTypeRankData[imageType][0].Count == ImagesOfType[imageType].Count;

        public struct VideoSettings
        {
            public int volume;
            public double playbackSpeed;

            public VideoSettings(int volume, double playbackSpeed)
            {
                this.volume = volume;
                this.playbackSpeed = playbackSpeed;
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

            [DataMember(Name = "Video Settings")] public VideoSettings VideoSettings = new VideoSettings(50, 1); // only applicable to images with the corresponding image type

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
                    if (!WallpaperManagerTools.IsSupportedVideoType(file.Extension))
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
                Path = newPath;
                PathFolder = new FileInfo(Path).Directory.FullName;
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

                        Debug.WriteLine("Tag Count: " + Tags[categoryName].Count);
                        Debug.WriteLine("Category Count: " + Tags.Count);


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
                Dictionary<string, HashSet<string>> tempTags = new Dictionary<string, HashSet<string>>();

                foreach (string category in Tags.Keys)
                {
                    tempTags.Add(category == oldName ? newName : category, Tags[oldName]);
                }

                Tags = tempTags;
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

            public string GetTaggedName()
            {
                string taggedName = "";

                foreach (string category in Tags.Keys)
                {
                    //List<string> tagsOrderedByCount = Tags[category].OrderBy(t => TaggingInfo.GetTag(category, t).GetLinkedImageCount()).ToList();
                    List<string> alphabeticTags = Tags[category].OrderBy(t => t).ToList();

                    foreach (TagData tag in GetTags())
                    {
                        if (!tag.UseForNaming)
                        {
                            alphabeticTags.Remove(tag.Name);
                            continue;
                        }

                        for (int i = alphabeticTags.Count - 1; i >= 0; i--)
                        {
                            Tuple<string, string> tagInfo = new Tuple<string, string>(category, alphabeticTags[i]);
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

                return taggedName;
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
