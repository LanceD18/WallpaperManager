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
            this.components = new System.ComponentModel.Container();
            this.checkBoxLargerImagesOnLargerMonitors = new System.Windows.Forms.CheckBox();
            this.checkBoxHigherRankedImagesOnLargerMonitors = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableDetectionOfInactiveImages = new System.Windows.Forms.CheckBox();
            this.buttonInspectRankDistribution = new System.Windows.Forms.Button();
            this.buttonModifyMaxRank = new System.Windows.Forms.Button();
            this.labelDefaultTheme = new System.Windows.Forms.Label();
            this.labelDefaultThemePath = new System.Windows.Forms.Label();
            this.buttonSetDefaultTheme = new System.Windows.Forms.Button();
            this.buttonLoadDefaultTheme = new System.Windows.Forms.Button();
            this.checkBoxEnableGlobalHotkey = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxWeightedRanks = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            // labelDefaultTheme
            // 
            this.labelDefaultTheme.AutoSize = true;
            this.labelDefaultTheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultTheme.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.labelDefaultTheme.Location = new System.Drawing.Point(9, 113);
            this.labelDefaultTheme.Name = "labelDefaultTheme";
            this.labelDefaultTheme.Size = new System.Drawing.Size(113, 16);
            this.labelDefaultTheme.TabIndex = 44;
            this.labelDefaultTheme.Text = "Default Theme:";
            // 
            // labelDefaultThemePath
            // 
            this.labelDefaultThemePath.AutoSize = true;
            this.labelDefaultThemePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultThemePath.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.labelDefaultThemePath.Location = new System.Drawing.Point(9, 139);
            this.labelDefaultThemePath.Name = "labelDefaultThemePath";
            this.labelDefaultThemePath.Size = new System.Drawing.Size(94, 13);
            this.labelDefaultThemePath.TabIndex = 45;
            this.labelDefaultThemePath.Text = "defaultThemePath";
            // 
            // buttonSetDefaultTheme
            // 
            this.buttonSetDefaultTheme.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSetDefaultTheme.Location = new System.Drawing.Point(246, 110);
            this.buttonSetDefaultTheme.Name = "buttonSetDefaultTheme";
            this.buttonSetDefaultTheme.Size = new System.Drawing.Size(112, 23);
            this.buttonSetDefaultTheme.TabIndex = 46;
            this.buttonSetDefaultTheme.Text = "Set Default Theme";
            this.buttonSetDefaultTheme.UseVisualStyleBackColor = true;
            this.buttonSetDefaultTheme.Click += new System.EventHandler(this.buttonSetDefaultTheme_Click);
            // 
            // buttonLoadDefaultTheme
            // 
            this.buttonLoadDefaultTheme.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonLoadDefaultTheme.Location = new System.Drawing.Point(128, 110);
            this.buttonLoadDefaultTheme.Name = "buttonLoadDefaultTheme";
            this.buttonLoadDefaultTheme.Size = new System.Drawing.Size(112, 23);
            this.buttonLoadDefaultTheme.TabIndex = 47;
            this.buttonLoadDefaultTheme.Text = "Load Default Theme";
            this.buttonLoadDefaultTheme.UseVisualStyleBackColor = true;
            this.buttonLoadDefaultTheme.Click += new System.EventHandler(this.buttonLoadDefaultTheme_Click);
            // 
            // checkBoxEnableGlobalHotkey
            // 
            this.checkBoxEnableGlobalHotkey.AutoSize = true;
            this.checkBoxEnableGlobalHotkey.Checked = true;
            this.checkBoxEnableGlobalHotkey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableGlobalHotkey.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxEnableGlobalHotkey.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxEnableGlobalHotkey.Location = new System.Drawing.Point(12, 155);
            this.checkBoxEnableGlobalHotkey.Name = "checkBoxEnableGlobalHotkey";
            this.checkBoxEnableGlobalHotkey.Size = new System.Drawing.Size(250, 17);
            this.checkBoxEnableGlobalHotkey.TabIndex = 48;
            this.checkBoxEnableGlobalHotkey.Text = "Enable Default Theme Global Hotkey (Alt+Shift)";
            this.checkBoxEnableGlobalHotkey.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(9, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(410, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "(Your Active Theme will be automatically saved upon swapping to the Default Theme" +
    ")";
            // 
            // checkBoxWeightedRanks
            // 
            this.checkBoxWeightedRanks.AutoSize = true;
            this.checkBoxWeightedRanks.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxWeightedRanks.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxWeightedRanks.Location = new System.Drawing.Point(12, 81);
            this.checkBoxWeightedRanks.Name = "checkBoxWeightedRanks";
            this.checkBoxWeightedRanks.Size = new System.Drawing.Size(106, 17);
            this.checkBoxWeightedRanks.TabIndex = 50;
            this.checkBoxWeightedRanks.Text = "Weighted Ranks";
            this.checkBoxWeightedRanks.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(484, 450);
            this.Controls.Add(this.checkBoxWeightedRanks);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxEnableGlobalHotkey);
            this.Controls.Add(this.buttonLoadDefaultTheme);
            this.Controls.Add(this.buttonSetDefaultTheme);
            this.Controls.Add(this.labelDefaultThemePath);
            this.Controls.Add(this.labelDefaultTheme);
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
        private System.Windows.Forms.Label labelDefaultTheme;
        private System.Windows.Forms.Label labelDefaultThemePath;
        private System.Windows.Forms.Button buttonSetDefaultTheme;
        private System.Windows.Forms.Button buttonLoadDefaultTheme;
        private System.Windows.Forms.CheckBox checkBoxEnableGlobalHotkey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxWeightedRanks;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}