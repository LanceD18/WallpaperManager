using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperManager.Controls
{
    public struct DisplaySettings
    {
        public int[] WallpaperIntervals;
        public WallpaperStyle[] WallpaperStyles;
        public bool Synced;

        public DisplaySettings(int[] wallpaperInterval, WallpaperStyle[] wallpaperStyle, bool synced)
        {
            this.WallpaperIntervals = wallpaperInterval;
            this.WallpaperStyles = wallpaperStyle;
            this.Synced = synced;
        }
    }
}
