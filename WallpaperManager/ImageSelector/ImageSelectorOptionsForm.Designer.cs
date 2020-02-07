namespace WallpaperManager.ImageSelector
{
    partial class ImageSelectorOptionsForm
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
            this.buttonSelectRankedInFolder = new System.Windows.Forms.Button();
            this.buttonSelectUnrankedinFolder = new System.Windows.Forms.Button();
            this.buttonSelectRankedImages = new System.Windows.Forms.Button();
            this.buttonSelectUnrankedImages = new System.Windows.Forms.Button();
            this.buttonSelectActiveImages = new System.Windows.Forms.Button();
            this.buttonSelectImagesOfRank = new System.Windows.Forms.Button();
            this.checkBoxRandomize = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonSelectRankedInFolder
            // 
            this.buttonSelectRankedInFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelectRankedInFolder.Location = new System.Drawing.Point(152, 64);
            this.buttonSelectRankedInFolder.Name = "buttonSelectRankedInFolder";
            this.buttonSelectRankedInFolder.Size = new System.Drawing.Size(135, 25);
            this.buttonSelectRankedInFolder.TabIndex = 41;
            this.buttonSelectRankedInFolder.Text = "Sel. Ranked in Folder";
            this.buttonSelectRankedInFolder.UseVisualStyleBackColor = true;
            this.buttonSelectRankedInFolder.Click += new System.EventHandler(this.buttonSelectRankedInFolder_Click);
            // 
            // buttonSelectUnrankedinFolder
            // 
            this.buttonSelectUnrankedinFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelectUnrankedinFolder.Location = new System.Drawing.Point(12, 64);
            this.buttonSelectUnrankedinFolder.Name = "buttonSelectUnrankedinFolder";
            this.buttonSelectUnrankedinFolder.Size = new System.Drawing.Size(135, 25);
            this.buttonSelectUnrankedinFolder.TabIndex = 40;
            this.buttonSelectUnrankedinFolder.Text = "Sel. Unranked in Folder";
            this.buttonSelectUnrankedinFolder.UseVisualStyleBackColor = true;
            this.buttonSelectUnrankedinFolder.Click += new System.EventHandler(this.buttonSelectUnrankedinFolder_Click);
            // 
            // buttonSelectRankedImages
            // 
            this.buttonSelectRankedImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelectRankedImages.Location = new System.Drawing.Point(152, 35);
            this.buttonSelectRankedImages.Name = "buttonSelectRankedImages";
            this.buttonSelectRankedImages.Size = new System.Drawing.Size(135, 25);
            this.buttonSelectRankedImages.TabIndex = 39;
            this.buttonSelectRankedImages.Text = "Select Ranked Images";
            this.buttonSelectRankedImages.UseVisualStyleBackColor = true;
            this.buttonSelectRankedImages.Click += new System.EventHandler(this.buttonSelectRankedImages_Click);
            // 
            // buttonSelectUnrankedImages
            // 
            this.buttonSelectUnrankedImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelectUnrankedImages.Location = new System.Drawing.Point(12, 35);
            this.buttonSelectUnrankedImages.Name = "buttonSelectUnrankedImages";
            this.buttonSelectUnrankedImages.Size = new System.Drawing.Size(135, 25);
            this.buttonSelectUnrankedImages.TabIndex = 38;
            this.buttonSelectUnrankedImages.Text = "Select Unranked Images";
            this.buttonSelectUnrankedImages.UseVisualStyleBackColor = true;
            this.buttonSelectUnrankedImages.Click += new System.EventHandler(this.buttonSelectUnrankedImages_Click);
            // 
            // buttonSelectActiveImages
            // 
            this.buttonSelectActiveImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelectActiveImages.Location = new System.Drawing.Point(12, 93);
            this.buttonSelectActiveImages.Name = "buttonSelectActiveImages";
            this.buttonSelectActiveImages.Size = new System.Drawing.Size(135, 25);
            this.buttonSelectActiveImages.TabIndex = 42;
            this.buttonSelectActiveImages.Text = "Select Active Images";
            this.buttonSelectActiveImages.UseVisualStyleBackColor = true;
            this.buttonSelectActiveImages.Click += new System.EventHandler(this.buttonSelectActiveImages_Click);
            // 
            // buttonSelectImagesOfRank
            // 
            this.buttonSelectImagesOfRank.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelectImagesOfRank.Location = new System.Drawing.Point(152, 93);
            this.buttonSelectImagesOfRank.Name = "buttonSelectImagesOfRank";
            this.buttonSelectImagesOfRank.Size = new System.Drawing.Size(135, 25);
            this.buttonSelectImagesOfRank.TabIndex = 43;
            this.buttonSelectImagesOfRank.Text = "Select Images of Rank";
            this.buttonSelectImagesOfRank.UseVisualStyleBackColor = true;
            this.buttonSelectImagesOfRank.Click += new System.EventHandler(this.buttonSelectImagesOfRank_Click);
            // 
            // checkBoxRandomize
            // 
            this.checkBoxRandomize.AutoSize = true;
            this.checkBoxRandomize.ForeColor = System.Drawing.Color.White;
            this.checkBoxRandomize.Location = new System.Drawing.Point(104, 12);
            this.checkBoxRandomize.Name = "checkBoxRandomize";
            this.checkBoxRandomize.Size = new System.Drawing.Size(85, 17);
            this.checkBoxRandomize.TabIndex = 44;
            this.checkBoxRandomize.Text = "Randomize?";
            this.checkBoxRandomize.UseVisualStyleBackColor = true;
            this.checkBoxRandomize.CheckedChanged += new System.EventHandler(this.checkBoxRandomize_CheckedChanged);
            // 
            // ImageSelectorOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(298, 130);
            this.Controls.Add(this.checkBoxRandomize);
            this.Controls.Add(this.buttonSelectImagesOfRank);
            this.Controls.Add(this.buttonSelectActiveImages);
            this.Controls.Add(this.buttonSelectRankedInFolder);
            this.Controls.Add(this.buttonSelectUnrankedinFolder);
            this.Controls.Add(this.buttonSelectRankedImages);
            this.Controls.Add(this.buttonSelectUnrankedImages);
            this.Name = "ImageSelectorOptionsForm";
            this.Text = "Image Selector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectRankedInFolder;
        private System.Windows.Forms.Button buttonSelectUnrankedinFolder;
        private System.Windows.Forms.Button buttonSelectRankedImages;
        private System.Windows.Forms.Button buttonSelectUnrankedImages;
        private System.Windows.Forms.Button buttonSelectImagesOfRank;
        private System.Windows.Forms.CheckBox checkBoxRandomize;
        private System.Windows.Forms.Button buttonSelectActiveImages;
    }
}