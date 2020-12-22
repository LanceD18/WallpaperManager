namespace WallpaperManager.ImageSelector
{
    partial class ImageSelectionOptionsForm
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
            this.buttonSelectImagesOfType = new System.Windows.Forms.Button();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.radioButtonUnranked = new System.Windows.Forms.RadioButton();
            this.radioButtonRanked = new System.Windows.Forms.RadioButton();
            this.buttonSelectImages = new System.Windows.Forms.Button();
            this.buttonSelectImagesInFolder = new System.Windows.Forms.Button();
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
            this.buttonSelectActiveImages.Location = new System.Drawing.Point(12, 271);
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
            this.buttonSelectImagesOfRank.Location = new System.Drawing.Point(153, 271);
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
            // buttonSelectImagesOfType
            // 
            this.buttonSelectImagesOfType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelectImagesOfType.Location = new System.Drawing.Point(12, 124);
            this.buttonSelectImagesOfType.Name = "buttonSelectImagesOfType";
            this.buttonSelectImagesOfType.Size = new System.Drawing.Size(135, 25);
            this.buttonSelectImagesOfType.TabIndex = 45;
            this.buttonSelectImagesOfType.Text = "Select Images of Type";
            this.buttonSelectImagesOfType.UseVisualStyleBackColor = true;
            this.buttonSelectImagesOfType.Click += new System.EventHandler(this.buttonSelectImagesOfType_Click);
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.radioButtonAll.Location = new System.Drawing.Point(308, 35);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(36, 17);
            this.radioButtonAll.TabIndex = 46;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "All";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            // 
            // radioButtonUnranked
            // 
            this.radioButtonUnranked.AutoSize = true;
            this.radioButtonUnranked.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.radioButtonUnranked.Location = new System.Drawing.Point(308, 58);
            this.radioButtonUnranked.Name = "radioButtonUnranked";
            this.radioButtonUnranked.Size = new System.Drawing.Size(72, 17);
            this.radioButtonUnranked.TabIndex = 47;
            this.radioButtonUnranked.TabStop = true;
            this.radioButtonUnranked.Text = "Unranked";
            this.radioButtonUnranked.UseVisualStyleBackColor = true;
            // 
            // radioButtonRanked
            // 
            this.radioButtonRanked.AutoSize = true;
            this.radioButtonRanked.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.radioButtonRanked.Location = new System.Drawing.Point(308, 81);
            this.radioButtonRanked.Name = "radioButtonRanked";
            this.radioButtonRanked.Size = new System.Drawing.Size(63, 17);
            this.radioButtonRanked.TabIndex = 48;
            this.radioButtonRanked.TabStop = true;
            this.radioButtonRanked.Text = "Ranked";
            this.radioButtonRanked.UseVisualStyleBackColor = true;
            // 
            // buttonSelectImages
            // 
            this.buttonSelectImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelectImages.Location = new System.Drawing.Point(12, 168);
            this.buttonSelectImages.Name = "buttonSelectImages";
            this.buttonSelectImages.Size = new System.Drawing.Size(135, 25);
            this.buttonSelectImages.TabIndex = 49;
            this.buttonSelectImages.Text = "Select Images";
            this.buttonSelectImages.UseVisualStyleBackColor = true;
            this.buttonSelectImages.Click += new System.EventHandler(this.buttonSelectImages_Click);
            // 
            // buttonSelectImagesInFolder
            // 
            this.buttonSelectImagesInFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelectImagesInFolder.Location = new System.Drawing.Point(12, 199);
            this.buttonSelectImagesInFolder.Name = "buttonSelectImagesInFolder";
            this.buttonSelectImagesInFolder.Size = new System.Drawing.Size(135, 25);
            this.buttonSelectImagesInFolder.TabIndex = 50;
            this.buttonSelectImagesInFolder.Text = "Sel. Images in Folder";
            this.buttonSelectImagesInFolder.UseVisualStyleBackColor = true;
            this.buttonSelectImagesInFolder.Click += new System.EventHandler(this.buttonSelectImagesInFolder_Click);
            // 
            // ImageSelectionOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(449, 308);
            this.Controls.Add(this.buttonSelectImagesInFolder);
            this.Controls.Add(this.buttonSelectImages);
            this.Controls.Add(this.radioButtonRanked);
            this.Controls.Add(this.radioButtonUnranked);
            this.Controls.Add(this.radioButtonAll);
            this.Controls.Add(this.buttonSelectImagesOfType);
            this.Controls.Add(this.checkBoxRandomize);
            this.Controls.Add(this.buttonSelectImagesOfRank);
            this.Controls.Add(this.buttonSelectActiveImages);
            this.Controls.Add(this.buttonSelectRankedInFolder);
            this.Controls.Add(this.buttonSelectUnrankedinFolder);
            this.Controls.Add(this.buttonSelectRankedImages);
            this.Controls.Add(this.buttonSelectUnrankedImages);
            this.Name = "ImageSelectionOptionsForm";
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
        private System.Windows.Forms.Button buttonSelectImagesOfType;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.RadioButton radioButtonUnranked;
        private System.Windows.Forms.RadioButton radioButtonRanked;
        private System.Windows.Forms.Button buttonSelectImages;
        private System.Windows.Forms.Button buttonSelectImagesInFolder;
    }
}