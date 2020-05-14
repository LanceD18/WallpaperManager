using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools;
using Microsoft.VisualBasic;
using Microsoft.WindowsAPICodePack.Dialogs;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.ImageSelector
{
    public partial class ImageSelectorOptionsForm : Form
    {
        public ImageSelectorOptionsForm()
        {
            InitializeComponent();
            checkBoxRandomize.Checked = WallpaperData.RandomizeSelection;

            buttonSelectUnrankedImages.Click += buttonClickClose;
            buttonSelectRankedImages.Click += buttonClickClose;
            buttonSelectUnrankedinFolder.Click += buttonClickClose;
            buttonSelectRankedInFolder.Click += buttonClickClose;
            buttonSelectActiveImages.Click += buttonClickClose;
            buttonSelectImagesOfRank.Click += buttonClickClose;
        }

        private void buttonClickClose(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSelectUnrankedImages_Click(object sender, EventArgs e)
        {
            string[] imagesSelected = checkBoxRandomize.Checked ? WallpaperData.GetImagesOfRank(0).Randomize().ToArray() : WallpaperData.GetImagesOfRank(0);
            WallpaperData.WallpaperManagerForm.RebuildImageSelector(imagesSelected);
        }

        private void buttonSelectRankedImages_Click(object sender, EventArgs e)
        {
            string[] imagesSelected = checkBoxRandomize.Checked ? WallpaperData.GetAllRankedImages().Randomize().ToArray() : WallpaperData.GetAllRankedImages();
            WallpaperData.WallpaperManagerForm.RebuildImageSelector(imagesSelected);
        }

        private void buttonSelectUnrankedinFolder_Click(object sender, EventArgs e)
        {
            using(CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    List<string> imagesToSelect = new List<string>();
                    foreach (string imagePath in WallpaperData.GetImagesOfFolder(dialog.FileName))
                    {
                        if (WallpaperData.GetImageRank(imagePath) == 0)
                        {
                            imagesToSelect.Add(imagePath);
                        }
                    }

                    string[] imagesSelected = checkBoxRandomize.Checked ? imagesToSelect.Randomize().ToArray() : imagesToSelect.ToArray();
                    WallpaperData.WallpaperManagerForm.RebuildImageSelector(imagesSelected);
                }
            }
        }

        private void buttonSelectRankedInFolder_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    List<string> imagesToSelect = new List<string>();
                    foreach (string imagePath in WallpaperData.GetImagesOfFolder(dialog.FileName))
                    {
                        if (WallpaperData.GetImageRank(imagePath) != 0)
                        {
                            imagesToSelect.Add(imagePath);
                        }
                    }

                    string[] imagesSelected = checkBoxRandomize.Checked ? imagesToSelect.Randomize().ToArray() : imagesToSelect.ToArray();
                    WallpaperData.WallpaperManagerForm.RebuildImageSelector(imagesSelected);
                }
            }
        }

        private void buttonSelectActiveImages_Click(object sender, EventArgs e)
        {
            string[] imagesSelected = checkBoxRandomize.Checked ? PathData.ActiveWallpapers.Randomize().ToArray() : PathData.ActiveWallpapers;
            WallpaperData.WallpaperManagerForm.RebuildImageSelector(imagesSelected);
        }

        private void buttonSelectImagesOfRank_Click(object sender, EventArgs e)
        {
            SelectAllImagesOfRank();
        }

        private void SelectAllImagesOfRank()
        {
            int selectedRank;

            try
            {
                selectedRank = int.Parse(Interaction.InputBox("Enter a rank to select: ", "Select Images of Rank"));
            }
            catch // no response
            {
                return;
            }

            if (WallpaperData.ContainsRank(selectedRank))
            {
                string[] imagesToSelect = WallpaperData.GetImagesOfRank(selectedRank);
                string[] imagesSelected = checkBoxRandomize.Checked ? imagesToSelect.Randomize().ToArray() : imagesToSelect;
                WallpaperData.WallpaperManagerForm.RebuildImageSelector(imagesSelected);
            }
            else
            {
                MessageBox.Show("Invalid Rank");
                SelectAllImagesOfRank();
                return;
            }
        }

        private void checkBoxRandomize_CheckedChanged(object sender, EventArgs e)
        {
            WallpaperData.RandomizeSelection = checkBoxRandomize.Checked;
        }
    }
}
