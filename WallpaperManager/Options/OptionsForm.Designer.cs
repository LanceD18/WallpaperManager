namespace WallpaperManager.Options
{
    partial class OptionsForm
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
            this.checkBoxLargerImagesOnLargerMonitors = new System.Windows.Forms.CheckBox();
            this.checkBoxHigherRankedImagesOnLargerMonitors = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableDetectionOfInactiveImages = new System.Windows.Forms.CheckBox();
            this.buttonInspectRankDistribution = new System.Windows.Forms.Button();
            this.buttonModifyMaxRank = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBoxLargerImagesOnLargerMonitors
            // 
            this.checkBoxLargerImagesOnLargerMonitors.AutoSize = true;
            this.checkBoxLargerImagesOnLargerMonitors.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxLargerImagesOnLargerMonitors.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxLargerImagesOnLargerMonitors.Location = new System.Drawing.Point(12, 12);
            this.checkBoxLargerImagesOnLargerMonitors.Name = "checkBoxLargerImagesOnLargerMonitors";
            this.checkBoxLargerImagesOnLargerMonitors.Size = new System.Drawing.Size(184, 17);
            this.checkBoxLargerImagesOnLargerMonitors.TabIndex = 36;
            this.checkBoxLargerImagesOnLargerMonitors.Text = "Larger Images on Larger Monitors";
            this.checkBoxLargerImagesOnLargerMonitors.UseVisualStyleBackColor = true;
            // 
            // checkBoxHigherRankedImagesOnLargerMonitors
            // 
            this.checkBoxHigherRankedImagesOnLargerMonitors.AutoSize = true;
            this.checkBoxHigherRankedImagesOnLargerMonitors.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxHigherRankedImagesOnLargerMonitors.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxHigherRankedImagesOnLargerMonitors.Location = new System.Drawing.Point(12, 35);
            this.checkBoxHigherRankedImagesOnLargerMonitors.Name = "checkBoxHigherRankedImagesOnLargerMonitors";
            this.checkBoxHigherRankedImagesOnLargerMonitors.Size = new System.Drawing.Size(226, 17);
            this.checkBoxHigherRankedImagesOnLargerMonitors.TabIndex = 37;
            this.checkBoxHigherRankedImagesOnLargerMonitors.Text = "Higher Ranked Images on Larger Monitors";
            this.checkBoxHigherRankedImagesOnLargerMonitors.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableDetectionOfInactiveImages
            // 
            this.checkBoxEnableDetectionOfInactiveImages.AutoSize = true;
            this.checkBoxEnableDetectionOfInactiveImages.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxEnableDetectionOfInactiveImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxEnableDetectionOfInactiveImages.Location = new System.Drawing.Point(12, 58);
            this.checkBoxEnableDetectionOfInactiveImages.Name = "checkBoxEnableDetectionOfInactiveImages";
            this.checkBoxEnableDetectionOfInactiveImages.Size = new System.Drawing.Size(198, 17);
            this.checkBoxEnableDetectionOfInactiveImages.TabIndex = 40;
            this.checkBoxEnableDetectionOfInactiveImages.Text = "Enable Detection of Inactive Images";
            this.checkBoxEnableDetectionOfInactiveImages.UseVisualStyleBackColor = true;
            // 
            // buttonInspectRankDistribution
            // 
            this.buttonInspectRankDistribution.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonInspectRankDistribution.Location = new System.Drawing.Point(338, 12);
            this.buttonInspectRankDistribution.Name = "buttonInspectRankDistribution";
            this.buttonInspectRankDistribution.Size = new System.Drawing.Size(134, 23);
            this.buttonInspectRankDistribution.TabIndex = 41;
            this.buttonInspectRankDistribution.Text = "Inspect Rank Distribution";
            this.buttonInspectRankDistribution.UseVisualStyleBackColor = true;
            this.buttonInspectRankDistribution.Click += new System.EventHandler(this.buttonInspectRankDistribution_Click);
            // 
            // buttonModifyMaxRank
            // 
            this.buttonModifyMaxRank.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonModifyMaxRank.Location = new System.Drawing.Point(338, 41);
            this.buttonModifyMaxRank.Name = "buttonModifyMaxRank";
            this.buttonModifyMaxRank.Size = new System.Drawing.Size(134, 23);
            this.buttonModifyMaxRank.TabIndex = 42;
            this.buttonModifyMaxRank.Text = "Modify Max Rank";
            this.buttonModifyMaxRank.UseVisualStyleBackColor = true;
            this.buttonModifyMaxRank.Click += new System.EventHandler(this.buttonModifyMaxRank_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(484, 450);
            this.Controls.Add(this.buttonModifyMaxRank);
            this.Controls.Add(this.buttonInspectRankDistribution);
            this.Controls.Add(this.checkBoxEnableDetectionOfInactiveImages);
            this.Controls.Add(this.checkBoxHigherRankedImagesOnLargerMonitors);
            this.Controls.Add(this.checkBoxLargerImagesOnLargerMonitors);
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxLargerImagesOnLargerMonitors;
        private System.Windows.Forms.CheckBox checkBoxHigherRankedImagesOnLargerMonitors;
        private System.Windows.Forms.CheckBox checkBoxEnableDetectionOfInactiveImages;
        private System.Windows.Forms.Button buttonInspectRankDistribution;
        private System.Windows.Forms.Button buttonModifyMaxRank;
    }
}