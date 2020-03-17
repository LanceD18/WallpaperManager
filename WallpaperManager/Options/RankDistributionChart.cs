using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Options
{
    public partial class RankDistributionChart : Form
    {
        public RankDistributionChart()
        {
            InitializeComponent();

            int maxRank = WallpaperData.GetMaxRank();

            // Adds each rank's image count to the graph
            SuspendLayout();
            chartRankDistribution.ChartAreas[0].AxisX.Interval = maxRank / 20;
            chartRankDistribution.ChartAreas[0].AxisX.Maximum = maxRank + 0.5; // this offsets the graph's visuals otherwise only half of the last bar would appear
            for (int i = 1; i <= maxRank; i++)
            {
                chartRankDistribution.Series[0].Points.Add(new DataPoint(i, WallpaperData.GetImagesOfRank(i).Length));
            }

            labelImageCount.Text = "Ranked Images: " + WallpaperData.GetAllRankedImages().Length + " | Unranked Images: " + (WallpaperData.GetAllImageData().Length - WallpaperData.GetAllRankedImages().Length);
            ResumeLayout();
        }
    }
}
