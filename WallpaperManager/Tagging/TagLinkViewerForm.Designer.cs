namespace WallpaperManager.Tagging
{
    partial class TagLinkViewerForm
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
            this.linkedTagsFLP = new System.Windows.Forms.FlowLayoutPanel();
            this.labelXTags = new System.Windows.Forms.Label();
            this.labelRemovalReminder = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // linkedTagsFLP
            // 
            this.linkedTagsFLP.Location = new System.Drawing.Point(0, 46);
            this.linkedTagsFLP.Name = "linkedTagsFLP";
            this.linkedTagsFLP.Size = new System.Drawing.Size(273, 290);
            this.linkedTagsFLP.TabIndex = 0;
            // 
            // labelXTags
            // 
            this.labelXTags.AutoSize = true;
            this.labelXTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXTags.ForeColor = System.Drawing.SystemColors.Control;
            this.labelXTags.Location = new System.Drawing.Point(12, 9);
            this.labelXTags.Name = "labelXTags";
            this.labelXTags.Size = new System.Drawing.Size(56, 16);
            this.labelXTags.TabIndex = 1;
            this.labelXTags.Text = "_ Tags";
            // 
            // labelRemovalReminder
            // 
            this.labelRemovalReminder.AutoSize = true;
            this.labelRemovalReminder.ForeColor = System.Drawing.SystemColors.Control;
            this.labelRemovalReminder.Location = new System.Drawing.Point(12, 30);
            this.labelRemovalReminder.Name = "labelRemovalReminder";
            this.labelRemovalReminder.Size = new System.Drawing.Size(129, 13);
            this.labelRemovalReminder.TabIndex = 0;
            this.labelRemovalReminder.Text = "(Click on a tag to unlink it)";
            // 
            // TagLinkViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(273, 336);
            this.Controls.Add(this.labelRemovalReminder);
            this.Controls.Add(this.linkedTagsFLP);
            this.Controls.Add(this.labelXTags);
            this.Name = "TagLinkViewerForm";
            this.Text = "TagLinkViewerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel linkedTagsFLP;
        private System.Windows.Forms.Label labelXTags;
        private System.Windows.Forms.Label labelRemovalReminder;
    }
}