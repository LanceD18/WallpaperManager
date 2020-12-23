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
        private WallpaperData.ImageData imageData;

        public ImageEditorControl(WallpaperData.ImageData imageData)
        {
            InitializeComponent();

            this.imageData = imageData;

            SuspendLayout();
            buttonSelectImage.Click += (o, i) => { WallpaperData.WallpaperManagerForm.UpdateSelectedImage(imageData); };

            // Rank Editor Lost Focus Event (Updates rank upon losing focus of the control)
            textBoxRankEditor.Text = imageData.Rank.ToString();
            textBoxRankEditor.LostFocus += (o, i) =>
            {
                try
                {
                    imageData.Rank = int.Parse(textBoxRankEditor.Text);
                    textBoxRankEditor.Text = imageData.Rank.ToString();
                }
                catch
                {
                    textBoxRankEditor.Text = imageData.Rank.ToString();
                }
            };

            // Rank Editor Key Down Event (Updates rank upon pressing the enter key)
            textBoxRankEditor.KeyDown += (o, i) =>
            {
                if (i.KeyCode == Keys.Enter)
                {
                    try
                    {
                        imageData.Rank = int.Parse(textBoxRankEditor.Text);
                        textBoxRankEditor.Text = imageData.Rank.ToString();
                    }
                    catch
                    {
                        textBoxRankEditor.Text = imageData.Rank.ToString();
                    }

                    i.SuppressKeyPress = true; // prevents windows 'ding' sound
                }
            };

            buttonMinus.Click += (o, i) => { imageData.Rank--; textBoxRankEditor.Text = imageData.Rank.ToString(); };
            buttonPlus.Click += (o, i) => { imageData.Rank++; textBoxRankEditor.Text = imageData.Rank.ToString(); };

            WallpaperData.WallpaperManagerForm.LoadImage(this, imageData.Path);

            if (!imageData.Active) // if EnableDetectionOfInactiveImages is true, this'll show which images are disabled
            {
                buttonSelectImage.FlatAppearance.BorderColor = Color.Red;
            }
            ResumeLayout();
        }

        public void SetBackgroundImage(Image image)
        {
            buttonSelectImage.BackgroundImage = image;
        }

        public void UpdateRank()
        {
            textBoxRankEditor.Text = imageData.Rank.ToString();
        }

        private void buttonEditImage_Click(object sender, EventArgs e)
        {
            using (ImageEditorForm f = new ImageEditorForm(imageData))
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
