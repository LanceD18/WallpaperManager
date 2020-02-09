using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            for (int i = 1; i <= maxRank; i++)
            {
                chartRankDistribution.Series[0].Points.Add(new DataPoint(i, WallpaperData.GetImagesOfRank(i).Length));
            }

            // TODO Make this scale better visually with the max rank
            /*
            if (maxRank == 100)
            {
                chartRankDistribution.ChartAreas[0].AxisX.Interval = 5;
            }
            */
        }
    }
}
