namespace WallpaperManager.Options
{
    partial class RankDistributionChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartRankDistribution = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.labelImageCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartRankDistribution)).BeginInit();
            this.SuspendLayout();
            // 
            // chartRankDistribution
            // 
            chartArea1.AxisX.InterlacedColor = System.Drawing.Color.Black;
            chartArea1.AxisX.Title = "Rank";
            chartArea1.AxisY.Title = "Image Count";
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            this.chartRankDistribution.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartRankDistribution.Legends.Add(legend1);
            this.chartRankDistribution.Location = new System.Drawing.Point(1, 1);
            this.chartRankDistribution.Name = "chartRankDistribution";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Ranks";
            series1.YValuesPerPoint = 2;
            this.chartRankDistribution.Series.Add(series1);
            this.chartRankDistribution.Size = new System.Drawing.Size(800, 451);
            this.chartRankDistribution.TabIndex = 0;
            this.chartRankDistribution.Tag = "";
            this.chartRankDistribution.Text = "chart1";
            // 
            // labelImageCount
            // 
            this.labelImageCount.AutoSize = true;
            this.labelImageCount.BackColor = System.Drawing.Color.White;
            this.labelImageCount.ForeColor = System.Drawing.Color.Black;
            this.labelImageCount.Location = new System.Drawing.Point(12, 428);
            this.labelImageCount.Name = "labelImageCount";
            this.labelImageCount.Size = new System.Drawing.Size(67, 13);
            this.labelImageCount.TabIndex = 1;
            this.labelImageCount.Text = "Image Count";
            // 
            // RankDistributionChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelImageCount);
            this.Controls.Add(this.chartRankDistribution);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RankDistributionChart";
            this.Text = "RankDistributionGraph";
            ((System.ComponentModel.ISupportInitialize)(this.chartRankDistribution)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartRankDistribution;
        private System.Windows.Forms.Label labelImageCount;
    }
}