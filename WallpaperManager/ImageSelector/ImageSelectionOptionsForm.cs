using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
using WallpaperManager.Pathing;

namespace WallpaperManager.ImageSelector
{
    public partial class ImageSelectionOptionsForm : Form
    {
        //? Note, the option for selecting images by file explorer is *not* here, this is a sub-page for more categorized selections
        public ImageSelectionOptionsForm()
        {
            InitializeComponent();

            radioButtonAll.Select();
            checkBoxRandomize.Checked = WallpaperData.RandomizeSelection;
        }

        private void checkBoxRandomize_CheckedChanged(object sender, EventArgs e)
        {
            WallpaperData.RandomizeSelection = checkBoxRandomize.Checked;
        }

        private void RebuildImageSelector_CheckRandomizer(string[] imagesToSelect)
        {
            if (checkBoxRandomize.Checked)
            {
                WallpaperData.WallpaperManagerForm.RebuildImageSelector(imagesToSelect.Randomize().ToArray(), false);
            }
            else
            {
                WallpaperData.WallpaperManagerForm.RebuildImageSelector(imagesToSelect, true);
            }
            Close(); // if the program got to this point then the selection was complete so close the selection window
        }

        private void buttonSelectActiveImages_Click(object sender, EventArgs e)
        {
            List<string> imagesToSelect = new List<string>();
            for (int i = 0; i < WallpaperPathing.ActiveWallpapers.Length; i++)
            {
                if (File.Exists(WallpaperPathing.ActiveWallpapers[i])) // depending on the setup, some wallpapers can be activated before others are put in place
                {
                    imagesToSelect.Add(WallpaperPathing.ActiveWallpapers[i]);
                }
            }

            RebuildImageSelector_CheckRandomizer(imagesToSelect.ToArray());
        }

        private void buttonSelectImagesOfRank_Click(object sender, EventArgs e) => SelectAllImagesOfRank();

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
                RebuildImageSelector_CheckRandomizer(imagesToSelect);
            }
            else
            {
                MessageBox.Show("Invalid Rank");
                SelectAllImagesOfRank();
                return; //? keep this here for just in case code is added below this segment in the future
            }
        }

        private void buttonSelectImagesOfType_Click(object sender, EventArgs e)
        {
            string[] imagesToSelect = null;
            Button staticImage = new Button() {Text = "Static"};
            staticImage.Click += (obj, args) => { imagesToSelect = SelectAllImagesOfType(ImageType.Static); };

            Button gifImage = new Button() { Text = "GIF" };
            gifImage.Click += (obj, args) => { imagesToSelect = SelectAllImagesOfType(ImageType.GIF); };

            Button videoImage = new Button() { Text = "Video" };
            videoImage.Click += (obj, args) => { imagesToSelect = SelectAllImagesOfType(ImageType.Video); };

            MessageBoxDynamic.Show("Type selector", "Select a type", new Button[] {staticImage, gifImage, videoImage}, true);

            if (imagesToSelect == null) return; // process was cancelled
            RebuildImageSelector_CheckRandomizer(FilterImages(imagesToSelect));
        }

        private string[] SelectAllImagesOfType(ImageType imageType)
        {
            switch (imageType)
            {
                case ImageType.Static:
                    return WallpaperData.GetAllImagesOfType(ImageType.Static);

                case ImageType.GIF:
                    return WallpaperData.GetAllImagesOfType(ImageType.GIF);

                case ImageType.Video:
                    return WallpaperData.GetAllImagesOfType(ImageType.Video);

                default:
                    return null;
            }
        }

        private void buttonSelectImages_Click(object sender, EventArgs e)
        {
            if (radioButtonAll.Checked) // All
            {
                RebuildImageSelector_CheckRandomizer(WallpaperData.GetAllImages());
            }
            else if (radioButtonUnranked.Checked) // Unranked
            {
                RebuildImageSelector_CheckRandomizer(WallpaperData.GetImagesOfRank(0));
            }
            else if (radioButtonRanked.Checked) // Ranked
            {
                RebuildImageSelector_CheckRandomizer(WallpaperData.GetAllRankedImages());
            }
        }

        private void buttonSelectImagesInFolder_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    RebuildImageSelector_CheckRandomizer(FilterImages(WallpaperData.GetImagesOfFolder(dialog.FileName)));
                }
            }
        }

        // Filters a subset of images based on the active radio button (Not needed if your input is not already filtered in some way, there are functions for that)
        private string[] FilterImages(string[] imagesToFilter)
        {
            List<string> imagesToSelect = new List<string>();
            if (radioButtonAll.Checked) // All
            {
                foreach (string imagePath in imagesToFilter)
                {
                    imagesToSelect.Add(imagePath);
                }
            }
            else if (radioButtonUnranked.Checked) // Unranked
            {
                foreach (string imagePath in imagesToFilter)
                {
                    if (WallpaperData.GetImageRank(imagePath) == 0)
                    {
                        imagesToSelect.Add(imagePath);
                    }
                }
            }
            else if (radioButtonRanked.Checked) // Ranked
            {
                foreach (string imagePath in imagesToFilter)
                {
                    if (WallpaperData.GetImageRank(imagePath) != 0)
                    {
                        imagesToSelect.Add(imagePath);
                    }
                }
            }

            return imagesToSelect.ToArray();
        }
    }
}
