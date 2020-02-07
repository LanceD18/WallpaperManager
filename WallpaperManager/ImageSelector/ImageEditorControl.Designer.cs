namespace WallpaperManager.ImageSelector
{
    partial class ImageEditorControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageEditorControl));
            this.buttonEditTags = new System.Windows.Forms.Button();
            this.buttonMinus = new System.Windows.Forms.Button();
            this.textBoxRankEditor = new System.Windows.Forms.TextBox();
            this.buttonPlus = new System.Windows.Forms.Button();
            this.buttonSelectImage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonEditTags
            // 
            this.buttonEditTags.Location = new System.Drawing.Point(63, 236);
            this.buttonEditTags.Name = "buttonEditTags";
            this.buttonEditTags.Size = new System.Drawing.Size(75, 20);
            this.buttonEditTags.TabIndex = 2;
            this.buttonEditTags.Text = "Edit Tags";
            this.buttonEditTags.UseVisualStyleBackColor = true;
            this.buttonEditTags.Click += new System.EventHandler(this.buttonEditImage_Click);
            // 
            // buttonMinus
            // 
            this.buttonMinus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMinus.BackgroundImage")));
            this.buttonMinus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMinus.Location = new System.Drawing.Point(47, 258);
            this.buttonMinus.Name = "buttonMinus";
            this.buttonMinus.Size = new System.Drawing.Size(20, 20);
            this.buttonMinus.TabIndex = 3;
            this.buttonMinus.UseVisualStyleBackColor = true;
            // 
            // textBoxRankEditor
            // 
            this.textBoxRankEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textBoxRankEditor.ForeColor = System.Drawing.Color.White;
            this.textBoxRankEditor.Location = new System.Drawing.Point(75, 258);
            this.textBoxRankEditor.Name = "textBoxRankEditor";
            this.textBoxRankEditor.Size = new System.Drawing.Size(50, 20);
            this.textBoxRankEditor.TabIndex = 4;
            this.textBoxRankEditor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonPlus
            // 
            this.buttonPlus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPlus.BackgroundImage")));
            this.buttonPlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPlus.Location = new System.Drawing.Point(132, 258);
            this.buttonPlus.Name = "buttonPlus";
            this.buttonPlus.Size = new System.Drawing.Size(20, 20);
            this.buttonPlus.TabIndex = 5;
            this.buttonPlus.UseVisualStyleBackColor = true;
            // 
            // buttonSelectImage
            // 
            this.buttonSelectImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonSelectImage.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.buttonSelectImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectImage.Location = new System.Drawing.Point(0, 0);
            this.buttonSelectImage.Name = "buttonSelectImage";
            this.buttonSelectImage.Size = new System.Drawing.Size(200, 230);
            this.buttonSelectImage.TabIndex = 6;
            this.buttonSelectImage.UseVisualStyleBackColor = true;
            // 
            // ImageEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.buttonSelectImage);
            this.Controls.Add(this.buttonPlus);
            this.Controls.Add(this.textBoxRankEditor);
            this.Controls.Add(this.buttonMinus);
            this.Controls.Add(this.buttonEditTags);
            this.Name = "ImageEditorControl";
            this.Size = new System.Drawing.Size(200, 280);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonEditTags;
        private System.Windows.Forms.Button buttonMinus;
        private System.Windows.Forms.TextBox textBoxRankEditor;
        private System.Windows.Forms.Button buttonPlus;
        private System.Windows.Forms.Button buttonSelectImage;
    }
}
