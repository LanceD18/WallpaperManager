using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;
using WallpaperManager.Wallpaper;

namespace WallpaperManager
{
    public enum WallpaperStyle
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
        public void UpdateWallpaperStyle(int index)
        {
            switch (displaySettingForms[index].GetWallpaperStyle())
            {
                case "Fill":
                    wallpaperStyles[index] = WallpaperStyle.Fill;
                    break;

                case "Stretch":
                    wallpaperStyles[index] = WallpaperStyle.Stretch;
                    break;

                case "Zoom":
                    wallpaperStyles[index] = WallpaperStyle.Zoom;
                    break;
            }

            wallpapers[index].SetWallpaperStyle(wallpaperStyles[index]);
        }

        public void UpdateWallpaperStyle(WallpaperStyle newWallpaperStyle, int index)
        {
            wallpaperStyles[index] = newWallpaperStyle;
            displaySettingForms[index].SetWallpaperStyle(newWallpaperStyle.ToString());
            UpdateWallpaperStyle(index);
        }

        public void UpdateWallpaperStyle(WallpaperStyle[] newWallpaperStyles)
        {
            for (int i = 0; i < newWallpaperStyles.Length; i++)
            {
                UpdateWallpaperStyle(newWallpaperStyles[i], i);
            }
        }

        public WallpaperStyle GetWallpaperStyle(int index) => wallpaperStyles[index];

        public WallpaperStyle[] GetWallpaperStyles() => wallpaperStyles;
    }
}
