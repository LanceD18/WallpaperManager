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
        Tile,
        Center
    }

    public partial class WallpaperManager : Form
    {
        private PictureStyle WallpaperStyle;
        private bool cancelWallpaperStyleUpdate;

        private void comboBoxSelectStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cancelWallpaperStyleUpdate)
            {
                UpdateWallpaperStyle();

                if (PathData.IsWallpapersValid())
                {
                    SetWallpaper();
                    Thread.Sleep(250);
                }
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
                    break;

                case "Stretch":
                    WallpaperStyle = PictureStyle.Stretch;
                    break;

                case "Tile":
                    WallpaperStyle = PictureStyle.Tile;
                    break;
            }
        }

        public void UpdateWallpaperStyle(PictureStyle newWallpaperStyle, bool loadingTheme)
        {
            WallpaperStyle = newWallpaperStyle;
            comboBoxSelectStyle.Text = newWallpaperStyle.ToString();
            cancelWallpaperStyleUpdate = loadingTheme;
        }

        public PictureStyle GetWallpaperStyle()
        {
            return WallpaperStyle;
        }
    }
}
