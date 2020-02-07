using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public partial class TagForm : Form
    {
        public void AddTag(TagData tag)
        {
            CategoryData tagParentCategory = WallpaperData.TaggingInfo.GetTagParentCategory(tag);

            if (TaggingTools.GetCategoryTagContainer(tagParentCategory, TabControlImageTagger) != null)
            {
                //xreturn // This used to return the below line of code
                TaggingTools.GetCategoryTagContainer(tagParentCategory, TabControlImageTagger).InsertTag(tag);
            }

            UpdateCategoryControls(); // the above return calls this method inside of it

            //xreturn true; // the tag adds itself and since the TagContainer has not yet been created it'll be loaded when this occurs
        }

        public void RemoveTag(Button tagButton)
        {
            TaggingTools.GetCategoryTagContainer(ActiveCategory, TabControlImageTagger).RemoveTagFromActiveCategory(tagButton);
        }

        public void MoveTag(string tagName, string categoryName)
        {
            TagData origTag = WallpaperData.TaggingInfo.GetTag(ActiveCategory.Name, tagName);
            TagData newTag = new TagData(origTag);
            //! Do NOT move the call to remove tag up here as that prevents you from keep the tag in the event of not wanting to remove it

            if (!WallpaperData.TaggingInfo.GetCategory(categoryName).ContainsTag(newTag))
            {
                RemoveTag(TabControlImageTagger.selectedButton);
                newTag.Initialize(categoryName); //! this must be done after the call to RemoveTag above | Also ensure that this only happens when you've confirmed that the tag is new!
                AddTag(newTag);
            }
            else if (MessageBox.Show("The tag " + tagName + " already exists in the " + categoryName + " category. " +
                                     "Would you like to merge these two tags instead?",
                         "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RemoveTag(TabControlImageTagger.selectedButton);
                TagData dupTag = WallpaperData.TaggingInfo.GetTag(categoryName, tagName);
                newTag.CopyLinkedImagesToTag(dupTag);

                TaggingTools.GetCategoryTagContainer(WallpaperData.TaggingInfo.GetCategory(categoryName), TabControlImageTagger)?.UpdateTagButtonImageCount(dupTag);
                //xRemoveTag(TabControlImageTagger.selectedButton);
            }

            UpdateCategoryControls();
        }
    }
}
