using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using WallpaperManager.Tagging;

namespace WallpaperManager.ApplicationData
{
    public static partial class WallpaperData
    {
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
                            RankData[Rank].Remove(Path);
                            RankData[value].Add(Path);
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
                            RankData[Rank].Add(Path);
                            ActiveImages.Add(Path);
                        }
                        else  // Note that Rank Data does not include inactive images
                        {
                            RankData[Rank].Remove(Path);
                            ActiveImages.Remove(Path);
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

            public ImageData(string path, int rank, bool active, Dictionary<string, HashSet<string>> tags = null, HashSet<Tuple<string, string>> tagNamingExceptions = null)
            {
                Path = path;
                PathFolder = new FileInfo(path).Directory.FullName;
                Rank = rank;
                Active = active;
                Tags = tags ?? new Dictionary<string, HashSet<string>>();
                TagNamingExceptions = tagNamingExceptions ?? new HashSet<Tuple<string, string>>();
                //! NOTE that EvaluateActiveState should be called on load through ImageFolder's initialization, effectively initializing every image
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
                    Debug.WriteLine("Forcefully Disabled: " + Path);
                    Active = false;
                    return;
                }

                // Check the image's individual Active State
                if (!Enabled)
                {
                    Debug.WriteLine("Disabled by Image: " + Path);
                    Active = false;
                    return;
                }

                // Check Active State of Image Folders
                if (!ImageFolders[PathFolder])
                { 
                    Debug.WriteLine("Disabled by Image Folder: " + Path);
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
                                    Debug.WriteLine("Disabled by Tag: " + Path);
                                    Active = false;
                                    return;
                                }
                            }
                        }
                        else // the category itself is not enabled so all tags here are disabled
                        {
                            Debug.WriteLine("Disabled by Category: " + Path);
                            Active = false;
                            return;
                        }
                    }
                    else // this image no longer has any tags for this category, remove it
                    {
                        categoriesToRemove.Add(category);
                        continue;
                    }
                }

                foreach (string category in categoriesToRemove)
                {
                    Debug.WriteLine("Removing the Category: " + category);
                    Tags.Remove(category);
                }

                //Debug.WriteLine("Enabled: " + Path);
                Active = true; // if the program manages to get through the entire evaluation, then the image is active
            }
        }
    }
}
