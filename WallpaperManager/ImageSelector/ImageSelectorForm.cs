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
using WallpaperManager.ApplicationData;

namespace WallpaperManager.ImageSelector
{
    public partial class ImageSelectorForm : Form
    {
        private string[] selectedImages;
        private int imagesPerPage;

        public ImageSelectorForm(string[] selectedImages, int imagesPerPage = 25)
        {
            if (selectedImages.Length == 0)
            {
                MessageBox.Show("No images were selected");
                //TODO Prevent this from opening the form without causing a crash
                return;
            }

            InitializeComponent();

            this.selectedImages = selectedImages;
            this.imagesPerPage = imagesPerPage;

            int pageCount = ((int)selectedImages.Length / imagesPerPage) + 1;

            tabControlImagePages.BackColor = Color.Black;
            tabControlImagePages.SelectedIndexChanged += LoadTabEvent;
            tabControlImagePages.ItemSize = new Size(20, 18);

            tabControlImagePages.SuspendLayout();
            for (int i = 0; i < pageCount; i++)
            {
                tabControlImagePages.TabPages.Add((i + 1).ToString() + " Key", (i + 1).ToString());

                TabPage curTab = tabControlImagePages.TabPages[i];
                curTab.BackColor = Color.Black;

                FlowLayoutPanel tabLayoutPanel = new FlowLayoutPanel();
                tabLayoutPanel.Size = new Size(tabControlImagePages.Size.Width - 5, tabControlImagePages.Size.Height - tabControlImagePages.ItemSize.Height - 5);
                tabLayoutPanel.AutoScroll = true;
                tabLayoutPanel.HorizontalScroll.Visible = false;

                curTab.Controls.Add(tabLayoutPanel);
            }
            tabControlImagePages.ResumeLayout();

            LoadTab(0);
        }

        private void LoadTabEvent(object sender, EventArgs e)
        {
            LoadTab(tabControlImagePages.SelectedIndex);
        }

        private void LoadTab(int tabIndex)
        {
            FlowLayoutPanel curTabLayoutPanel = tabControlImagePages.TabPages[tabIndex].Controls[0] as FlowLayoutPanel;

            if (curTabLayoutPanel.Controls.Count == 0) // if true, then this tab has not yet been loaded
            {
                int indexOffset = tabIndex * imagesPerPage;
                int maxIndex = imagesPerPage + indexOffset - 1;
                maxIndex = maxIndex < selectedImages.Length ? maxIndex : selectedImages.Length;

                string invalidImageSelection = "The following images are not included in your theme: ";
                string invalidImageSelectionDefault = invalidImageSelection;

                curTabLayoutPanel.SuspendLayout();
                for (int i = indexOffset; i < maxIndex; i++)
                {
                    if (selectedImages[i] != null && WallpaperData.ContainsImage(selectedImages[i]))
                    {
                        curTabLayoutPanel.Controls.Add(new ImageEditorControl(WallpaperData.GetImageData(selectedImages[i])));
                    }
                    else
                    {
                        invalidImageSelection += "\n" + selectedImages[i];
                    }
                }
                curTabLayoutPanel.ResumeLayout();

                if (invalidImageSelectionDefault != invalidImageSelection)
                {
                    MessageBox.Show(invalidImageSelection);
                }
            }
        }

        public void UpdateSelectedImage(WallpaperData.ImageData imageData)
        {
            labelSelectedImage.Text = imageData.Path;
        }
    }
}
