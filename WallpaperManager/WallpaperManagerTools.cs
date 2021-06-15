using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AxWMPLib;
using LanceTools.FormUtil;
using Microsoft.WindowsAPICodePack.Shell;

using Emgu.CV;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using WallpaperManager.ApplicationData;
using WMPLib;
using _WMPOCXEvents_MouseMoveEventHandler = WMPLib._WMPOCXEvents_MouseMoveEventHandler;
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

        public static bool IsSupportedVideoType(string filePath)
        {
            if (File.Exists(filePath))
            {
                return IsSupportedVideoType(new FileInfo(filePath));
            }
            else
            {
                return false;
            }
        }

        public static bool IsSupportedVideoType(FileInfo fileInfo)
        {
            string extension = fileInfo.Extension;
            return extension == ".mp4" || extension == ".webm" || extension == ".avi";
        }

        public static Image GetImageFromFile(string filePath)
        {
            try
            {
                if (!IsSupportedVideoType(filePath))
                {
                    return Image.FromFile(filePath);
                }
                else
                {
                    return GetFirstVideoFrame(filePath);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Attempted to load an unsupported file type: " + filePath + "\n" + e.Message);
                return null;
            }
        }

        //! NOTE: You can't have two threads accessing a bitmap at the same time
        public static Bitmap GetFirstVideoFrame(string videoPath)
        {
            Bitmap bitmap;
            using (VideoCapture video = new VideoCapture(videoPath))
            {
                try
                {
                    bitmap = video.QueryFrame().ToBitmap();
                }
                catch
                {
                    Debug.WriteLine("Bitmap failed to load for video: " + videoPath);
                    bitmap = new Bitmap(1, 1); // TODO test what happens when you try to load a bitmap with a height and width of 0
                }
            }

            return bitmap;
        }

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
    }
}
