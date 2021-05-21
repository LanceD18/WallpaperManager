using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools;
using LanceTools.FormUtil;
using Microsoft.VisualBasic;
using Microsoft.WindowsAPICodePack.Dialogs;
using WallpaperManager.ApplicationData;
using WallpaperManager.ImageSelector;
using WallpaperManager.Options;
using WallpaperManager.Pathing;
using WallpaperManager.Tagging;

namespace WallpaperManager
{
    public partial class WallpaperManagerForm : Form
    {
        private void buttonSelect_Click(object sender, EventArgs e)
        {
            Button regularSelectionButton = new Button();
            regularSelectionButton.AutoSize = true;
            regularSelectionButton.Text = "Select by File Exploror";
            regularSelectionButton.Click += (o, i) => { SelectImages(); };

            Button optionalSelectionButton = new Button();
            optionalSelectionButton.AutoSize = true;
            optionalSelectionButton.Text = "Other Selection Options...";
            optionalSelectionButton.Click += (o, i) => { new ImageSelectionOptionsForm().ShowDialog(); };

            MessageBoxDynamic.Show("Choose a selection type", "Choose an option", new Button[] { regularSelectionButton, optionalSelectionButton }, true);
        }

        private void SelectImages()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                //dialog.InitialDirectory = lastSelectedPath;
                dialog.Multiselect = true;
                dialog.Title = "Select an Image";
                dialog.Filter = WallpaperManagerTools.IMAGE_FILES_FILTER;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    RebuildImageSelector(dialog.FileNames, false); // this will be in order by default
                }
            }
        }

        private void buttonNameImage_Click(object sender, EventArgs e)
        {
            if (selectedImages != null)
            {
                HandleImageRenaming(true);
            }
            else
            {
                MessageBox.Show("There are no images selected to rename");
            }
        }

        private void buttonMoveImage_Click(object sender, EventArgs e)
        {
            if (selectedImages != null)
            {
                using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
                {
                    dialog.IsFolderPicker = true;

                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        if (WallpaperData.ThemeContainsFolder(dialog.FileName))
                        {
                            HandleImageRenaming(OptionsData.ThemeOptions.AllowTagBasedRenamingForMovedImages, new DirectoryInfo(dialog.FileName));
                        }
                        else
                        {
                            MessageBox.Show("The selected folder is not included in your theme");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("There are no images selected to move");
            }
        }

        private void HandleImageRenaming(bool allowTagBasedNaming, DirectoryInfo moveDirectory = null)
        {
            List<ImageType> filter = new List<ImageType>();

            if (OptionsData.ThemeOptions.ExcludeRenamingStatic) filter.Add(ImageType.Static);
            if (OptionsData.ThemeOptions.ExcludeRenamingGif) filter.Add(ImageType.GIF);
            if (OptionsData.ThemeOptions.ExcludeRenamingVideo) filter.Add(ImageType.Video);

            switch (WallpaperManagerTools.ChooseSelectionType())
            {
                case SelectionType.Active:
                    if (InspectedImage != "")
                    {
                        if (WallpaperData.ContainsImage(InspectedImage))
                        {
                            int imageIndex = selectedImages.IndexOf(InspectedImage);
                            string[] newName = ImagePathing.RenameImage(InspectedImage, moveDirectory, allowTagBasedNaming);
                            if (newName != null && newName.Length > 0) // this can occur if the given image is not able to be named
                            {
                                selectedImages[imageIndex] = newName[0]; // there will only be one entry in this array, the renamed image
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid image");
                        }
                    }
                    else
                    {
                        MessageBox.Show("There is no image selected");
                    }
                    break;

                case SelectionType.All:
                    if (MessageBox.Show("Are you sure you want to rename ALL " + selectedImages.Length + " selected images?", "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // Doing this will require a rebuild of the image selector to keep it in tact
                        RebuildImageSelector(ImagePathing.RenameImages(selectedImages, moveDirectory, allowTagBasedNaming), false);
                    }
                    break;
            }
        }

        private void buttonDeleteImage_Click(object sender, EventArgs e)
        {
            if (selectedImages != null)
            {
                //! Always ensure that this is emphasized somewhere
                MessageBox.Show("NOTE: This deletes the actual image file! (While also safely removing the image from the application)");
                switch (WallpaperManagerTools.ChooseSelectionType())
                {
                    case SelectionType.Active:
                        MessageBox.Show("Selected [This function is currently in development]");
                        break;

                    case SelectionType.All:
                        MessageBox.Show("All [This function is currently in development]");
                        break;
                }
            }
            else
            {
                MessageBox.Show("There are no images selected to delete");
            }
        }

        private void buttonRankImages_Click(object sender, EventArgs e)
        {
            if (selectedImages != null && selectedImages.Length > 0)
            {
                int newRank;
                try
                {
                    newRank = Int32.Parse(Interaction.InputBox("Enter a new rank for all selected images: ", "Select a new rank"));
                }
                catch // no response
                {
                    return;
                }

                if (newRank >= 0 && newRank <= WallpaperData.GetMaxRank())
                {
                    foreach (string image in selectedImages)
                    {
                        WallpaperData.GetImageData(image).Rank = newRank;
                        UpdateImageRanks();
                    }
                }
                else
                {
                    MessageBox.Show("The selected rank was out of range");
                }
            }
            else
            {
                MessageBox.Show("There are no images selected to rank");
            }
        }

        private void buttonClearSelection_Click(object sender, EventArgs e) => ClearImageSelector();
    }
}
