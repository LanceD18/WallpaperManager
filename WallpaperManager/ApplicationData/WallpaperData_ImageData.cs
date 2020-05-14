using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.Tagging;

namespace WallpaperManager.ApplicationData
{
    public static partial class WallpaperData
    {
        public class ImageData
        {
            [DataMember(Name = "path")]
            public string Path { get; private set; } //? If you need to change this, FileData's field must be changed into List<ImageData>

            [DataMember(Name = "rank")]
            private int rank;
            public int Rank
            {
                get => rank;

                set
                {
                    if (RankData.ContainsKey(value))
                    {
                        if (Active) // Rank Data does not include inactive images
                        {
                            Debug.WriteLine("Adjusting Rank: " + Rank + " => " + value);
                            RankData[Rank].Remove(Path);
                            RankData[value].Add(Path);
                        }

                        rank = value; // place this after the above if statement to ensure that the right image file path is found
                    }
                    else
                    {
                        Debug.WriteLine("Error: Attempted to assign an invalid rank");
                    }
                }
            }

            [DataMember(Name = "active")]
            private bool active;
            public bool Active
            {
                get => active;

                set
                {
                    if (active != value) // Prevents element duplication whenever active is set to the same value
                    {
                        active = value;

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

            [DataMember(Name = "Tags")] public Dictionary<string, HashSet<string>> Tags; // this should stay as a string for saving to JSON

            [DataMember(Name = "Tag Naming Exceptions")] public HashSet<Tuple<string, string>> TagNamingExceptions; // these tags be used for naming regardless of constraints

            public ImageData(string path, int rank, bool active, Dictionary<string, HashSet<string>> tags = null, HashSet<Tuple<string, string>> tagNamingExceptions = null)
            {
                Path = path;
                Rank = rank;
                Active = active;
                Tags = tags ?? new Dictionary<string, HashSet<string>>();
                TagNamingExceptions = tagNamingExceptions ?? new HashSet<Tuple<string, string>>();
            }

            public void UpdatePath(string newPath)
            {
                Path = newPath;
            }

            public void AddTag(CategoryData category, string tag)
            {
                AddTag(TaggingInfo.GetTag(category, tag));
            }

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
        }
    }
}
