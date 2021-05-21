namespace WallpaperManager.Tagging
{
    partial class TagEditorForm
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
            this.buttonAddToSelectedImages = new System.Windows.Forms.Button();
            this.buttonSelectImagesWithTag = new System.Windows.Forms.Button();
            this.buttonDeleteTag = new System.Windows.Forms.Button();
            this.buttomRemoveFromSelectedImages = new System.Windows.Forms.Button();
            this.checkBoxUseForNaming = new System.Windows.Forms.CheckBox();
            this.labelTagName = new System.Windows.Forms.Label();
            this.labelFoundImages = new System.Windows.Forms.Label();
            this.checkBoxEnabled = new System.Windows.Forms.CheckBox();
            this.buttonRenameTag = new System.Windows.Forms.Button();
            this.buttonLink = new System.Windows.Forms.Button();
            this.buttonParentTags = new System.Windows.Forms.Button();
            this.buttonChildTags = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAddToSelectedImages
            // 
            this.buttonAddToSelectedImages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.buttonAddToSelectedImages.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonAddToSelectedImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddToSelectedImages.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonAddToSelectedImages.Location = new System.Drawing.Point(12, 117);
            this.buttonAddToSelectedImages.Name = "buttonAddToSelectedImages";
            this.buttonAddToSelectedImages.Size = new System.Drawing.Size(175, 26);
            this.buttonAddToSelectedImages.TabIndex = 0;
            this.buttonAddToSelectedImages.Text = "Add to Selected Image(s)";
            this.buttonAddToSelectedImages.UseVisualStyleBackColor = false;
            this.buttonAddToSelectedImages.Click += new System.EventHandler(this.buttonAddToSelectedImages_Click);
            // 
            // buttonSelectImagesWithTag
            // 
            this.buttonSelectImagesWithTag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.buttonSelectImagesWithTag.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonSelectImagesWithTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectImagesWithTag.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonSelectImagesWithTag.Location = new System.Drawing.Point(12, 181);
            this.buttonSelectImagesWithTag.Name = "buttonSelectImagesWithTag";
            this.buttonSelectImagesWithTag.Size = new System.Drawing.Size(175, 26);
            this.buttonSelectImagesWithTag.TabIndex = 1;
            this.buttonSelectImagesWithTag.Text = "Select Images with Tag";
            this.buttonSelectImagesWithTag.UseVisualStyleBackColor = false;
            this.buttonSelectImagesWithTag.Click += new System.EventHandler(this.buttonSelectImagesWithTag_Click);
            // 
            // buttonDeleteTag
            // 
            this.buttonDeleteTag.Location = new System.Drawing.Point(139, 221);
            this.buttonDeleteTag.Name = "buttonDeleteTag";
            this.buttonDeleteTag.Size = new System.Drawing.Size(57, 23);
            this.buttonDeleteTag.TabIndex = 2;
            this.buttonDeleteTag.Text = "Delete";
            this.buttonDeleteTag.UseVisualStyleBackColor = true;
            this.buttonDeleteTag.Click += new System.EventHandler(this.buttonDeleteTag_Click);
            // 
            // buttomRemoveFromSelectedImages
            // 
            this.buttomRemoveFromSelectedImages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttomRemoveFromSelectedImages.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttomRemoveFromSelectedImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttomRemoveFromSelectedImages.ForeColor = System.Drawing.SystemColors.Control;
            this.buttomRemoveFromSelectedImages.Location = new System.Drawing.Point(12, 149);
            this.buttomRemoveFromSelectedImages.Name = "buttomRemoveFromSelectedImages";
            this.buttomRemoveFromSelectedImages.Size = new System.Drawing.Size(175, 26);
            this.buttomRemoveFromSelectedImages.TabIndex = 3;
            this.buttomRemoveFromSelectedImages.Text = "Remove From Selected Image(s)";
            this.buttomRemoveFromSelectedImages.UseVisualStyleBackColor = false;
            this.buttomRemoveFromSelectedImages.Click += new System.EventHandler(this.buttomRemoveFromSelectedImages_Click);
            // 
            // checkBoxUseForNaming
            // 
            this.checkBoxUseForNaming.AutoSize = true;
            this.checkBoxUseForNaming.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxUseForNaming.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBoxUseForNaming.Location = new System.Drawing.Point(16, 83);
            this.checkBoxUseForNaming.Name = "checkBoxUseForNaming";
            this.checkBoxUseForNaming.Size = new System.Drawing.Size(119, 19);
            this.checkBoxUseForNaming.TabIndex = 4;
            this.checkBoxUseForNaming.Text = "Use for Naming?";
            this.checkBoxUseForNaming.UseVisualStyleBackColor = true;
            this.checkBoxUseForNaming.CheckedChanged += new System.EventHandler(this.checkBoxUseForNaming_CheckedChanged);
            // 
            // labelTagName
            // 
            this.labelTagName.AutoSize = true;
            this.labelTagName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTagName.ForeColor = System.Drawing.SystemColors.Control;
            this.labelTagName.Location = new System.Drawing.Point(12, 9);
            this.labelTagName.Name = "labelTagName";
            this.labelTagName.Size = new System.Drawing.Size(39, 20);
            this.labelTagName.TabIndex = 5;
            this.labelTagName.Text = "Tag";
            // 
            // labelFoundImages
            // 
            this.labelFoundImages.AutoSize = true;
            this.labelFoundImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFoundImages.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelFoundImages.Location = new System.Drawing.Point(13, 39);
            this.labelFoundImages.Name = "labelFoundImages";
            this.labelFoundImages.Size = new System.Drawing.Size(118, 16);
            this.labelFoundImages.TabIndex = 6;
            this.labelFoundImages.Text = "Found in X images";
            // 
            // checkBoxEnabled
            // 
            this.checkBoxEnabled.AutoSize = true;
            this.checkBoxEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnabled.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBoxEnabled.Location = new System.Drawing.Point(16, 58);
            this.checkBoxEnabled.Name = "checkBoxEnabled";
            this.checkBoxEnabled.Size = new System.Drawing.Size(72, 19);
            this.checkBoxEnabled.TabIndex = 7;
            this.checkBoxEnabled.Text = "Enabled";
            this.checkBoxEnabled.UseVisualStyleBackColor = true;
            this.checkBoxEnabled.CheckedChanged += new System.EventHandler(this.checkBoxEnabled_CheckedChanged);
            // 
            // buttonRenameTag
            // 
            this.buttonRenameTag.Location = new System.Drawing.Point(6, 221);
            this.buttonRenameTag.Name = "buttonRenameTag";
            this.buttonRenameTag.Size = new System.Drawing.Size(56, 23);
            this.buttonRenameTag.TabIndex = 8;
            this.buttonRenameTag.Text = "Rename";
            this.buttonRenameTag.UseVisualStyleBackColor = true;
            this.buttonRenameTag.Click += new System.EventHandler(this.buttonRenameTag_Click);
            // 
            // buttonLink
            // 
            this.buttonLink.Location = new System.Drawing.Point(72, 221);
            this.buttonLink.Name = "buttonLink";
            this.buttonLink.Size = new System.Drawing.Size(56, 23);
            this.buttonLink.TabIndex = 9;
            this.buttonLink.Text = "Link";
            this.buttonLink.UseVisualStyleBackColor = true;
            this.buttonLink.Click += new System.EventHandler(this.buttonLink_Click);
            // 
            // buttonParentTags
            // 
            this.buttonParentTags.Location = new System.Drawing.Point(16, 250);
            this.buttonParentTags.Name = "buttonParentTags";
            this.buttonParentTags.Size = new System.Drawing.Size(82, 23);
            this.buttonParentTags.TabIndex = 10;
            this.buttonParentTags.Text = "Parent Tags";
            this.buttonParentTags.UseVisualStyleBackColor = true;
            this.buttonParentTags.Click += new System.EventHandler(this.buttonParentTags_Click);
            // 
            // buttonChildTags
            // 
            this.buttonChildTags.Location = new System.Drawing.Point(104, 250);
            this.buttonChildTags.Name = "buttonChildTags";
            this.buttonChildTags.Size = new System.Drawing.Size(82, 23);
            this.buttonChildTags.TabIndex = 11;
            this.buttonChildTags.Text = "Child Tags";
            this.buttonChildTags.UseVisualStyleBackColor = true;
            this.buttonChildTags.Click += new System.EventHandler(this.buttonChildTags_Click);
            // 
            // TagEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(202, 283);
            this.Controls.Add(this.buttonChildTags);
            this.Controls.Add(this.buttonParentTags);
            this.Controls.Add(this.buttonLink);
            this.Controls.Add(this.buttonRenameTag);
            this.Controls.Add(this.checkBoxEnabled);
            this.Controls.Add(this.labelFoundImages);
            this.Controls.Add(this.labelTagName);
            this.Controls.Add(this.checkBoxUseForNaming);
            this.Controls.Add(this.buttomRemoveFromSelectedImages);
            this.Controls.Add(this.buttonDeleteTag);
            this.Controls.Add(this.buttonSelectImagesWithTag);
            this.Controls.Add(this.buttonAddToSelectedImages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TagEditorForm";
            this.Text = "TagEditorForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAddToSelectedImages;
        private System.Windows.Forms.Button buttonSelectImagesWithTag;
        private System.Windows.Forms.Button buttonDeleteTag;
        private System.Windows.Forms.Button buttomRemoveFromSelectedImages;
        private System.Windows.Forms.CheckBox checkBoxUseForNaming;
        private System.Windows.Forms.Label labelTagName;
        private System.Windows.Forms.Label labelFoundImages;
        private System.Windows.Forms.CheckBox checkBoxEnabled;
        private System.Windows.Forms.Button buttonRenameTag;
        private System.Windows.Forms.Button buttonLink;
        private System.Windows.Forms.Button buttonParentTags;
        private System.Windows.Forms.Button buttonChildTags;
    }
}