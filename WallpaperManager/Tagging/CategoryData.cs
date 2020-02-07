using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public class CategoryData
    {
        private string name;
        public string Name
        {
            get => name;

            set
            {
                if (Tags != null)
                {
                    HashSet<string> alteredImages = new HashSet<string>();

                    foreach (TagData tag in Tags)
                    {
                        tag.ParentCategoryName = value;

                        foreach (string image in tag.GetLinkedImages())
                        {
                            if (!alteredImages.Contains(image))
                            {
                                WallpaperData.GetImageData(image).RenameCategory(name, value);
                                alteredImages.Add(image);
                            }
                        }
                    }
                }

                name = value;
            }
        }

        public bool Enabled;
        public bool UseForNaming;

        public HashSet<TagData> Tags;

        [JsonIgnore]
        public bool Initialized { get; private set; }

        public CategoryData(string name, HashSet<TagData> tags = null, bool enabled = true, bool useForNaming = true)
        {
            Name = name;
            Enabled = enabled;
            UseForNaming = useForNaming;
            Tags = tags ?? new HashSet<TagData>();
            Initialized = false;
        }

        public void Initialize(bool initializeImages = true)
        {
            Initialized = true;
            WallpaperData.TaggingInfo.AddCategory(this);

            foreach (TagData tag in Tags)
            {
                tag.Initialize(name, initializeImages);
            }
        }

        public bool ContainsTag(TagData tag)
        {
            return ContainsTag(tag.Name);
        }

        public bool ContainsTag(string tagName)
        {
            foreach (TagData curTag in Tags)
            {
                if (tagName == curTag.Name)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool operator ==(CategoryData category1, CategoryData category2)
        {
            return category1?.Name == category2?.Name;
        }

        public static bool operator !=(CategoryData category1, CategoryData category2)
        {
            return category1?.Name != category2?.Name;
        }
    }
}
