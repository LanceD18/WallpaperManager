using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Imaging;
using LanceTools.FormUtil;
using Microsoft.WindowsAPICodePack.Shell;

using Emgu.CV;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using WallpaperManager.ApplicationData;
using Bitmap = System.Drawing.Bitmap;

namespace WallpaperManager
{
    public enum SelectionType
    {
        None,
        Active,
        All
    }

    public enum ImageType
    {
        None,
        Static,
        GIF,
        Video
    }

    public enum FrequencyType
    {
        Relative,
        Exact
    }

    public static class WallpaperManagerTools
    {
        public static readonly string IMAGE_FILES_FILTER = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.gif, *.mp4, *.webm, *.avi)" +
                                                           " | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.gif; *.mp4; *.webm; *.avi";

        public static bool IsSupportedVideoType(string extension) => extension == ".mp4" || extension == ".webm" || extension == ".avi";

        public static SelectionType ChooseSelectionType()
        {
            SelectionType selectionType = SelectionType.None;

            Button selectedImageButton = new Button();
            selectedImageButton.AutoSize = true;
            selectedImageButton.Text = "Active Selected Image";
            selectedImageButton.Click += (o, i) => { selectionType = SelectionType.Active; };

            Button allImagesButton = new Button();
            allImagesButton.AutoSize = true;
            allImagesButton.Text = "All Selected Images";
            allImagesButton.Click += (o, i) => { selectionType = SelectionType.All; };

            MessageBoxDynamic.Show("Choose a selection type", "Choose an option", new Button[] { selectedImageButton, allImagesButton }, true);

            return selectionType;
        }

        public static Bitmap GetFirstVideoFrame(string videoPath)
        {
            Bitmap bitmap;
            using (VideoCapture video = new VideoCapture(videoPath))
            {
                var videoInfo = new MediaFile {Filename = videoPath}; //! This is part of the below
                using (var engine = new Engine()) { engine.GetMetadata(videoInfo); } //! Not sure what this was here for

                Mat m = new Mat();
                video.Read(m);
                video.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosAviRatio, 0);
                bitmap = video.QueryFrame().ToBitmap().Clone(new Rectangle(0, 0, video.Width, video.Height), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            }

            return bitmap;
        }
    }
}
