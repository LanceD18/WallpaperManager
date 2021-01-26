using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools;
using LanceTools.ControlLibrary;
using LanceTools.FormUtil;
using Mpv.NET.Player;
using WallpaperManager.ApplicationData;
using WallpaperManager.ImageSelector;
using WallpaperManager.Options;

namespace WallpaperManager
{
    public partial class WallpaperManagerForm : Form
    {
        private string[] selectedImages;
        private int imagesPerPage;

        private float initialLabelSelectedImageFontSize;

        private Queue<FlowLayoutPanel> loadedTabs = new Queue<FlowLayoutPanel>();
        private Dictionary<string, Bitmap> loadedImageInfo = new Dictionary<string, Bitmap>();
        //!private List<Bitmap> loadedImages = new List<Bitmap>(); //? consider making this a ReactiveList, although note that OnAdd won't be able to find the string
        //!private List<string> loadedImagePaths = new List<string>();
        private int maxLoadedTabs;

        private SmoothScrollFlowLayoutPanel activeTabLayoutPanel;

        private static Panel imageEditorVideoPanel;
        private static MpvPlayer imageEditorVideoPlayer;

        private void tabLayoutPanel_MouseClick(object sender, EventArgs e)
        {
            (sender as SmoothScrollFlowLayoutPanel).Focus();
        }

        private void ClearImageSelector()
        {
            panelImageSelector.Visible = false;

            InspectedImage = "";
            selectedImages = null;
            DeactivateImageInspector();

            labelSelectedImage.Text = "Select an image for more info";
            labelImageSize.Text = "";

            ClearLoadedImages();
            loadedTabs.Clear();

            tabControlImagePages.SuspendLayout();
            tabControlImagePages.Flush();
            tabControlImagePages.ResumeLayout();
        }

        private void InitializeImageSelector()
        {
            tabControlImagePages.SelectedIndexChanged += LoadTabEvent;
            labelSelectedImage.TextChanged += labelSelectedImage_TextChanged;
            initialLabelSelectedImageFontSize = labelSelectedImage.Font.Size;
            labelImageSize.Text = "";

            new ToolTip(components).SetToolTip(buttonNameImage, "Renames images based on their given tags (If the image has any tags)" +
                                                                "\n*The theme will auto-save after renaming*");
            new ToolTip(components).SetToolTip(buttonMoveImage, "If 'Allow Tag-Based Renaming for Moved Images' is selected, every moved image will be" +
                                                                "\nrenamed based on their given tags (If the image has any tags)" +
                                                                "\nThe default behavior will directly move the image as if you were to normally do so" +
                                                                "\n*The theme will auto-save after moving");
        }

        public void RebuildImageSelector(string[] selectedImages, bool reverseOrder, int imagesPerPage = 30, int maxLoadedTabs = 25)
        {
            // Cancel the rebuild early if there were no images selected
            if (selectedImages == null)
            {
                MessageBox.Show("No images were selected");
                return;
            }

            // Check for null images (The EnableDetectionOfInactiveImages function is handled further down)
            int invalidCounter = 0;
            foreach (string image in selectedImages)
            {
                if (image == null)
                {
                    invalidCounter++;
                }
            }

            if (invalidCounter == selectedImages.Length)
            {
                MessageBox.Show("No images were selected");
                return;
            }

            if (invalidCounter > 0)
            {
                MessageBox.Show("Error, some of the images selected were null (Does not include disabled images)");
                return;
            }

            ClearImageSelector();

            if (reverseOrder) Array.Reverse(selectedImages); // generally, with the way the collections have been handled an "in-order" result will start from z, or backwards

            // Ensure that only enabled images are selected
            if (!OptionsData.ThemeOptions.EnableDetectionOfInactiveImages)
            {
                HashSet<string> activeSelectedImages = new HashSet<string>();
                foreach (string image in selectedImages)
                {
                    if (WallpaperData.ContainsImage(image)) // nothing's stopping you from seleting any image in the file explorer search
                    {
                        if (WallpaperData.GetImageData(image).Active)
                        {
                            activeSelectedImages.Add(image);
                        }
                    }
                }

                selectedImages = activeSelectedImages.ToArray();
            }

            if (selectedImages.Length == 0)
            {
                MessageBox.Show("No images were selected");
                return;
            }

            int pageCount = (selectedImages.Length / imagesPerPage) + 1;

            for (int i = 0; i < pageCount; i++)
            {
                tabControlImagePages.TabPages.Add((i + 1).ToString() + " Key", (i + 1).ToString());

                TabPage curTab = tabControlImagePages.TabPages[i];
                curTab.BackColor = Color.Black;
            }

            // Finalization
            this.selectedImages = selectedImages;
            this.imagesPerPage = imagesPerPage;
            this.maxLoadedTabs = maxLoadedTabs;

            LoadTab(0);
            //tabControlImagePages.Refresh();
            tabControlImagePages.ResumeLayout();

            panelImageSelector.Visible = true;
        }

