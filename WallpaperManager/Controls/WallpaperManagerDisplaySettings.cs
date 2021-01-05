using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Controls
{
    public partial class WallpaperManagerDisplaySettings : UserControl
    {
        private const int MARGIN = 5;
        private const int X_SHIFT = 3;
        public int Index { get; private set; }

        public WallpaperManagerDisplaySettings(int index)
        {
            InitializeComponent();
            this.Resize += OnResize;
            Index = index;
        }

        private void OnResize(object sender, EventArgs e)
        {
            int styleControlsX = MARGIN + X_SHIFT;
            labelWallpaperStyle.Location = new Point(styleControlsX, MARGIN);

            int comboBoxControlsY = labelWallpaperStyle.Location.Y + labelWallpaperStyle.Height + MARGIN;
            comboBoxSelectStyle.Location = new Point(styleControlsX, comboBoxControlsY);

            int intervalControlsX = labelWallpaperStyle.Location.X + labelWallpaperStyle.Width + (MARGIN * 2);
            labelTimeLeft.Location = new Point(intervalControlsX, MARGIN);
            comboBoxWallpaperInterval.Location = new Point(intervalControlsX, comboBoxControlsY);
        }

        public string GetWallpaperStyle() => comboBoxSelectStyle.SelectedItem as string;

        public string GetSelectedWallpaperInterval() => comboBoxWallpaperInterval.SelectedItem.ToString();

        public int GetSelectedWallpaperIntervalIndex() => comboBoxWallpaperInterval.SelectedIndex;

        public int GetIntervalCount() => comboBoxWallpaperInterval.Items.Count;

        public void SetTimeLeft(string time) => labelTimeLeft.Text = time;

        public void SetTimeLeft(int time) => labelTimeLeft.Text = time.ToString();

        public void SetWallpaperIntervalIndex(int newIndex) => comboBoxWallpaperInterval.SelectedIndex = newIndex;

        public void SetWallpaperStyle(string styleName) => comboBoxSelectStyle.Text = styleName;

        // when the user change's their preferred timer
        private void comboBoxWallpaperInterval_SelectedIndexChanged(object sender, EventArgs e) => WallpaperData.WallpaperManagerForm.UpdateTimerIndex(Index);

        private void comboBoxSelectStyle_SelectedIndexChanged(object sender, EventArgs e) => WallpaperData.WallpaperManagerForm.UpdateWallpaperStyle(Index);
    }
}
