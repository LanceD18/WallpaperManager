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

    public partial class WallpaperManagerForm : Form
    {
        private PictureStyle WallpaperStyle;

        private void comboBoxSelectStyle_SelectedIndexChanged(object sender, EventArgs e) => UpdateWallpaperStyle();

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

                case "Zoom":
                    WallpaperStyle = PictureStyle.Zoom;
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
            UpdateWallpaperStyle();
        }

        public PictureStyle GetWallpaperStyle() => WallpaperStyle;
    }
}
