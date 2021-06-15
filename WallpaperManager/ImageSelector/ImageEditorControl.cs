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
        public WallpaperData.ImageData ImageData { get; }

        public ImageEditorControl(WallpaperData.ImageData imageData, bool immediatelyLoadImage)
        {
            InitializeComponent();

            this.ImageData = imageData;

            SuspendLayout();
            buttonSelectImage.Click += (o, i) => { WallpaperData.WallpaperManagerForm.UpdateSelectedImage(ImageData); };

            textBoxRankEditor.Text = ImageData.Rank.ToString();
            textBoxRankEditor.Click += (s, e) => textBoxRankEditor.SelectAll(); // highlights all text when clicking on the control
            textBoxRankEditor.LostFocus += TextBoxRankEditor_LostFocus;
            textBoxRankEditor.KeyDown += TextBoxRankEditor_KeyDown;

            buttonMinus.Click += (o, i) => { ImageData.Rank--; textBoxRankEditor.Text = ImageData.Rank.ToString(); };
            buttonPlus.Click += (o, i) => { ImageData.Rank++; textBoxRankEditor.Text = ImageData.Rank.ToString(); };

            if (immediatelyLoadImage) // this can cause lag if too many ImageEditorControl objects are loaded at once
            {
                WallpaperData.WallpaperManagerForm.LoadImage(this, ImageData.Path); //? calls back to SetBackgroundImage after getting the needed info to set the background
            }

            if (!ImageData.Active) // if EnableDetectionOfInactiveImages is true, this'll show which images are disabled
            {
                buttonSelectImage.FlatAppearance.BorderColor = Color.Red;
            }

            ResumeLayout();
        }

        public void SetBackgroundImage(Image image) => buttonSelectImage.BackgroundImage = image;

        public Size GetBackgroundImageSize() => buttonSelectImage.Size;

        public void UpdateRank() => textBoxRankEditor.Text = ImageData.Rank.ToString();

        protected override System.Drawing.Point ScrollToControl(Control activeControl)
        {
            // Returning the current location prevents the panel from
            // scrolling to the active control when the panel loses and regains focus
            return this.DisplayRectangle.Location;
        }

        private void buttonEditImage_Click(object sender, EventArgs e)
        {
            using (ImageEditorForm f = new ImageEditorForm(ImageData))
            {
                f.ShowDialog();
            }
        }

        // Rank Editor Lost Focus Event (Updates rank upon losing focus of the control)
        private void TextBoxRankEditor_LostFocus(object o, EventArgs i)
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
        }

        // Rank Editor Key Down Event (Updates rank upon pressing the enter key)
        private void TextBoxRankEditor_KeyDown(object o, KeyEventArgs i)
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
        }
    }
}