        private void LoadTabEvent(object sender, EventArgs e)
        {
            LoadTab(tabControlImagePages.SelectedIndex);
        }

        private async void LoadTab(int tabIndex)
        {
            if (tabIndex != -1 && selectedImages != null) // tabIndex will equal -1 when resetting the tabControl | selectedImages may equal null when clearing
            {
                tabControlImagePages.SuspendLayout();
                ClearLoadedImages();

                // Add controls to their panels
                activeTabLayoutPanel?.Dispose();

                SmoothScrollFlowLayoutPanel tabLayoutPanel = new SmoothScrollFlowLayoutPanel();
                tabLayoutPanel.Size = new Size(tabControlImagePages.Size.Width - 5, tabControlImagePages.Size.Height - tabControlImagePages.ItemSize.Height - 5);
                tabLayoutPanel.AutoScroll = true;
                tabLayoutPanel.HorizontalScroll.Visible = false;
                tabLayoutPanel.MouseClick += tabLayoutPanel_MouseClick;

                if (tabControlImagePages.TabPages[tabIndex].Controls.Count > 0)
                {
                    tabControlImagePages.TabPages[tabIndex].Controls.RemoveAt(0);
                }

                tabControlImagePages.TabPages[tabIndex].Controls.Add(tabLayoutPanel); // panel added here

                // Load in image editors
                int indexOffset = tabIndex * imagesPerPage;
                int maxIndex = imagesPerPage + indexOffset - 1;
                maxIndex = maxIndex < selectedImages.Length ? maxIndex : selectedImages.Length;

                string invalidImageSelection = "The following images are not included in your theme: ";
                string invalidImageSelectionDefault = invalidImageSelection;

                tabLayoutPanel.SuspendLayout();
                for (int i = indexOffset; i <= maxIndex; i++)
                {
                    if (i != selectedImages.Length)
                    {
                        if (selectedImages[i] != null && File.Exists(selectedImages[i]) && WallpaperData.ContainsImage(selectedImages[i]))
                        {
                            tabLayoutPanel.Controls.Add(new ImageEditorControl(WallpaperData.GetImageData(selectedImages[i]), false));
                        }
                        else
                        {
                            invalidImageSelection += "\n" + selectedImages[i];
                        }
                    }
                }

                //?tabLayoutPanel.UpdateScroll(); Used in the FastScrollFlowLayoutPanel version of the tabLayoutPanel, may swap back to this in the future
                tabLayoutPanel.ResumeLayout();

                // Loading all of these at the same time is not ideal
                await Task.Run(() =>
                {
                    ImageEditorControl[] imageEditorControls = tabLayoutPanel.Controls.OfType<ImageEditorControl>().ToArray();

                    foreach (ImageEditorControl control in imageEditorControls)
                    {
                        LoadImage(control, control.ImageData.Path);
                    }
                });

                tabLayoutPanel.Focus(); // clicking on the Tab Control to change pages loses the focus of the panel
                activeTabLayoutPanel = tabLayoutPanel;
                //tabControlImagePages.Refresh();
                tabControlImagePages.ResumeLayout();

                if (invalidImageSelectionDefault != invalidImageSelection)
                {
                    MessageBox.Show(invalidImageSelection);
                }
            }
        }

