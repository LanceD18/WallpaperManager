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

        private void comboBoxSelectStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWallpaperStyle();

            if (PathData.IsWallpapersValid())
            {
                SetWallpaper();
                Thread.Sleep(250);
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

        public void UpdateWallpaperStyle(PictureStyle newWallpaperStyle)
        {
            WallpaperStyle = newWallpaperStyle;
            comboBoxSelectStyle.Text = newWallpaperStyle.ToString();
        }

        public PictureStyle GetWallpaperStyle()
        {
            return WallpaperStyle;
        }
    }
}
