using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public enum TagFormStyle
    {
        Editor,
        Adder,
        Linker
    }

    public static class TaggingTools
    {
        public static TagTabControl ActiveTagTabControl;

        public static string GetButtonTagName(Button tagButton)
        {
            return tagButton.Text.Substring(0, tagButton.Text.IndexOf('(') - 1);
        }

        public static string GetButtonCategoryName(Button tagButton)
        {
            int startIndex = tagButton.Text.IndexOf('(') + 1;
            int endIndex = tagButton.Text.IndexOf(')');

            return tagButton.Text.Substring(startIndex, endIndex - startIndex);
        }

        public static void UpdateTagButton(Button tagButton, TagData tag, WallpaperData.ImageData activeImage = null)
        {
            if (activeImage != null && activeImage.ContainsTag(tag))
            {
                tagButton.BackColor = Color.LightGreen;
            }
            else
            {
                tagButton.BackColor = SystemColors.ButtonFace;
            }

            tagButton.Text = tag.Name + " (" + tag.GetLinkedImageCount() + ")";
            tagButton.ForeColor = tag.Enabled ? Color.Black : Color.Red;
        }

        public static Button GetTagButton(TagData tag)
        {
            CategoryData tagCategory = WallpaperData.TaggingInfo.GetCategory(tag.ParentCategoryName);
            return GetCategoryTagContainer(tagCategory, ActiveTagTabControl)?.GetTagButton(tag);
        }

        public static TagContainer GetCategoryTagContainer(CategoryData category, TagTabControl tagTabControl)
        {
            TagContainer categoryTagContainer = null;
            foreach (TabPage tabPage in tagTabControl.TabPages)
            {
                if (tabPage.Text == category.Name)
                {
                    if (tabPage.Controls.Count > 0) // otherwise, the TagContainer does not yet exist and needs to be created
                    {
                        categoryTagContainer = tabPage.Controls[0] as TagContainer;
                    }

                    break;
                }
            }

            return categoryTagContainer;
        }
    }
}

