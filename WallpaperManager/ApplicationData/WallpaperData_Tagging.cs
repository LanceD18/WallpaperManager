using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.Tagging;

namespace WallpaperManager.ApplicationData
{
    public static partial class WallpaperData
    {
        //private static Dictionary<string, CategoryData> TagData = new Dictionary<string, CategoryData>();
        public static TaggingInfo TaggingInfo = new TaggingInfo();
        public static string TagSortOption = "Count_Decreasing";

        public static class Tagging
        {
            public static void LinkImageTags(ImageData taggedImage)
            {
                foreach (string category in taggedImage.Tags.Keys)
                {
                    foreach (string tag in taggedImage.Tags[category])
                    {
                        TaggingInfo.GetTag(category, tag).LinkImage(taggedImage);
                    }
                }
            }

            public static void UnlinkImageTags(ImageData taggedImage)
            {
                foreach (string category in taggedImage.Tags.Keys)
                {
                    foreach (string tag in taggedImage.Tags[category])
                    {
                        TaggingInfo.GetTag(category, tag).UnlinkImage(taggedImage);
                    }
                }
            }

            public static void UpdateTaggingInfo(TaggingInfo updatedTaggingInfo)
            {
                TaggingInfo = updatedTaggingInfo;
            }

            public static TaggingInfo GetTaggingInfo()
            {
                return TaggingInfo;
            }
        }
    }
}
