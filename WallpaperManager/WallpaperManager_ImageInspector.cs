using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;
using LanceTools;
using LanceTools.Mpv;
using Mpv.NET.API;
using Mpv.NET.Player;
using Vlc.DotNet.Forms;
using WallpaperManager.ApplicationData;
using WallpaperManager.ImageSelector;
using WallpaperManager.Controls;
using WallpaperManager.Pathing;

namespace WallpaperManager
{
    public partial class WallpaperManagerForm : Form
    {
        private string _inspectedImage;
        public string InspectedImage
        {
            get => _inspectedImage;
            private set
            {
                _inspectedImage = value; 
                labelSelectedImage.Text = value;

                if (File.Exists(value))
                {
                    Image image = WallpaperManagerTools.GetImageFromFile(value);

                    if (image != null)
                    {
                        labelImageSize.Text = image.Width + "x" + image.Height;
                        labelImageSize.Left = panelImageSelector.Location.X - labelImageSize.Size.Width - 5;
                        image.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Attempted to load an unsupported file type");
                    }
                }
            }
        }

        private MpvPlayer inspector_mpvPlayer; //? Initialized in OnLoad
        //?private VlcControl inspector_VlcControl;

        public bool IsViewingInspector;

        private void InitializeImageInspector()
        {
            // Init Inspector Panel
            panelImageInspector.Visible = false;
            panelImageInspector.BringToFront();

            inspector_mpvPlayer = new MpvPlayer(inspector_panelVideo.Handle)
            {
                AutoPlay = true,
                Loop = true
            };
            inspector_mpvVideoBar.LinkPlayer(inspector_mpvPlayer, MpvBarUpdater);

            /*!
            // VLC Control
            inspector_VlcControl = new VlcControl();

            ((ISupportInitialize)(this.inspector_VlcControl)).BeginInit();

            inspector_VlcControl.Bounds = inspector_panelVideo.Bounds;
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            inspector_VlcControl.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            ((ISupportInitialize)(this.inspector_VlcControl)).EndInit();

            inspector_panelVideo.Controls.Add(inspector_VlcControl);
            */

            // Inspector Button Events
            SuspendLayout();
            inspector_textBoxRankEditor.LostFocus += (o, i) =>
            {
                try
                {
                    WallpaperData.GetImageData(InspectedImage).Rank = Int32.Parse(inspector_textBoxRankEditor.Text);
                    inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(InspectedImage).ToString();
                }
                catch
                {
                    inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(InspectedImage).ToString();
                }
            };

            inspector_textBoxRankEditor.KeyDown += (o, i) =>
            {
                if (i.KeyCode == Keys.Enter)
                {
                    try
                    {
                        WallpaperData.GetImageData(InspectedImage).Rank = Int32.Parse(inspector_textBoxRankEditor.Text);
                        inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(InspectedImage).ToString();
                    }
                    catch
                    {
                        inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(InspectedImage).ToString();
                    }

                    i.SuppressKeyPress = true; // prevents windows 'ding' sound
                }
            };

            inspector_buttonMinus.Click += (o, i) => { WallpaperData.GetImageData(InspectedImage).Rank--; inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(InspectedImage).ToString(); };
            inspector_buttonPlus.Click += (o, i) => { WallpaperData.GetImageData(InspectedImage).Rank++; inspector_textBoxRankEditor.Text = WallpaperData.GetImageRank(InspectedImage).ToString(); };

            inspector_buttonLeft.Click += (o, i) =>
            {
                int leftIndex = selectedImages.IndexOf(InspectedImage) - 1;

                if (leftIndex >= 0)
                {
                    InspectedImage = selectedImages[leftIndex];
                    ActivateImageInspector(); // more like updating image inspector
                }
            };

            inspector_buttonRight.Click += (o, i) =>
            {
                int rightIndex = selectedImages.IndexOf(InspectedImage) + 1;

                if (rightIndex < selectedImages.Length)
                {
                    InspectedImage = selectedImages[rightIndex];
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
                    if (selectedImages.Contains(InspectedImage))
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
            IsViewingInspector = true;

            panelImageInspector.SuspendLayout();
            buttonInspectImage.Text = "Close Inspector";

            if (!WallpaperManagerTools.IsSupportedVideoType(InspectedImage)) // display image
            {
                inspector_panelVideo.Visible = false;
                inspector_panelVideo.Enabled = false;

                inspector_pictureBoxImage.Enabled = true;
                inspector_pictureBoxImage.Visible = true;
                inspector_pictureBoxImage.ImageLocation = InspectedImage;

                inspector_pictureBoxImage.BringToFront(); //? despite disabling the other control, this still needs to be called
            }
            else // display video
            {
                inspector_pictureBoxImage.Visible = false;
                inspector_pictureBoxImage.Enabled = false;

                inspector_panelVideo.Enabled = true;
                inspector_panelVideo.Visible = true;

                inspector_mpvPlayer.Reload(InspectedImage);
                WallpaperData.VideoSettings VideoSettings = WallpaperData.GetImageData(InspectedImage).VideoSettings;
                inspector_mpvPlayer.Volume = VideoSettings.Volume;
                inspector_mpvPlayer.Speed = VideoSettings.PlaybackSpeed;
                fixAdministered = true; //? if the inspector displays first the fix will no longer be needed

                inspector_mpvVideoBar.UpdatePlayerVolume(); //! This must be done after inspector_mpvPlayer settings are set otherwise the bar won't update properly to the new video

                /*!
                inspector_VlcControl.Audio.Volume = WallpaperData.GetImageData(inspectedImage).VideoSettings.volume;
                string[] mediaOptions = new[] { "input-repeat=65535" }; // 65535 is the max https://wiki.videolan.org/VLC_command-line_help/
                //vlcControl1.SetMedia(new FileInfo("F:\\Documents\\Game Notes\\Main Theme\\Secondary Theme\\video0_7.mp4"), mediaOptions);
                inspector_VlcControl.Play(new FileInfo(inspectedImage), mediaOptions);
                */

                inspector_panelVideo.BringToFront(); //? this doesn't need to be called for some reason, but for consistency with the above i'll keep it here
            }

            inspector_textBoxRankEditor.Text = WallpaperData.GetImageData(InspectedImage).Rank.ToString();

            panelImageInspector.ResumeLayout();
            panelImageInspector.Visible = true;
        }

        private void DeactivateImageInspector()
        {
            IsViewingInspector = false;

            panelImageInspector.SuspendLayout();
            inspector_mpvPlayer.Stop();
            inspector_mpvPlayer.Volume = 0;
            //!inspector_VlcControl.Stop();
            inspector_panelVideo.Enabled = false;

            inspector_pictureBoxImage.Enabled = false;

            buttonInspectImage.Text = "Inspect Image";
            panelImageInspector.Visible = false;
            panelImageInspector.ResumeLayout();
            UpdateImageRanks();
        }

        private void MpvBarUpdater()
        {
            WallpaperData.GetImageData(InspectedImage).VideoSettings = new WallpaperData.VideoSettings(
                inspector_mpvVideoBar.GetVolume(),
                inspector_mpvVideoBar.GetSpeed());

            for (var i = 0; i < WallpaperPathing.ActiveWallpapers.Length; i++)
            {
                if (WallpaperPathing.ActiveWallpapers[i] == InspectedImage) // this wallpaper is currently active, change its volume
                {
                    wallpapers[i].SetVolume(inspector_mpvVideoBar.GetVolume());
                }
            }
        }
    }
}
