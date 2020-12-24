using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;
using LanceTools;
using WallpaperManager.ApplicationData;
using WallpaperManager.ImageSelector;

namespace WallpaperManager
{
    public partial class WallpaperManager : Form
    {
        private string InspectedImage;
        private string inspectedImage
        {
            get => InspectedImage;
            set
            {
                InspectedImage = value; 
                labelSelectedImage.Text = value;

                if (File.Exists(value))
                {
                    try
                    {
                        using (Image image = WallpaperManagerTools.GetImageFromFile(value))
                        {
                            labelImageSize.Text = image.Width + "x" + image.Height;
                            labelImageSize.Left = panelImageSelector.Location.X - labelImageSize.Size.Width - 5;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Attempted to load an unsupported file type\n" + e.Message);
                    }
                }
            }
        }

        private void InitializeImageInspector()
        {
            panelImageInspector.Visible = false;
            panelImageInspector.BringToFront();

            SuspendLayout();
            inspector_axWindowsMediaPlayer.stretchToFit = true;
            //inspector_axWindowsMediaPlayer.uiMode = "none";
            inspector_axWindowsMediaPlayer.settings.setMode("loop", true);
            inspector_axWindowsMediaPlayer.settings.autoStart = true;

            inspector_textBoxRankEditor.LostFocus += (o, i) =>
            {
                try
                {
                    WallpaperData.GetImageData(inspectedImage).Rank = int.Parse(inspector_textBoxRankEditor.Text);
                    inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(inspectedImage).ToString();
                }
                catch
                {
                    inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(inspectedImage).ToString();
                }
            };

            inspector_textBoxRankEditor.KeyDown += (o, i) =>
            {
                if (i.KeyCode == Keys.Enter)
                {
                    try
                    {
                        WallpaperData.GetImageData(inspectedImage).Rank = int.Parse(inspector_textBoxRankEditor.Text);
                        inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(inspectedImage).ToString();
                    }
                    catch
                    {
                        inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(inspectedImage).ToString();
                    }

                    i.SuppressKeyPress = true; // prevents windows 'ding' sound
                }
            };

            inspector_buttonMinus.Click += (o, i) => { WallpaperData.GetImageData(inspectedImage).Rank--; inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(inspectedImage).ToString(); };
            inspector_buttonPlus.Click += (o, i) => { WallpaperData.GetImageData(inspectedImage).Rank++; inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(inspectedImage).ToString(); };

            inspector_buttonLeft.Click += (o, i) =>
            {
                int leftIndex = selectedImages.IndexOf(inspectedImage) - 1;

                if (leftIndex >= 0)
                {
                    inspectedImage = selectedImages[leftIndex];
                    ActivateImageInspector(); // more like updating image inspector
                }
            };

            inspector_buttonRight.Click += (o, i) =>
            {
                int rightIndex = selectedImages.IndexOf(inspectedImage) + 1;

                if (rightIndex < selectedImages.Length)
                {
                    inspectedImage = selectedImages[rightIndex];
                    ActivateImageInspector(); // more like updating image inspector
                }
            };

            ResumeLayout();
        }

        private void buttonInspectImage_Click(object sender, EventArgs e)
        {
            if (selectedImages != null)
            {
                if (!panelImageInspector.Visible)
                {
                    if (selectedImages.Contains(inspectedImage))
                    {
                        ActivateImageInspector();
                    }
                    else
                    {
                        MessageBox.Show("Make this button gray out or disappear if no image is selected");
                    }
                }
                else
                {
                    DeactivateImageInspector();
                }
            }
        }

        // also used for updating the image inspector
        private void ActivateImageInspector()
        {
            panelImageInspector.SuspendLayout();
            buttonInspectImage.Text = "Close Inspector";

            if (!WallpaperData.GetAllVideoImages().Contains(inspectedImage)) // display image
            {
                inspector_axWindowsMediaPlayer.Visible = false;

                inspector_pictureBoxImage.Visible = true;
                inspector_pictureBoxImage.ImageLocation = inspectedImage;
            }
            else // display video
            {
                inspector_pictureBoxImage.Visible = false;
                inspector_axWindowsMediaPlayer.Visible = true;

                inspector_axWindowsMediaPlayer = WallpaperManagerTools.InitializeWindowMediaPlayer(inspector_axWindowsMediaPlayer, inspectedImage);
            }

            inspector_textBoxRankEditor.Text = WallpaperData.GetImageData(inspectedImage).Rank.ToString();

            panelImageInspector.ResumeLayout();
            panelImageInspector.Visible = true;
        }

        private void DeactivateImageInspector()
        {
            inspector_axWindowsMediaPlayer.settings.volume = 0;
            inspector_axWindowsMediaPlayer.Enabled = false;

            buttonInspectImage.Text = "Inspect Image";
            panelImageInspector.Visible = false;
            UpdateImageRanks();
        }
    }
}
