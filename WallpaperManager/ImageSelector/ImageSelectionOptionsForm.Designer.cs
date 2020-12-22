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
            this.buttonSelectActiveImages = new System.Windows.Forms.Button();
            this.buttonSelectImagesOfRank = new System.Windows.Forms.Button();
            this.checkBoxRandomize = new System.Windows.Forms.CheckBox();
            this.buttonSelectImagesOfType = new System.Windows.Forms.Button();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.radioButtonUnranked = new System.Windows.Forms.RadioButton();
            this.radioButtonRanked = new System.Windows.Forms.RadioButton();
            this.buttonSelectImages = new System.Windows.Forms.Button();
            this.buttonSelectImagesInFolder = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSelectActiveImages
            // 
            this.buttonSelectActiveImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelectActiveImages.Location = new System.Drawing.Point(12, 138);
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
            this.buttonSelectImagesOfRank.Location = new System.Drawing.Point(153, 138);
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
            this.buttonSelectImagesOfType.Location = new System.Drawing.Point(3, 34);
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
            this.radioButtonAll.Location = new System.Drawing.Point(179, 15);
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
            this.radioButtonUnranked.Location = new System.Drawing.Point(179, 38);
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
            this.radioButtonRanked.Location = new System.Drawing.Point(179, 61);
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
            this.buttonSelectImages.Location = new System.Drawing.Point(3, 3);
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
            this.buttonSelectImagesInFolder.Location = new System.Drawing.Point(3, 65);
            this.buttonSelectImagesInFolder.Name = "buttonSelectImagesInFolder";
            this.buttonSelectImagesInFolder.Size = new System.Drawing.Size(135, 25);
            this.buttonSelectImagesInFolder.TabIndex = 50;
            this.buttonSelectImagesInFolder.Text = "Sel. Images in Folder";
            this.buttonSelectImagesInFolder.UseVisualStyleBackColor = true;
            this.buttonSelectImagesInFolder.Click += new System.EventHandler(this.buttonSelectImagesInFolder_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonSelectImagesOfType);
            this.panel1.Controls.Add(this.radioButtonRanked);
            this.panel1.Controls.Add(this.buttonSelectImagesInFolder);
            this.panel1.Controls.Add(this.radioButtonUnranked);
            this.panel1.Controls.Add(this.buttonSelectImages);
            this.panel1.Controls.Add(this.radioButtonAll);
            this.panel1.Location = new System.Drawing.Point(12, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(276, 97);
            this.panel1.TabIndex = 51;
            // 
            // ImageSelectionOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(304, 179);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.checkBoxRandomize);
            this.Controls.Add(this.buttonSelectImagesOfRank);
            this.Controls.Add(this.buttonSelectActiveImages);
            this.Name = "ImageSelectionOptionsForm";
            this.Text = "Image Selector";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonSelectImagesOfRank;
        private System.Windows.Forms.CheckBox checkBoxRandomize;
        private System.Windows.Forms.Button buttonSelectActiveImages;
        private System.Windows.Forms.Button buttonSelectImagesOfType;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.RadioButton radioButtonUnranked;
        private System.Windows.Forms.RadioButton radioButtonRanked;
        private System.Windows.Forms.Button buttonSelectImages;
        private System.Windows.Forms.Button buttonSelectImagesInFolder;
        private System.Windows.Forms.Panel panel1;
    }
}