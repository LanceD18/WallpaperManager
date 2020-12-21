namespace WallpaperManager
{
    partial class WallpaperForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WallpaperForm));
            this.pictureBoxWallpaper = new System.Windows.Forms.PictureBox();
            this.axWindowsMediaPlayerWallpaper = new AxWMPLib.AxWindowsMediaPlayer();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWallpaper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayerWallpaper)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxWallpaper
            // 
            this.pictureBoxWallpaper.Location = new System.Drawing.Point(117, 94);
            this.pictureBoxWallpaper.Name = "pictureBoxWallpaper";
            this.pictureBoxWallpaper.Size = new System.Drawing.Size(169, 140);
            this.pictureBoxWallpaper.TabIndex = 0;
            this.pictureBoxWallpaper.TabStop = false;
            // 
            // axWindowsMediaPlayerWallpaper
            // 
            this.axWindowsMediaPlayerWallpaper.Enabled = true;
            this.axWindowsMediaPlayerWallpaper.Location = new System.Drawing.Point(415, 228);
            this.axWindowsMediaPlayerWallpaper.Name = "axWindowsMediaPlayerWallpaper";
            this.axWindowsMediaPlayerWallpaper.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayerWallpaper.OcxState")));
            this.axWindowsMediaPlayerWallpaper.Size = new System.Drawing.Size(343, 210);
            this.axWindowsMediaPlayerWallpaper.TabIndex = 1;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // WallpaperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.axWindowsMediaPlayerWallpaper);
            this.Controls.Add(this.pictureBoxWallpaper);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WallpaperForm";
            this.Text = "WallpaperForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWallpaper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayerWallpaper)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxWallpaper;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayerWallpaper;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
    }
}