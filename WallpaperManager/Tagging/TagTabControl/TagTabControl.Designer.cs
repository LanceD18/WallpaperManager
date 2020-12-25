namespace WallpaperManager.Tagging
{
    partial class TagTabControl
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
            this.tabControlImageTagger = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabControlImageTagger
            // 
            this.tabControlImageTagger.AllowDrop = true;
            this.tabControlImageTagger.Location = new System.Drawing.Point(0, 0);
            this.tabControlImageTagger.Name = "tabControlImageTagger";
            this.tabControlImageTagger.SelectedIndex = 0;
            this.tabControlImageTagger.Size = new System.Drawing.Size(553, 556);
            this.tabControlImageTagger.TabIndex = 2;
            // 
            // Tagging_TabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.tabControlImageTagger);
            this.Name = "Tagging_TabControl";
            this.Size = new System.Drawing.Size(553, 556);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlImageTagger;
    }
}
