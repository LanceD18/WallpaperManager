namespace WallpaperManager.ImageSelector
{
    partial class ImageEditorForm
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
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.imageTagsFLP = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonAddTag = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelEnabled = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureBoxImage.Location = new System.Drawing.Point(300, 0);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(383, 462);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 0;
            this.pictureBoxImage.TabStop = false;
            // 
            // imageTagsFLP
            // 
            this.imageTagsFLP.Location = new System.Drawing.Point(0, 27);
            this.imageTagsFLP.Name = "imageTagsFLP";
            this.imageTagsFLP.Size = new System.Drawing.Size(300, 393);
            this.imageTagsFLP.TabIndex = 1;
            // 
            // buttonAddTag
            // 
            this.buttonAddTag.Location = new System.Drawing.Point(104, 426);
            this.buttonAddTag.Name = "buttonAddTag";
            this.buttonAddTag.Size = new System.Drawing.Size(75, 23);
            this.buttonAddTag.TabIndex = 2;
            this.buttonAddTag.Text = "Add Tag";
            this.buttonAddTag.UseVisualStyleBackColor = true;
            this.buttonAddTag.Click += new System.EventHandler(this.buttonAddTag_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(270, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Click on a tag to remove it or create a naming exception";
            // 
            // labelEnabled
            // 
            this.labelEnabled.AutoSize = true;
            this.labelEnabled.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnabled.ForeColor = System.Drawing.Color.Lime;
            this.labelEnabled.Location = new System.Drawing.Point(210, 431);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(53, 13);
            this.labelEnabled.TabIndex = 4;
            this.labelEnabled.Text = "Enabled";
            // 
            // ImageEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.labelEnabled);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonAddTag);
            this.Controls.Add(this.imageTagsFLP);
            this.Controls.Add(this.pictureBoxImage);
            this.Name = "ImageEditorForm";
            this.Text = "ImageEditorForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.FlowLayoutPanel imageTagsFLP;
        private System.Windows.Forms.Button buttonAddTag;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelEnabled;
    }
}