        public void UpdateSelectedImage(WallpaperData.ImageData imageData)
        {
            labelSelectedImage.Text = InspectedImage = imageData.Path;
        }

        private void ClearLoadedImages()
        {
            // the bitmaps aren't saved anywhere so they must be disposed here
            foreach (Bitmap bitmap in loadedImageInfo.Values) bitmap?.Dispose();
            loadedImageInfo.Clear();
        }

        private void labelSelectedImage_TextChanged(object sender, EventArgs e) // resize if bounds extend too far to the right
        {
            //TODO Figure out how to do this properly
            /*
            labelSelectedImage.Refresh();

            if (labelSelectedImage.Bounds.Right > panelImageSelector.Width)
            {
                int actualBoundsRight = labelSelectedImage.Bounds.Right + panelImageSelector.Width;
                float newSize = (float)Math.Floor(((initialLabelSelectedImageFontSize * (float) this.Width / actualBoundsRight) - 0.25f));
                Debug.WriteLine(newSize);
                Font resizedFont = new Font
                (
                    labelSelectedImage.Font.FontFamily,
                    newSize,//initialLabelSelectedImageFontSize * (float)this.Bounds.Right / actualBoundsRight,
                    labelSelectedImage.Font.Style,
                    labelSelectedImage.Font.Unit,
                    labelSelectedImage.Font.GdiCharSet
                );

                labelSelectedImage.Font = resizedFont;
            }
            else if (labelSelectedImage.Font.Size != initialLabelSelectedImageFontSize)
            {
                Font resizedFont = new Font
                (
                    labelSelectedImage.Font.FontFamily,
                    initialLabelSelectedImageFontSize,
                    labelSelectedImage.Font.Style,
                    labelSelectedImage.Font.Unit,
                    labelSelectedImage.Font.GdiCharSet
                );

                labelSelectedImage.Font = resizedFont;
            }
            */
        }

        //? This allows the image to be disposed when changing pages, otherwise it would just be loaded in the ImageEditorControl
        public async void LoadImage(ImageEditorControl parentEditorControl, string imagePath)
        {
            await Task.Run(() =>
            {
                using (Image image = WallpaperManagerTools.GetImageFromFile(imagePath))
                {
                    if (image != null) // this will happen to unsupported file types
                    {
                        //! the image must be re-drawn to prevent it from being used by wallpaper manager
                        //! which is why it needed to be put onto a bitmap
                        //! don't change to image, if you do make sure to test what happens when an image is used & loaded simultaneously
                        Bitmap imageBitmap = new Bitmap(image.Width, image.Height);
                        using (Graphics g = Graphics.FromImage(imageBitmap)) g.DrawImage(image, 0, 0, image.Width, image.Height);

                        loadedImageInfo.Add(imagePath, imageBitmap);  //? Disposes images later (Whenever the page is changed)
                        parentEditorControl.SetBackgroundImage(imageBitmap);
                    }
                }
            }).ConfigureAwait(false); // ConfigureAwait(false) prevents a UI deadlock in the instance that the calling function needed to do LoadImage().Result
        }

        public string GetActiveImage()
        {
            return InspectedImage ?? "";
        }

        public string[] GetSelectedImages()
        {
            return selectedImages ?? new string[0];
        }

        public void UpdateImageRanks()
        {
            if (activeTabLayoutPanel != null)
            {
                foreach (Control control in activeTabLayoutPanel.Controls)
                {
                    (control as ImageEditorControl)?.UpdateRank();
                }
            }
        }
    }
}
