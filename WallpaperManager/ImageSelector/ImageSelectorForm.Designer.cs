namespace WallpaperManager.ImageSelector
{
    partial class ImageSelectorForm
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
            this.flowLayoutPanelImageContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControlImagePages = new System.Windows.Forms.TabControl();
            this.labelSelectedImage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flowLayoutPanelImageContainer
            // 
            this.flowLayoutPanelImageContainer.AutoScroll = true;
            this.flowLayoutPanelImageContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.flowLayoutPanelImageContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanelImageContainer.Location = new System.Drawing.Point(0, 61);
            this.flowLayoutPanelImageContainer.Name = "flowLayoutPanelImageContainer";
            this.flowLayoutPanelImageContainer.Size = new System.Drawing.Size(800, 391);
            this.flowLayoutPanelImageContainer.TabIndex = 0;
            this.flowLayoutPanelImageContainer.Visible = false;
            // 
            // tabControlImagePages
            // 
            this.tabControlImagePages.ItemSize = new System.Drawing.Size(20, 18);
            this.tabControlImagePages.Location = new System.Drawing.Point(0, 61);
            this.tabControlImagePages.Name = "tabControlImagePages";
            this.tabControlImagePages.SelectedIndex = 0;
            this.tabControlImagePages.Size = new System.Drawing.Size(800, 390);
            this.tabControlImagePages.TabIndex = 1;
            // 
            // labelSelectedImage
            // 
            this.labelSelectedImage.AutoSize = true;
            this.labelSelectedImage.ForeColor = System.Drawing.Color.White;
            this.labelSelectedImage.Location = new System.Drawing.Point(12, 9);
            this.labelSelectedImage.Name = "labelSelectedImage";
            this.labelSelectedImage.Size = new System.Drawing.Size(144, 13);
            this.labelSelectedImage.TabIndex = 2;
            this.labelSelectedImage.Text = "Select an image for more info";
            // 
            // ImageSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelSelectedImage);
            this.Controls.Add(this.tabControlImagePages);
            this.Controls.Add(this.flowLayoutPanelImageContainer);
            this.Name = "ImageSelectorForm";
            this.Text = "ImageSelectorForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelImageContainer;
        private System.Windows.Forms.TabControl tabControlImagePages;
        private System.Windows.Forms.Label labelSelectedImage;
    }
}