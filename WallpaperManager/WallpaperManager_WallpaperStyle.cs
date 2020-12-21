using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager
{
    public enum PictureStyle
    {
        Fill,
        Fit,
        Span,
        Stretch,
        Zoom,
        Center
    }

    public partial class WallpaperManager : Form
    {
        private PictureStyle WallpaperStyle;
        private PictureBoxSizeMode _WallpaperStyle;
        private bool cancelWallpaperStyleUpdate;

        private void comboBoxSelectStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cancelWallpaperStyleUpdate)
            {
                UpdateWallpaperStyle();

                /*
                if (PathData.IsWallpapersValid())
                {
                    SetWallpaper();
                    //?Thread.Sleep(250);
                }
                */
            }
            else
            {
                cancelWallpaperStyleUpdate = false;
            }
        }

        private void UpdateWallpaperStyle()
        {
            switch (comboBoxSelectStyle.SelectedItem)
            {
                case "Fill":
                    WallpaperStyle = PictureStyle.Fill;
                    _WallpaperStyle = PictureBoxSizeMode.StretchImage;
                    break;

                case "Stretch":
                    WallpaperStyle = PictureStyle.Stretch;
                    _WallpaperStyle = PictureBoxSizeMode.StretchImage;
                    break;

                case "Zoom":
                    WallpaperStyle = PictureStyle.Zoom;
                    _WallpaperStyle = PictureBoxSizeMode.Zoom;
                    break;
            }

            foreach (WallpaperForm wallpaper in wallpapers)
            {
                wallpaper.SetWallpaperStyle(WallpaperStyle);
            }
        }

        public void UpdateWallpaperStyle(PictureStyle newWallpaperStyle)
        {
            WallpaperStyle = newWallpaperStyle;
            comboBoxSelectStyle.Text = newWallpaperStyle.ToString();
            cancelWallpaperStyleUpdate = WallpaperData.IsLoadingData;
            UpdateWallpaperStyle(); // TODO Refactor this and the above to remove redundant code
        }

        public PictureStyle GetWallpaperStyle()
        {
            return WallpaperStyle;
        }
    }
}
