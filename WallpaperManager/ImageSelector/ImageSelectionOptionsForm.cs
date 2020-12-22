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
using Microsoft.WindowsAPICodePack.Dialogs;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.ImageSelector
{
    public partial class ImageSelectionOptionsForm : Form
    {
        //? Note, the option for selecting images by file explorer is *not* here, this is a sub-page for more categorized selections
        public ImageSelectionOptionsForm()
        {
            InitializeComponent();
            checkBoxRandomize.Checked = WallpaperData.RandomizeSelection;

            buttonSelectUnrankedImages.Click += buttonClickClose;
            buttonSelectRankedImages.Click += buttonClickClose;
            buttonSelectUnrankedinFolder.Click += buttonClickClose;
            buttonSelectRankedInFolder.Click += buttonClickClose;
            buttonSelectActiveImages.Click += buttonClickClose;
            buttonSelectImagesOfRank.Click += buttonClickClose;
            buttonSelectImagesOfType.Click += buttonClickClose;

            buttonSelectImages.Click += buttonClickClose;
            buttonSelectImagesInFolder.Click += buttonClickClose;

            radioButtonAll.Select();
        }

        private void buttonClickClose(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBoxRandomize_CheckedChanged(object sender, EventArgs e)
        {
            WallpaperData.RandomizeSelection = checkBoxRandomize.Checked;
        }

        private void RebuildImageSelector_CheckRandomizer(string[] imagesToSelect)
        {
            if (imagesToSelect != null) // this can occur when the selection is cancelled
            {
                WallpaperData.WallpaperManagerForm.RebuildImageSelector(checkBoxRandomize.Checked ? imagesToSelect.Randomize().ToArray() : imagesToSelect);
            }
        }

        private void buttonSelectUnrankedImages_Click(object sender, EventArgs e) => RebuildImageSelector_CheckRandomizer(WallpaperData.GetImagesOfRank(0));

        private void buttonSelectRankedImages_Click(object sender, EventArgs e) => RebuildImageSelector_CheckRandomizer(WallpaperData.GetAllRankedImages());

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

                    RebuildImageSelector_CheckRandomizer(imagesToSelect.ToArray());
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

                    RebuildImageSelector_CheckRandomizer(imagesToSelect.ToArray());
                }
            }
        }

        private void buttonSelectActiveImages_Click(object sender, EventArgs e) => RebuildImageSelector_CheckRandomizer(PathData.ActiveWallpapers);

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
                return;
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

            RebuildImageSelector_CheckRandomizer(imagesToSelect);
        }

        private string[] SelectAllImagesOfType(ImageType imageType)
        {
            switch (imageType)
            {
                case ImageType.Static:
                    return WallpaperData.GetAllStaticImages();

                case ImageType.GIF:
                    return WallpaperData.GetAllGifImages();

                case ImageType.Video:
                    return WallpaperData.GetAllVideoImages();

                default:
                    return null;
            }
        }

        private void buttonSelectImages_Click(object sender, EventArgs e)
        {
            if (radioButtonAll.Checked) RebuildImageSelector_CheckRandomizer(WallpaperData.GetAllImages());

            if (radioButtonUnranked.Checked) RebuildImageSelector_CheckRandomizer(WallpaperData.GetImagesOfRank(0));

            if (radioButtonRanked.Checked) RebuildImageSelector_CheckRandomizer(WallpaperData.GetAllRankedImages());
        }

        private void buttonSelectImagesInFolder_Click(object sender, EventArgs e)
        {

        }
    }
}
