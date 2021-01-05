namespace WallpaperManager.Controls
{
    partial class WallpaperManagerDisplaySettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelWallpaperStyle = new System.Windows.Forms.Label();
            this.comboBoxSelectStyle = new System.Windows.Forms.ComboBox();
            this.comboBoxWallpaperInterval = new System.Windows.Forms.ComboBox();
            this.labelTimeLeft = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelWallpaperStyle
            // 
            this.labelWallpaperStyle.AutoSize = true;
            this.labelWallpaperStyle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelWallpaperStyle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelWallpaperStyle.Location = new System.Drawing.Point(12, 15);
            this.labelWallpaperStyle.Name = "labelWallpaperStyle";
            this.labelWallpaperStyle.Size = new System.Drawing.Size(81, 13);
            this.labelWallpaperStyle.TabIndex = 41;
            this.labelWallpaperStyle.Text = "Wallpaper Style";
            // 
            // comboBoxSelectStyle
            // 
            this.comboBoxSelectStyle.DisplayMember = "Sel";
            this.comboBoxSelectStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelectStyle.FormattingEnabled = true;
            this.comboBoxSelectStyle.Items.AddRange(new object[] {
            "Fill",
            "Stretch",
            "Zoom"});
            this.comboBoxSelectStyle.Location = new System.Drawing.Point(15, 32);
            this.comboBoxSelectStyle.Name = "comboBoxSelectStyle";
            this.comboBoxSelectStyle.Size = new System.Drawing.Size(80, 21);
            this.comboBoxSelectStyle.TabIndex = 38;
            this.comboBoxSelectStyle.Tag = "";
            this.comboBoxSelectStyle.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectStyle_SelectedIndexChanged);
            // 
            // comboBoxWallpaperInterval
            // 
            this.comboBoxWallpaperInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWallpaperInterval.FormattingEnabled = true;
            this.comboBoxWallpaperInterval.Items.AddRange(new object[] {
            "None",
            "5 sec",
            "10 sec",
            "15 sec",
            "30 sec",
            "45 sec",
            "1 min",
            "2 min",
            "3 min",
            "5 min",
            "10 min",
            "15 min",
            "30 min",
            "45 min",
            "1 hour",
            "2 hours",
            "3 hours",
            "5 hours",
            "6 hours",
            "8 hours",
            "10 hours",
            "12 hours",
            "16 hours",
            "20 hours",
            "1 day",
            "3 days",
            "1 week"});
            this.comboBoxWallpaperInterval.Location = new System.Drawing.Point(104, 32);
            this.comboBoxWallpaperInterval.Name = "comboBoxWallpaperInterval";
            this.comboBoxWallpaperInterval.Size = new System.Drawing.Size(80, 21);
            this.comboBoxWallpaperInterval.TabIndex = 39;
            this.comboBoxWallpaperInterval.SelectedIndexChanged += new System.EventHandler(this.comboBoxWallpaperInterval_SelectedIndexChanged);
            // 
            // labelTimeLeft
            // 
            this.labelTimeLeft.AutoSize = true;
            this.labelTimeLeft.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelTimeLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelTimeLeft.Location = new System.Drawing.Point(101, 15);
            this.labelTimeLeft.Name = "labelTimeLeft";
            this.labelTimeLeft.Size = new System.Drawing.Size(76, 13);
            this.labelTimeLeft.TabIndex = 40;
            this.labelTimeLeft.Text = "Awaiting Timer";
            // 
            // WallpaperManagerDisplaySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.Controls.Add(this.labelWallpaperStyle);
            this.Controls.Add(this.comboBoxSelectStyle);
            this.Controls.Add(this.comboBoxWallpaperInterval);
            this.Controls.Add(this.labelTimeLeft);
            this.Name = "WallpaperManagerDisplaySettings";
            this.Size = new System.Drawing.Size(199, 67);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWallpaperStyle;
        private System.Windows.Forms.ComboBox comboBoxSelectStyle;
        private System.Windows.Forms.ComboBox comboBoxWallpaperInterval;
        private System.Windows.Forms.Label labelTimeLeft;
    }
}
