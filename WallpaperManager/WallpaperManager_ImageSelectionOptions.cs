using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools.FormUtil;
using Microsoft.VisualBasic;
using WallpaperManager.ApplicationData;
using WallpaperManager.ImageSelector;
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
                    RebuildImageSelector(dialog.FileNames);
                }
            }
        }

        private void buttonNameImage_Click(object sender, EventArgs e)
        {
            if (selectedImages != null)
            {
                switch (WallpaperManagerTools.ChooseSelectionType())
                {
                    case SelectionType.Active:
                        if (InspectedImage != "")
                        {
                            if (WallpaperData.ContainsImage(InspectedImage))
                            {
                                PathData.RenameImage(WallpaperData.GetImageData(InspectedImage));
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
                            HashSet<WallpaperData.ImageData> imageDatas = new HashSet<WallpaperData.ImageData>();
                            foreach (string image in selectedImages)
                            {
                                imageDatas.Add(WallpaperData.GetImageData(image));
                            }

                            PathData.RenameImages(imageDatas.ToArray());
                        }
                        break;
                }
            }
            else
            {
                MessageBox.Show("There are no images selected to name");
            }
        }

        private void buttonMoveImage_Click(object sender, EventArgs e)
        {
            if (selectedImages != null)
            {
                switch (WallpaperManagerTools.ChooseSelectionType())
                {
                    case SelectionType.Active:
                        MessageBox.Show("Selected");
                        break;

                    case SelectionType.All:
                        MessageBox.Show("All");
                        break;
                }
            }
            else
            {
                MessageBox.Show("There are no images selected to move");
            }
        }

        private void buttonDeleteImage_Click(object sender, EventArgs e)
        {
            if (selectedImages != null)
            {
                switch (WallpaperManagerTools.ChooseSelectionType())
                {
                    case SelectionType.Active:
                        MessageBox.Show("Selected");
                        break;

                    case SelectionType.All:
                        MessageBox.Show("All");
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
    }
}
