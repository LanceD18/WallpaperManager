using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
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

        //! be careful not to have multiple threads accessing this at the same time, you can't have two threads accessing a bitmap at the same time
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
                try
                {
                    bitmap = video.QueryFrame().ToBitmap().Clone(new Rectangle(0, 0, video.Width, video.Height), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                }
                catch
                {
                    Debug.WriteLine("Bitmap failed to load for video: " + videoPath);
                    bitmap = new Bitmap(0, 0);
                }
            }

            return bitmap;
        }

        /*x
        public static AxWindowsMediaPlayer InitializeWindowsMediaPlayer(AxWindowsMediaPlayer axWindowsMediaPlayer, bool editable)
        {
            axWindowsMediaPlayer.Enabled = false;
            axWindowsMediaPlayer.stretchToFit = true;
            axWindowsMediaPlayer.settings.setMode("loop", true);
            axWindowsMediaPlayer.PlayStateChange += (s, e) =>
            {
                // ensures that the video auto-starts, some video types fail to do so regularly, such as .webm (They generally take longer to load too)
                //if (axWindowsMediaPlayer.playState == WMPPlayState.wmppsReady) 
                if (e.newState == 10) // ready state
                {
                    try // this may sometimes cause an error however this doesn't break the program so just ignore it, the video should eventually play
                    {
                        Action playInvoker = () => axWindowsMediaPlayer.Ctlcontrols.play();

                        axWindowsMediaPlayer.BeginInvoke(playInvoker); // ensures that the program waits for the media to load before playing it
                        Debug.WriteLine("Playing: " + axWindowsMediaPlayer.URL);
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                    }
                }

                Debug.WriteLine(axWindowsMediaPlayer.playState.ToString() + '[' + axWindowsMediaPlayer.URL + ']');
            };

            if (editable) // If editable, allow the volume slider to be saved
            {
                //? The MouseDownEvent will only save the volume at the time of clicking while the MouseUpEvent won't work while using the slider
                //TODO To improve this you'll need to make your own slider and attach it to the control
                axWindowsMediaPlayer.MouseMoveEvent += (s, e) =>
                {
                    //! This event should only be added once!!!
                    WallpaperData.GetImageData(axWindowsMediaPlayer.URL).VideoSettings.volume = axWindowsMediaPlayer.settings.volume;
                };
            }
            else // Not editable, disable UI
            {
                axWindowsMediaPlayer.uiMode = "none";
            }

            return axWindowsMediaPlayer;
        }

        public static AxWindowsMediaPlayer UpdateWindowsMediaPlayer(AxWindowsMediaPlayer axWindowsMediaPlayer, string videoPath)
        {
            axWindowsMediaPlayer.URL = videoPath;

            WallpaperData.VideoSettings VideoSettings = WallpaperData.GetImageData(videoPath).VideoSettings;
            axWindowsMediaPlayer.settings.volume = VideoSettings.volume;
            axWindowsMediaPlayer.settings.rate = VideoSettings.playbackSpeed;
            axWindowsMediaPlayer.Enabled = true;
            axWindowsMediaPlayer.settings.autoStart = true;

            // these two lines force the video to autoplay regardless of its video type, I hope
            axWindowsMediaPlayer.Ctlcontrols.currentPosition = 0;
            axWindowsMediaPlayer.Ctlcontrols.play();

            return axWindowsMediaPlayer;
        }
        */

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
