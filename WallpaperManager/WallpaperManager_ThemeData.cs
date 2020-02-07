using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager
{
    public partial class WallpaperManager : Form
    {
        private void buttonLoadTheme_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                // dialog properties
                dialog.Title = "Select a compatible json file to load";
                dialog.Filter = "Wallpaper Data (*.json, *.xml) | *.json; *.xml";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    WallpaperData.LoadData(dialog.FileName);
                    PathData.ActiveWallpaperTheme = dialog.FileName;
                }
            }
        }

        private void buttonSaveTheme_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Save data to json";
                dialog.Filter = "Image files (*.json) | *.json";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    WallpaperData.SaveData(dialog.FileName);
                }
            }
        }

        private void buttonUpdateTheme_Click(object sender, EventArgs e)
        {
            WallpaperData.SaveData(PathData.ActiveWallpaperTheme);
        }
    }
}
