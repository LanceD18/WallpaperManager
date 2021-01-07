using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.ImageSelector
{
    public partial class ImageEditorControl : UserControl
    {
        private WallpaperData.ImageData ImageData;

        public ImageEditorControl(WallpaperData.ImageData imageData)
        {
            InitializeComponent();

            this.ImageData = imageData;

            SuspendLayout();
            buttonSelectImage.Click += (o, i) => { WallpaperData.WallpaperManagerForm.UpdateSelectedImage(ImageData); };

            // Rank Editor Lost Focus Event (Updates rank upon losing focus of the control)
            textBoxRankEditor.Text = ImageData.Rank.ToString();
            textBoxRankEditor.LostFocus += (o, i) =>
            {
                try
                {
                    ImageData.Rank = int.Parse(textBoxRankEditor.Text);
                    textBoxRankEditor.Text = ImageData.Rank.ToString();
                }
                catch
                {
                    textBoxRankEditor.Text = ImageData.Rank.ToString();
                }
            };

            // Rank Editor Key Down Event (Updates rank upon pressing the enter key)
            textBoxRankEditor.KeyDown += (o, i) =>
            {
                if (i.KeyCode == Keys.Enter)
                {
                    try
                    {
                        ImageData.Rank = int.Parse(textBoxRankEditor.Text);
                        textBoxRankEditor.Text = ImageData.Rank.ToString();
                    }
                    catch
                    {
                        textBoxRankEditor.Text = ImageData.Rank.ToString();
                    }

                    i.SuppressKeyPress = true; // prevents windows 'ding' sound
                }
            };

            buttonMinus.Click += (o, i) => { ImageData.Rank--; textBoxRankEditor.Text = ImageData.Rank.ToString(); };
            buttonPlus.Click += (o, i) => { ImageData.Rank++; textBoxRankEditor.Text = ImageData.Rank.ToString(); };

            WallpaperData.WallpaperManagerForm.LoadImage(this, ImageData.Path); //? calls back to SetBackgroundImage after getting the needed info to set the background

            if (!ImageData.Active) // if EnableDetectionOfInactiveImages is true, this'll show which images are disabled
            {
                buttonSelectImage.FlatAppearance.BorderColor = Color.Red;
            }
            ResumeLayout();
        }

        public void SetBackgroundImage(Image image) => buttonSelectImage.BackgroundImage = image;

        public void UpdateRank() => textBoxRankEditor.Text = ImageData.Rank.ToString();

        private void buttonEditImage_Click(object sender, EventArgs e)
        {
            using (ImageEditorForm f = new ImageEditorForm(ImageData))
            {
                f.ShowDialog();
            }
        }

        protected override System.Drawing.Point ScrollToControl(Control activeControl)
        {
            // Returning the current location prevents the panel from
            // scrolling to the active control when the panel loses and regains focus
            return this.DisplayRectangle.Location;
        }
    }
}
