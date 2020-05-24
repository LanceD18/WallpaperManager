namespace WallpaperManager.Tagging
{
    partial class TagForm
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
            this.buttonAddTag = new System.Windows.Forms.Button();
            this.buttonAddCategory = new System.Windows.Forms.Button();
            this.buttonRemoveCategory = new System.Windows.Forms.Button();
            this.labelCategory = new System.Windows.Forms.Label();
            this.labelTagging = new System.Windows.Forms.Label();
            this.buttonRenameCategory = new System.Windows.Forms.Button();
            this.buttonSelectCategoryImages = new System.Windows.Forms.Button();
            this.labelDefaultSettings = new System.Windows.Forms.Label();
            this.labelDefaultSettingsInfo = new System.Windows.Forms.Label();
            this.checkBoxEnabled = new System.Windows.Forms.CheckBox();
            this.checkBoxUseForNaming = new System.Windows.Forms.CheckBox();
            this.buttonApplyDefaultSettings = new System.Windows.Forms.Button();
            this.labelContainsTags = new System.Windows.Forms.Label();
            this.buttonNamingOrderHelp = new System.Windows.Forms.Button();
            this.buttonRemoveTag = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAddTag
            // 
            this.buttonAddTag.Location = new System.Drawing.Point(9, 434);
            this.buttonAddTag.Name = "buttonAddTag";
            this.buttonAddTag.Size = new System.Drawing.Size(101, 23);
            this.buttonAddTag.TabIndex = 13;
            this.buttonAddTag.Text = "Add Tag";
            this.buttonAddTag.UseVisualStyleBackColor = true;
            this.buttonAddTag.Click += new System.EventHandler(this.buttonAddTag_Click);
            // 
            // buttonAddCategory
            // 
            this.buttonAddCategory.Location = new System.Drawing.Point(8, 63);
            this.buttonAddCategory.Name = "buttonAddCategory";
            this.buttonAddCategory.Size = new System.Drawing.Size(101, 23);
            this.buttonAddCategory.TabIndex = 11;
            this.buttonAddCategory.Text = "Add Category";
            this.buttonAddCategory.UseVisualStyleBackColor = true;
            this.buttonAddCategory.Click += new System.EventHandler(this.buttonAddCategory_Click);
            // 
            // buttonRemoveCategory
            // 
            this.buttonRemoveCategory.Location = new System.Drawing.Point(8, 92);
            this.buttonRemoveCategory.Name = "buttonRemoveCategory";
            this.buttonRemoveCategory.Size = new System.Drawing.Size(101, 23);
            this.buttonRemoveCategory.TabIndex = 10;
            this.buttonRemoveCategory.Text = "Remove Category";
            this.buttonRemoveCategory.UseVisualStyleBackColor = true;
            this.buttonRemoveCategory.Click += new System.EventHandler(this.buttonRemoveCategory_Click);
            // 
            // labelCategory
            // 
            this.labelCategory.AutoSize = true;
            this.labelCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCategory.ForeColor = System.Drawing.SystemColors.Control;
            this.labelCategory.Location = new System.Drawing.Point(6, 9);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(95, 18);
            this.labelCategory.TabIndex = 16;
            this.labelCategory.Text = "Categories:";
            // 
            // labelTagging
            // 
            this.labelTagging.AutoSize = true;
            this.labelTagging.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTagging.ForeColor = System.Drawing.SystemColors.Control;
            this.labelTagging.Location = new System.Drawing.Point(6, 403);
            this.labelTagging.Name = "labelTagging";
            this.labelTagging.Size = new System.Drawing.Size(77, 18);
            this.labelTagging.TabIndex = 17;
            this.labelTagging.Text = "Tagging: ";
            // 
            // buttonRenameCategory
            // 
            this.buttonRenameCategory.Location = new System.Drawing.Point(8, 121);
            this.buttonRenameCategory.Name = "buttonRenameCategory";
            this.buttonRenameCategory.Size = new System.Drawing.Size(101, 23);
            this.buttonRenameCategory.TabIndex = 18;
            this.buttonRenameCategory.Text = "Rename Category";
            this.buttonRenameCategory.UseVisualStyleBackColor = true;
            this.buttonRenameCategory.Click += new System.EventHandler(this.buttonRenameCategory_Click);
            // 
            // buttonSelectCategoryImages
            // 
            this.buttonSelectCategoryImages.Location = new System.Drawing.Point(7, 150);
            this.buttonSelectCategoryImages.Name = "buttonSelectCategoryImages";
            this.buttonSelectCategoryImages.Size = new System.Drawing.Size(101, 34);
            this.buttonSelectCategoryImages.TabIndex = 19;
            this.buttonSelectCategoryImages.Text = "Select Category Images";
            this.buttonSelectCategoryImages.UseVisualStyleBackColor = true;
            this.buttonSelectCategoryImages.Click += new System.EventHandler(this.buttonSelectCategoryImages_Click);
            // 
            // labelDefaultSettings
            // 
            this.labelDefaultSettings.AutoSize = true;
            this.labelDefaultSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultSettings.ForeColor = System.Drawing.SystemColors.Control;
            this.labelDefaultSettings.Location = new System.Drawing.Point(6, 196);
            this.labelDefaultSettings.Name = "labelDefaultSettings";
            this.labelDefaultSettings.Size = new System.Drawing.Size(102, 13);
            this.labelDefaultSettings.TabIndex = 20;
            this.labelDefaultSettings.Text = "Default Settings:";
            // 
            // labelDefaultSettingsInfo
            // 
            this.labelDefaultSettingsInfo.AutoSize = true;
            this.labelDefaultSettingsInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultSettingsInfo.ForeColor = System.Drawing.SystemColors.Control;
            this.labelDefaultSettingsInfo.Location = new System.Drawing.Point(8, 213);
            this.labelDefaultSettingsInfo.Name = "labelDefaultSettingsInfo";
            this.labelDefaultSettingsInfo.Size = new System.Drawing.Size(98, 36);
            this.labelDefaultSettingsInfo.TabIndex = 21;
            this.labelDefaultSettingsInfo.Text = "(Will only apply to new \r\ntags unless you press\r\nthe button below)";
            // 
            // checkBoxEnabled
            // 
            this.checkBoxEnabled.AutoSize = true;
            this.checkBoxEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnabled.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBoxEnabled.Location = new System.Drawing.Point(8, 252);
            this.checkBoxEnabled.Name = "checkBoxEnabled";
            this.checkBoxEnabled.Size = new System.Drawing.Size(65, 17);
            this.checkBoxEnabled.TabIndex = 23;
            this.checkBoxEnabled.Text = "Enabled";
            this.checkBoxEnabled.UseVisualStyleBackColor = true;
            this.checkBoxEnabled.CheckedChanged += new System.EventHandler(this.checkBoxEnabled_CheckedChanged);
            // 
            // checkBoxUseForNaming
            // 
            this.checkBoxUseForNaming.AutoSize = true;
            this.checkBoxUseForNaming.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxUseForNaming.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBoxUseForNaming.Location = new System.Drawing.Point(8, 277);
            this.checkBoxUseForNaming.Name = "checkBoxUseForNaming";
            this.checkBoxUseForNaming.Size = new System.Drawing.Size(105, 17);
            this.checkBoxUseForNaming.TabIndex = 22;
            this.checkBoxUseForNaming.Text = "Use for Naming?";
            this.checkBoxUseForNaming.UseVisualStyleBackColor = true;
            this.checkBoxUseForNaming.CheckedChanged += new System.EventHandler(this.checkBoxUseForNaming_CheckedChanged);
            // 
            // buttonApplyDefaultSettings
            // 
            this.buttonApplyDefaultSettings.Location = new System.Drawing.Point(8, 300);
            this.buttonApplyDefaultSettings.Name = "buttonApplyDefaultSettings";
            this.buttonApplyDefaultSettings.Size = new System.Drawing.Size(101, 48);
            this.buttonApplyDefaultSettings.TabIndex = 24;
            this.buttonApplyDefaultSettings.Text = "Apply Default Settings to all Tags";
            this.buttonApplyDefaultSettings.UseVisualStyleBackColor = true;
            this.buttonApplyDefaultSettings.Click += new System.EventHandler(this.buttonApplyDefaultSettings_Click);
            // 
            // labelContainsTags
            // 
            this.labelContainsTags.AutoSize = true;
            this.labelContainsTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContainsTags.ForeColor = System.Drawing.SystemColors.Control;
            this.labelContainsTags.Location = new System.Drawing.Point(6, 37);
            this.labelContainsTags.Name = "labelContainsTags";
            this.labelContainsTags.Size = new System.Drawing.Size(85, 13);
            this.labelContainsTags.TabIndex = 25;
            this.labelContainsTags.Text = "Contains X Tags";
            // 
            // buttonNamingOrderHelp
            // 
            this.buttonNamingOrderHelp.Location = new System.Drawing.Point(8, 354);
            this.buttonNamingOrderHelp.Name = "buttonNamingOrderHelp";
            this.buttonNamingOrderHelp.Size = new System.Drawing.Size(101, 34);
            this.buttonNamingOrderHelp.TabIndex = 26;
            this.buttonNamingOrderHelp.Text = "Naming Order (Help)";
            this.buttonNamingOrderHelp.UseVisualStyleBackColor = true;
            this.buttonNamingOrderHelp.Click += new System.EventHandler(this.buttonNamingOrderHelp_Click);
            // 
            // buttonRemoveTag
            // 
            this.buttonRemoveTag.Location = new System.Drawing.Point(9, 463);
            this.buttonRemoveTag.Name = "buttonRemoveTag";
            this.buttonRemoveTag.Size = new System.Drawing.Size(101, 23);
            this.buttonRemoveTag.TabIndex = 28;
            this.buttonRemoveTag.Text = "Remove Tag";
            this.buttonRemoveTag.UseVisualStyleBackColor = true;
            this.buttonRemoveTag.Click += new System.EventHandler(this.buttonRemoveTag_Click);
            // 
            // TagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(684, 576);
            this.Controls.Add(this.buttonRemoveTag);
            this.Controls.Add(this.buttonNamingOrderHelp);
            this.Controls.Add(this.labelContainsTags);
            this.Controls.Add(this.buttonApplyDefaultSettings);
            this.Controls.Add(this.checkBoxEnabled);
            this.Controls.Add(this.checkBoxUseForNaming);
            this.Controls.Add(this.labelDefaultSettingsInfo);
            this.Controls.Add(this.labelDefaultSettings);
            this.Controls.Add(this.buttonSelectCategoryImages);
            this.Controls.Add(this.buttonRenameCategory);
            this.Controls.Add(this.labelTagging);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.buttonAddTag);
            this.Controls.Add(this.buttonAddCategory);
            this.Controls.Add(this.buttonRemoveCategory);
            this.Name = "TagForm";
            this.Text = "TagForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonAddTag;
        private System.Windows.Forms.Button buttonAddCategory;
        private System.Windows.Forms.Button buttonRemoveCategory;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.Label labelTagging;
        private System.Windows.Forms.Button buttonRenameCategory;
        private System.Windows.Forms.Button buttonSelectCategoryImages;
        private System.Windows.Forms.Label labelDefaultSettings;
        private System.Windows.Forms.Label labelDefaultSettingsInfo;
        private System.Windows.Forms.CheckBox checkBoxEnabled;
        private System.Windows.Forms.CheckBox checkBoxUseForNaming;
        private System.Windows.Forms.Button buttonApplyDefaultSettings;
        private System.Windows.Forms.Label labelContainsTags;
        private System.Windows.Forms.Button buttonNamingOrderHelp;
        private System.Windows.Forms.Button buttonRemoveTag;
    }
}