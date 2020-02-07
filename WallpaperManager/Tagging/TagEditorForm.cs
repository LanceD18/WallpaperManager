using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools;
using LanceTools.FormUtil;
using Microsoft.VisualBasic;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public partial class TagEditorForm : Form
    {
        private TagData activeTag;
        private Button activeTagButton;

        private FlowLayoutPanel tagContainerFLP;

        public TagEditorForm(TagData activeTag, Button activeTagButton, FlowLayoutPanel tagContainerFLP)
        {
            InitializeComponent();

            this.activeTag = activeTag;
            this.activeTagButton = activeTagButton;
            this.tagContainerFLP = tagContainerFLP;

            labelTagName.Text = activeTag.Name;

            if (labelTagName.Bounds.Right >= Size.Width - 25)
            {
                Size = new Size(labelTagName.Bounds.Right + 25, Size.Height);
            }

            labelFoundImages.Text = "Found in " + activeTag.GetLinkedImageCount() + " images";
            checkBoxEnabled.Checked = activeTag.Enabled;
            checkBoxUseForNaming.Checked = activeTag.UseForNaming;
        }

        private void UpdateTagButton()
        {
            labelFoundImages.Text = "Found in " + activeTag.GetLinkedImageCount() + " images";
            TaggingTools.UpdateTagButton(activeTagButton, activeTag);
        }

        // Add To Selected Images
        private void buttonAddToSelectedImages_Click(object sender, EventArgs e)
        {
            switch (WallpaperManagerTools.ChooseSelectionType())
            {
                case SelectionType.Active:
                    AddTagToImage(WallpaperData.WallpaperManagerForm.GetActiveImage());
                    break;

                case SelectionType.All:
                    AddTagToImages(WallpaperData.WallpaperManagerForm.GetSelectedImages());
                    break;
            }

            UpdateTagButton();
        }

        private void AddTagToImage(string targetImage)
        {
            if (WallpaperData.ContainsImage(targetImage))
            {
                WallpaperData.GetImageData(targetImage).AddTag(WallpaperData.TaggingInfo.GetTagParentCategory(activeTag), activeTag.Name);
            }
            else
            {
                MessageBox.Show("The image you attempted to tag is not included");
            }

        }

        private void AddTagToImages(string[] targetImages)
        {
            if (targetImages.Length > 0)
            {
                string unincludedImages = "The following images you attempted to tag are not included:";
                foreach (string image in targetImages)
                {
                    if (WallpaperData.ContainsImage(image))
                    {
                        WallpaperData.GetImageData(image).AddTag(WallpaperData.TaggingInfo.GetTagParentCategory(activeTag), activeTag.Name);
                    }
                    else
                    {
                        unincludedImages += "\n" + image;
                    }
                }

                if (unincludedImages.Contains("\n"))
                {
                    MessageBox.Show(unincludedImages);
                }
            }
            else
            {
                MessageBox.Show("There are no images to tag");
            }
        }

        // Remove From Selected Images
        private void buttomRemoveFromSelectedImages_Click(object sender, EventArgs e)
        {
            switch (WallpaperManagerTools.ChooseSelectionType())
            {
                case SelectionType.Active:
                    RemoveTagFromImage(WallpaperData.WallpaperManagerForm.GetActiveImage(), sender as Button);
                    break;

                case SelectionType.All:
                    RemoveTagFromImages(WallpaperData.WallpaperManagerForm.GetSelectedImages(), sender as Button);
                    break;
            }

            UpdateTagButton();
        }

        private void RemoveTagFromImage(string targetImage, Button tagButton)
        {
            if (MessageBox.Show("Remove tag from active image?", "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (WallpaperData.ContainsImage(targetImage))
                {
                    WallpaperData.GetImageData(targetImage).RemoveTag(activeTag);
                }
                else
                {
                    MessageBox.Show("The image you attempted to remove a tag from is not included");
                }
            }
        }

        private void RemoveTagFromImages(string[] targetImages, Button tagButton)
        {
            if (MessageBox.Show("Remove tag from all selected images?", "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (targetImages.Length > 0)
                {
                    string unincludedImages = "The following images you attempted to remove a tag from are not included:";
                    foreach (string image in targetImages)
                    {
                        if (WallpaperData.ContainsImage(image))
                        {
                            WallpaperData.GetImageData(image).RemoveTag(activeTag);
                        }
                        else
                        {
                            unincludedImages += "\n" + image;
                        }
                    }

                    if (unincludedImages.Contains("\n"))
                    {
                        MessageBox.Show(unincludedImages);
                    }
                }
                else
                {
                    MessageBox.Show("There are no images selected to remove this tag from");
                }
            }
        }

        // Select Images With Tag
        private void buttonSelectImagesWithTag_Click(object sender, EventArgs e)
        {
            DialogResult randomize = MessageBox.Show("Randomize selection?", "Choose an option", MessageBoxButtons.YesNoCancel);

            if (randomize != DialogResult.Cancel)
            {
                string[] selectedImages = randomize == DialogResult.Yes ? activeTag.GetLinkedImages().Randomize().ToArray() : activeTag.GetLinkedImages();
                WallpaperData.WallpaperManagerForm.RebuildImageSelector(selectedImages);
            }
        }

        // Delete Tag
        private void buttonDeleteTag_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the tag " + activeTag.Name + "?", "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TagContainer parentTagContainer = tagContainerFLP.Parent as TagContainer;
                parentTagContainer.RemoveTagFromActiveCategory(activeTagButton);
                Close();
            }
        }

        // Enabled
        private void checkBoxEnabled_CheckedChanged(object sender, EventArgs e)
        {
            activeTag.Enabled = checkBoxEnabled.Checked;
            activeTagButton.ForeColor = activeTag.Enabled ? Color.Black: Color.Red;
        }

        // Use For Naming
        private void checkBoxUseForNaming_CheckedChanged(object sender, EventArgs e)
        {
            activeTag.UseForNaming = checkBoxUseForNaming.Checked;
        }

        // Rename Tag
        private void buttonRenameTag_Click(object sender, EventArgs e)
        {
            string tagName = Interaction.InputBox("Enter a new name for the " + activeTag.Name + " tag", "Rename Tag", "", -1, -1);

            if (tagName != "" && tagName != activeTag.Name)
            {
                if (!WallpaperData.TaggingInfo.GetCategory(activeTag.ParentCategoryName).ContainsTag(tagName))
                {
                    activeTag.SetName(tagName);
                    labelTagName.Text = tagName;
                    TaggingTools.UpdateTagButton(activeTagButton, activeTag); //! must be place lower than the call to activeTag.SetName()
                }
                else
                {
                    MessageBox.Show(tagName + " already exists");
                }
            }
        }

        private void buttonLink_Click(object sender, EventArgs e)
        {
            CategoryData activeCategory = WallpaperData.TaggingInfo.GetCategory(activeTag.ParentCategoryName);
            using (TagClickerForm f = new TagClickerForm(WallpaperData.TaggingInfo.GetCategoryIndex(activeCategory), TagFormStyle.Linker, null, activeTag))
            {
                f.ShowDialog();
            }
        }

        private void buttonParentTags_Click(object sender, EventArgs e)
        {
            using (TagLinkViewerForm f = new TagLinkViewerForm(activeTag, LinkType.Parent))
            {
                f.ShowDialog();
            }
        }

        private void buttonChildTags_Click(object sender, EventArgs e)
        {
            using (TagLinkViewerForm f = new TagLinkViewerForm(activeTag, LinkType.Child))
            {
                f.ShowDialog();
            }
        }
    }
}
