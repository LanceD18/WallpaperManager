using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools;
using Microsoft.VisualBasic;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public partial class TagForm : Form
    {
        private void buttonAddCategory_Click(object sender, EventArgs e)
        {
            string categoryName = Interaction.InputBox("Enter the name of your category", "Add Category", "");

            if (categoryName != "")
            {
                if (!TabControlImageTagger.CreateCategory(categoryName))
                {
                    MessageBox.Show(categoryName + " already exists");
                }
            }
        }

        private void buttonRemoveCategory_Click(object sender, EventArgs e)
        {
            string categoryName = Interaction.InputBox("Enter the name of the cateogory you'd like to remove", "Remove Category", "");

            if (categoryName != "")
            {
                TabControlImageTagger.RemoveCategory(categoryName);
            }
        }

        private void buttonRenameCategory_Click(object sender, EventArgs e)
        {
            string origName = ActiveCategory.Name;
            string newName = Interaction.InputBox("Enter a new name for the " + ActiveCategory.Name + " category", "Rename Category", ActiveCategory.Name);

            if (newName != "")
            {
                foreach (CategoryData category in WallpaperData.TaggingInfo.GetAllCategories())
                {
                    if (category.Name == newName)
                    {
                        MessageBox.Show("This name is already in use");
                        return;
                    }
                }

                WallpaperData.TaggingInfo.GetCategory(origName).Name = newName;
                ActiveCategory = WallpaperData.TaggingInfo.GetCategory(newName);

                TabControlImageTagger.SelectedTab.Text = ActiveCategory.Name;
            }
        }

        private void buttonSelectCategoryImages_Click(object sender, EventArgs e)
        {
            DialogResult randomize = MessageBox.Show("Randomize selection?", "Choose an option", MessageBoxButtons.YesNoCancel);

            if (randomize != DialogResult.Cancel)
            {
                HashSet<string> images = new HashSet<string>();

                foreach (TagData tag in ActiveCategory.Tags)
                {
                    foreach (string image in tag.GetLinkedImages())
                    {
                        images.Add(image);
                    }
                }

                string[] selectedImages = randomize == DialogResult.Yes ? images.Randomize().ToArray() : images.ToArray();
                WallpaperData.WallpaperManagerForm.RebuildImageSelector(selectedImages);
            }
        }

        private void checkBoxEnabled_CheckedChanged(object sender, EventArgs e)
        { 
            WallpaperData.TaggingInfo.GetCategory(ActiveCategory.Name).Enabled = checkBoxEnabled.Checked;
            //ActiveCategory = WallpaperData.TaggingInfo.GetCategory(ActiveCategory.Name);
        }

        private void checkBoxUseForNaming_CheckedChanged(object sender, EventArgs e)
        {
            WallpaperData.TaggingInfo.GetCategory(ActiveCategory.Name).UseForNaming = checkBoxUseForNaming.Checked;
            //ActiveCategory = WallpaperData.TaggingInfo.GetCategory(ActiveCategory.Name);
        }

        // Apply Default Settings
        private void buttonApplyDefaultSettings_Click(object sender, EventArgs e)
        {
            bool renameAffectedImages = false;
            HashSet<WallpaperData.ImageData> imagesToRename = new HashSet<WallpaperData.ImageData>();

            // Loop through and update all tags
            foreach (TagData tag in ActiveCategory.Tags)
            {
                // If the UseForNaming property is changed, queue the tag's images for renaming
                if (tag.UseForNaming != ActiveCategory.UseForNaming)
                {
                    renameAffectedImages = true;

                    foreach (string image in tag.GetLinkedImages())
                    {
                        imagesToRename.Add(WallpaperData.GetImageData(image));
                    }
                }

                // Update Tag
                tag.Enabled = ActiveCategory.Enabled;
                tag.UseForNaming = ActiveCategory.UseForNaming;

                // Update Tag Colors
                Button tagButton = TaggingTools.GetCategoryTagContainer(ActiveCategory, TabControlImageTagger).GetTagButton(tag);
                if (tagButton != null)
                {
                    TaggingTools.GetCategoryTagContainer(ActiveCategory, TabControlImageTagger).GetTagButton(tag).ForeColor = tag.Enabled ? Color.Black : Color.Red;
                }
            }

            // Ask the user if they want to rename image's impacted by the UseForNaming change
            if (renameAffectedImages)
            {
                PathData.RenameAffectedImagesPrompt(imagesToRename.ToArray());
            }
        }

        private string namingOrderHelpString = "Tags that are used for naming will be ordered first by the frontmost " +
                                               "\n category and then be ordered within that category alphabetically." +
                                               "\n\n The naming order will then proceed to do the same with" +
                                               "\n each following category." +
                                               "\n\n (You can drag and drop each category to change their order)";

        // Naming Order Help
        private void buttonNamingOrderHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(namingOrderHelpString);
        }

        // Add Tag
        private void buttonAddTag_Click(object sender, EventArgs e)
        {
            string tagName = Interaction.InputBox("Enter the name of the tag you'd like to add to the " + ActiveCategory.Name + " category", "Add Tag", "", -1, -1);

            if (tagName.Contains('(') || tagName.Contains(')'))
            {
                MessageBox.Show("Tags cannot contain parenthesis");
                return;
            }

            char[] nums = "0123456789".ToCharArray();
            if (tagName.IndexOfAny(nums) != -1)
            {
                MessageBox.Show("Tags cannot contain numbers");
                return;
            }

            if (tagName != "")
            {
                TagData newTag = new TagData(tagName, ActiveCategory);
                if (newTag.Initialize())
                {
                    TaggingTools.GetCategoryTagContainer(ActiveCategory, TabControlImageTagger).InsertTag(newTag);
                    UpdateCategoryControls();
                }
                else
                {
                    MessageBox.Show(newTag.Name + " already exists in the " + ActiveCategory.Name + " category");
                }
            }
        }

        // Remove Tag
        private void buttonRemoveTag_Click(object sender, EventArgs e)
        {
            string tagName = Interaction.InputBox("Enter the name of the tag you'd like to remove from the  " + ActiveCategory.Name + " category", "Remove Tag", "", -1, -1);

            TagData tagToDelete = null;
            foreach (TagData tag in ActiveCategory.Tags)
            {
                if (tagName == tag.Name)
                {
                    tagToDelete = tag;
                    break;
                }
            }

            if (tagToDelete != null)
            {
                Button tagButton = TaggingTools.GetCategoryTagContainer(ActiveCategory, TabControlImageTagger).GetTagButton(tagToDelete);
                TaggingTools.GetCategoryTagContainer(ActiveCategory, TabControlImageTagger).RemoveTag(tagToDelete, tagButton);
            }
            else if (tagName != "")
            {
                MessageBox.Show("The tag " + tagName + " does not exist in the " + ActiveCategory.Name + " category");
            }
        }
    }
}
