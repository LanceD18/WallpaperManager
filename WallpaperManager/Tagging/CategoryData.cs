using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

                    WallpaperData.UpdateRankPercentiles();
                }

                name = value;
            }
        }

        private bool _Enabled;
        public bool Enabled
        {
            get => _Enabled;

            set
            {
                _Enabled = value;

                foreach (TagData tag in Tags)
                {
                    WallpaperData.EvaluateImageActiveStates(tag.GetLinkedImages(), !value); // will forceDisable if the value is set to false
                }
            }
        }

        private bool _UseForNaming;
        public bool UseForNaming
        {
            get => _UseForNaming;

            set
            {
                _UseForNaming = value;

                HashSet<WallpaperData.ImageData> imagesToRename = new HashSet<WallpaperData.ImageData>();
                foreach (TagData tag in Tags)
                {
                    foreach (string imagePath in tag.GetLinkedImages())
                    {
                        imagesToRename.Add(WallpaperData.GetImageData(imagePath));
                    }
                }

                PathData.RenameAffectedImages(imagesToRename.ToArray());
            }
        }

        public HashSet<TagData> Tags;

        [JsonIgnore]
        public bool Initialized { get; private set; }

        public CategoryData(string name, HashSet<TagData> tags = null, bool enabled = true, bool useForNaming = true)
        {
            Tags = tags ?? new HashSet<TagData>(); //! must be placed first to avoid getter setter errors (ex: enabled's setter)

            Name = name;
            Enabled = enabled;
            UseForNaming = useForNaming;
            Initialized = false;
        }

        public void Initialize(bool initializeImages)
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
            return GetTag(tagName) != null;
        }

        public TagData GetTag(string tagName)
        {
            foreach (TagData curTag in Tags)
            {
                if (tagName == curTag.Name)
                {
                    return curTag;
                }
            }

            return null;
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
