namespace WallpaperManager.Options
{
    partial class OptionsForm
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
            this.components = new System.ComponentModel.Container();
            this.checkBoxLargerImagesOnLargerMonitors = new System.Windows.Forms.CheckBox();
            this.checkBoxHigherRankedImagesOnLargerMonitors = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableDetectionOfInactiveImages = new System.Windows.Forms.CheckBox();
            this.buttonInspectRankDistribution = new System.Windows.Forms.Button();
            this.buttonModifyMaxRank = new System.Windows.Forms.Button();
            this.labelDefaultTheme = new System.Windows.Forms.Label();
            this.labelDefaultThemePath = new System.Windows.Forms.Label();
            this.buttonSetDefaultTheme = new System.Windows.Forms.Button();
            this.buttonLoadDefaultTheme = new System.Windows.Forms.Button();
            this.checkBoxEnableGlobalHotkey = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxWeightedRanks = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageThemeOptions = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxExactVideo = new System.Windows.Forms.TextBox();
            this.textBoxExactGIF = new System.Windows.Forms.TextBox();
            this.textBoxExactStatic = new System.Windows.Forms.TextBox();
            this.labelVideo = new System.Windows.Forms.Label();
            this.textBoxRelativeVideo = new System.Windows.Forms.TextBox();
            this.labelGIF = new System.Windows.Forms.Label();
            this.textBoxRelativeGIF = new System.Windows.Forms.TextBox();
            this.textBoxRelativeStatic = new System.Windows.Forms.TextBox();
            this.labelStatic = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelFrequency = new System.Windows.Forms.Label();
            this.tabPageGlobalOptions = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.labelMinVideoLoops = new System.Windows.Forms.Label();
            this.labelMaximumVideoTime = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageThemeOptions.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageGlobalOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxLargerImagesOnLargerMonitors
            // 
            this.checkBoxLargerImagesOnLargerMonitors.AutoSize = true;
            this.checkBoxLargerImagesOnLargerMonitors.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxLargerImagesOnLargerMonitors.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxLargerImagesOnLargerMonitors.Location = new System.Drawing.Point(6, 6);
            this.checkBoxLargerImagesOnLargerMonitors.Name = "checkBoxLargerImagesOnLargerMonitors";
            this.checkBoxLargerImagesOnLargerMonitors.Size = new System.Drawing.Size(184, 17);
            this.checkBoxLargerImagesOnLargerMonitors.TabIndex = 36;
            this.checkBoxLargerImagesOnLargerMonitors.Text = "Larger Images on Larger Monitors";
            this.checkBoxLargerImagesOnLargerMonitors.UseVisualStyleBackColor = true;
            // 
            // checkBoxHigherRankedImagesOnLargerMonitors
            // 
            this.checkBoxHigherRankedImagesOnLargerMonitors.AutoSize = true;
            this.checkBoxHigherRankedImagesOnLargerMonitors.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxHigherRankedImagesOnLargerMonitors.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxHigherRankedImagesOnLargerMonitors.Location = new System.Drawing.Point(6, 29);
            this.checkBoxHigherRankedImagesOnLargerMonitors.Name = "checkBoxHigherRankedImagesOnLargerMonitors";
            this.checkBoxHigherRankedImagesOnLargerMonitors.Size = new System.Drawing.Size(226, 17);
            this.checkBoxHigherRankedImagesOnLargerMonitors.TabIndex = 37;
            this.checkBoxHigherRankedImagesOnLargerMonitors.Text = "Higher Ranked Images on Larger Monitors";
            this.checkBoxHigherRankedImagesOnLargerMonitors.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableDetectionOfInactiveImages
            // 
            this.checkBoxEnableDetectionOfInactiveImages.AutoSize = true;
            this.checkBoxEnableDetectionOfInactiveImages.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxEnableDetectionOfInactiveImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxEnableDetectionOfInactiveImages.Location = new System.Drawing.Point(6, 52);
            this.checkBoxEnableDetectionOfInactiveImages.Name = "checkBoxEnableDetectionOfInactiveImages";
            this.checkBoxEnableDetectionOfInactiveImages.Size = new System.Drawing.Size(198, 17);
            this.checkBoxEnableDetectionOfInactiveImages.TabIndex = 40;
            this.checkBoxEnableDetectionOfInactiveImages.Text = "Enable Detection of Inactive Images";
            this.checkBoxEnableDetectionOfInactiveImages.UseVisualStyleBackColor = true;
            // 
            // buttonInspectRankDistribution
            // 
            this.buttonInspectRankDistribution.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonInspectRankDistribution.Location = new System.Drawing.Point(335, 6);
            this.buttonInspectRankDistribution.Name = "buttonInspectRankDistribution";
            this.buttonInspectRankDistribution.Size = new System.Drawing.Size(134, 23);
            this.buttonInspectRankDistribution.TabIndex = 41;
            this.buttonInspectRankDistribution.Text = "Inspect Rank Distribution";
            this.buttonInspectRankDistribution.UseVisualStyleBackColor = true;
            this.buttonInspectRankDistribution.Click += new System.EventHandler(this.buttonInspectRankDistribution_Click);
            // 
            // buttonModifyMaxRank
            // 
            this.buttonModifyMaxRank.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonModifyMaxRank.Location = new System.Drawing.Point(335, 35);
            this.buttonModifyMaxRank.Name = "buttonModifyMaxRank";
            this.buttonModifyMaxRank.Size = new System.Drawing.Size(134, 23);
            this.buttonModifyMaxRank.TabIndex = 42;
            this.buttonModifyMaxRank.Text = "Modify Max Rank";
            this.buttonModifyMaxRank.UseVisualStyleBackColor = true;
            this.buttonModifyMaxRank.Click += new System.EventHandler(this.buttonModifyMaxRank_Click);
            // 
            // labelDefaultTheme
            // 
            this.labelDefaultTheme.AutoSize = true;
            this.labelDefaultTheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultTheme.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.labelDefaultTheme.Location = new System.Drawing.Point(3, 3);
            this.labelDefaultTheme.Name = "labelDefaultTheme";
            this.labelDefaultTheme.Size = new System.Drawing.Size(113, 16);
            this.labelDefaultTheme.TabIndex = 44;
            this.labelDefaultTheme.Text = "Default Theme:";
            // 
            // labelDefaultThemePath
            // 
            this.labelDefaultThemePath.AutoSize = true;
            this.labelDefaultThemePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultThemePath.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.labelDefaultThemePath.Location = new System.Drawing.Point(3, 29);
            this.labelDefaultThemePath.Name = "labelDefaultThemePath";
            this.labelDefaultThemePath.Size = new System.Drawing.Size(94, 13);
            this.labelDefaultThemePath.TabIndex = 45;
            this.labelDefaultThemePath.Text = "defaultThemePath";
            // 
            // buttonSetDefaultTheme
            // 
            this.buttonSetDefaultTheme.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSetDefaultTheme.Location = new System.Drawing.Point(240, 0);
            this.buttonSetDefaultTheme.Name = "buttonSetDefaultTheme";
            this.buttonSetDefaultTheme.Size = new System.Drawing.Size(112, 23);
            this.buttonSetDefaultTheme.TabIndex = 46;
            this.buttonSetDefaultTheme.Text = "Set Default Theme";
            this.buttonSetDefaultTheme.UseVisualStyleBackColor = true;
            this.buttonSetDefaultTheme.Click += new System.EventHandler(this.buttonSetDefaultTheme_Click);
            // 
            // buttonLoadDefaultTheme
            // 
            this.buttonLoadDefaultTheme.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonLoadDefaultTheme.Location = new System.Drawing.Point(122, 0);
            this.buttonLoadDefaultTheme.Name = "buttonLoadDefaultTheme";
            this.buttonLoadDefaultTheme.Size = new System.Drawing.Size(112, 23);
            this.buttonLoadDefaultTheme.TabIndex = 47;
            this.buttonLoadDefaultTheme.Text = "Load Default Theme";
            this.buttonLoadDefaultTheme.UseVisualStyleBackColor = true;
            this.buttonLoadDefaultTheme.Click += new System.EventHandler(this.buttonLoadDefaultTheme_Click);
            // 
            // checkBoxEnableGlobalHotkey
            // 
            this.checkBoxEnableGlobalHotkey.AutoSize = true;
            this.checkBoxEnableGlobalHotkey.Checked = true;
            this.checkBoxEnableGlobalHotkey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableGlobalHotkey.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxEnableGlobalHotkey.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxEnableGlobalHotkey.Location = new System.Drawing.Point(6, 45);
            this.checkBoxEnableGlobalHotkey.Name = "checkBoxEnableGlobalHotkey";
            this.checkBoxEnableGlobalHotkey.Size = new System.Drawing.Size(250, 17);
            this.checkBoxEnableGlobalHotkey.TabIndex = 48;
            this.checkBoxEnableGlobalHotkey.Text = "Enable Default Theme Global Hotkey (Alt+Shift)";
            this.checkBoxEnableGlobalHotkey.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(3, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(410, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "(Your Active Theme will be automatically saved upon swapping to the Default Theme" +
    ")";
            // 
            // checkBoxWeightedRanks
            // 
            this.checkBoxWeightedRanks.AutoSize = true;
            this.checkBoxWeightedRanks.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxWeightedRanks.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxWeightedRanks.Location = new System.Drawing.Point(6, 75);
            this.checkBoxWeightedRanks.Name = "checkBoxWeightedRanks";
            this.checkBoxWeightedRanks.Size = new System.Drawing.Size(106, 17);
            this.checkBoxWeightedRanks.TabIndex = 50;
            this.checkBoxWeightedRanks.Text = "Weighted Ranks";
            this.checkBoxWeightedRanks.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageThemeOptions);
            this.tabControl1.Controls.Add(this.tabPageGlobalOptions);
            this.tabControl1.Location = new System.Drawing.Point(-1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(485, 449);
            this.tabControl1.TabIndex = 51;
            // 
            // tabPageThemeOptions
            // 
            this.tabPageThemeOptions.BackColor = System.Drawing.Color.Black;
            this.tabPageThemeOptions.Controls.Add(this.labelMaximumVideoTime);
            this.tabPageThemeOptions.Controls.Add(this.labelMinVideoLoops);
            this.tabPageThemeOptions.Controls.Add(this.panel1);
            this.tabPageThemeOptions.Controls.Add(this.buttonInspectRankDistribution);
            this.tabPageThemeOptions.Controls.Add(this.checkBoxWeightedRanks);
            this.tabPageThemeOptions.Controls.Add(this.buttonModifyMaxRank);
            this.tabPageThemeOptions.Controls.Add(this.checkBoxHigherRankedImagesOnLargerMonitors);
            this.tabPageThemeOptions.Controls.Add(this.checkBoxLargerImagesOnLargerMonitors);
            this.tabPageThemeOptions.Controls.Add(this.checkBoxEnableDetectionOfInactiveImages);
            this.tabPageThemeOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageThemeOptions.Name = "tabPageThemeOptions";
            this.tabPageThemeOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageThemeOptions.Size = new System.Drawing.Size(477, 423);
            this.tabPageThemeOptions.TabIndex = 0;
            this.tabPageThemeOptions.Text = "Theme Options";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBoxExactVideo);
            this.panel1.Controls.Add(this.textBoxExactGIF);
            this.panel1.Controls.Add(this.textBoxExactStatic);
            this.panel1.Controls.Add(this.labelVideo);
            this.panel1.Controls.Add(this.textBoxRelativeVideo);
            this.panel1.Controls.Add(this.labelGIF);
            this.panel1.Controls.Add(this.textBoxRelativeGIF);
            this.panel1.Controls.Add(this.textBoxRelativeStatic);
            this.panel1.Controls.Add(this.labelStatic);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.labelFrequency);
            this.panel1.Location = new System.Drawing.Point(9, 98);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 126);
            this.panel1.TabIndex = 51;
            // 
            // textBoxExactVideo
            // 
            this.textBoxExactVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxExactVideo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxExactVideo.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxExactVideo.Location = new System.Drawing.Point(211, 92);
            this.textBoxExactVideo.Name = "textBoxExactVideo";
            this.textBoxExactVideo.Size = new System.Drawing.Size(45, 20);
            this.textBoxExactVideo.TabIndex = 59;
            this.textBoxExactVideo.Text = "0";
            this.textBoxExactVideo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxExactGIF
            // 
            this.textBoxExactGIF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxExactGIF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxExactGIF.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxExactGIF.Location = new System.Drawing.Point(144, 92);
            this.textBoxExactGIF.Name = "textBoxExactGIF";
            this.textBoxExactGIF.Size = new System.Drawing.Size(45, 20);
            this.textBoxExactGIF.TabIndex = 58;
            this.textBoxExactGIF.Text = "0";
            this.textBoxExactGIF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxExactStatic
            // 
            this.textBoxExactStatic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxExactStatic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxExactStatic.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxExactStatic.Location = new System.Drawing.Point(72, 90);
            this.textBoxExactStatic.Name = "textBoxExactStatic";
            this.textBoxExactStatic.Size = new System.Drawing.Size(45, 20);
            this.textBoxExactStatic.TabIndex = 57;
            this.textBoxExactStatic.Text = "0";
            this.textBoxExactStatic.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelVideo
            // 
            this.labelVideo.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelVideo.Location = new System.Drawing.Point(214, 41);
            this.labelVideo.Name = "labelVideo";
            this.labelVideo.Size = new System.Drawing.Size(39, 13);
            this.labelVideo.TabIndex = 56;
            this.labelVideo.Text = "Video";
            this.labelVideo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxRelativeVideo
            // 
            this.textBoxRelativeVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxRelativeVideo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRelativeVideo.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxRelativeVideo.Location = new System.Drawing.Point(211, 57);
            this.textBoxRelativeVideo.Name = "textBoxRelativeVideo";
            this.textBoxRelativeVideo.Size = new System.Drawing.Size(45, 20);
            this.textBoxRelativeVideo.TabIndex = 55;
            this.textBoxRelativeVideo.Text = "0";
            this.textBoxRelativeVideo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelGIF
            // 
            this.labelGIF.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelGIF.Location = new System.Drawing.Point(147, 41);
            this.labelGIF.Name = "labelGIF";
            this.labelGIF.Size = new System.Drawing.Size(39, 13);
            this.labelGIF.TabIndex = 54;
            this.labelGIF.Text = "GIF";
            this.labelGIF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxRelativeGIF
            // 
            this.textBoxRelativeGIF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxRelativeGIF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRelativeGIF.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxRelativeGIF.Location = new System.Drawing.Point(144, 57);
            this.textBoxRelativeGIF.Name = "textBoxRelativeGIF";
            this.textBoxRelativeGIF.Size = new System.Drawing.Size(45, 20);
            this.textBoxRelativeGIF.TabIndex = 53;
            this.textBoxRelativeGIF.Text = "0";
            this.textBoxRelativeGIF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxRelativeStatic
            // 
            this.textBoxRelativeStatic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxRelativeStatic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRelativeStatic.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxRelativeStatic.Location = new System.Drawing.Point(72, 55);
            this.textBoxRelativeStatic.Name = "textBoxRelativeStatic";
            this.textBoxRelativeStatic.Size = new System.Drawing.Size(45, 20);
            this.textBoxRelativeStatic.TabIndex = 52;
            this.textBoxRelativeStatic.Text = "0";
            this.textBoxRelativeStatic.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelStatic
            // 
            this.labelStatic.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelStatic.Location = new System.Drawing.Point(75, 39);
            this.labelStatic.Name = "labelStatic";
            this.labelStatic.Size = new System.Drawing.Size(39, 13);
            this.labelStatic.TabIndex = 3;
            this.labelStatic.Text = "Static";
            this.labelStatic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(3, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Exact";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(3, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Relative";
            // 
            // labelFrequency
            // 
            this.labelFrequency.AutoSize = true;
            this.labelFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFrequency.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelFrequency.Location = new System.Drawing.Point(130, 10);
            this.labelFrequency.Name = "labelFrequency";
            this.labelFrequency.Size = new System.Drawing.Size(73, 15);
            this.labelFrequency.TabIndex = 0;
            this.labelFrequency.Text = "Frequency";
            // 
            // tabPageGlobalOptions
            // 
            this.tabPageGlobalOptions.BackColor = System.Drawing.Color.Black;
            this.tabPageGlobalOptions.Controls.Add(this.labelDefaultTheme);
            this.tabPageGlobalOptions.Controls.Add(this.label1);
            this.tabPageGlobalOptions.Controls.Add(this.labelDefaultThemePath);
            this.tabPageGlobalOptions.Controls.Add(this.checkBoxEnableGlobalHotkey);
            this.tabPageGlobalOptions.Controls.Add(this.buttonSetDefaultTheme);
            this.tabPageGlobalOptions.Controls.Add(this.buttonLoadDefaultTheme);
            this.tabPageGlobalOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageGlobalOptions.Name = "tabPageGlobalOptions";
            this.tabPageGlobalOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGlobalOptions.Size = new System.Drawing.Size(477, 423);
            this.tabPageGlobalOptions.TabIndex = 1;
            this.tabPageGlobalOptions.Text = "Global Options";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 12);
            this.label4.TabIndex = 52;
            this.label4.Text = "(Press Enter to save changes)";
            // 
            // labelMinVideoLoops
            // 
            this.labelMinVideoLoops.AutoSize = true;
            this.labelMinVideoLoops.ForeColor = System.Drawing.SystemColors.Control;
            this.labelMinVideoLoops.Location = new System.Drawing.Point(3, 276);
            this.labelMinVideoLoops.Name = "labelMinVideoLoops";
            this.labelMinVideoLoops.Size = new System.Drawing.Size(116, 13);
            this.labelMinVideoLoops.TabIndex = 53;
            this.labelMinVideoLoops.Text = "Minimum Video Loops: ";
            // 
            // labelMaximumVideoTime
            // 
            this.labelMaximumVideoTime.AutoSize = true;
            this.labelMaximumVideoTime.ForeColor = System.Drawing.SystemColors.Control;
            this.labelMaximumVideoTime.Location = new System.Drawing.Point(3, 297);
            this.labelMaximumVideoTime.Name = "labelMaximumVideoTime";
            this.labelMaximumVideoTime.Size = new System.Drawing.Size(113, 13);
            this.labelMaximumVideoTime.TabIndex = 54;
            this.labelMaximumVideoTime.Text = "Maximum Video Time: ";
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(484, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.tabControl1.ResumeLayout(false);
            this.tabPageThemeOptions.ResumeLayout(false);
            this.tabPageThemeOptions.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageGlobalOptions.ResumeLayout(false);
            this.tabPageGlobalOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxLargerImagesOnLargerMonitors;
        private System.Windows.Forms.CheckBox checkBoxHigherRankedImagesOnLargerMonitors;
        private System.Windows.Forms.CheckBox checkBoxEnableDetectionOfInactiveImages;
        private System.Windows.Forms.Button buttonInspectRankDistribution;
        private System.Windows.Forms.Button buttonModifyMaxRank;
        private System.Windows.Forms.Label labelDefaultTheme;
        private System.Windows.Forms.Label labelDefaultThemePath;
        private System.Windows.Forms.Button buttonSetDefaultTheme;
        private System.Windows.Forms.Button buttonLoadDefaultTheme;
        private System.Windows.Forms.CheckBox checkBoxEnableGlobalHotkey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxWeightedRanks;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageThemeOptions;
        private System.Windows.Forms.TabPage tabPageGlobalOptions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelFrequency;
        private System.Windows.Forms.Label labelStatic;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxRelativeGIF;
        private System.Windows.Forms.TextBox textBoxRelativeStatic;
        private System.Windows.Forms.TextBox textBoxExactVideo;
        private System.Windows.Forms.TextBox textBoxExactGIF;
        private System.Windows.Forms.TextBox textBoxExactStatic;
        private System.Windows.Forms.Label labelVideo;
        private System.Windows.Forms.TextBox textBoxRelativeVideo;
        private System.Windows.Forms.Label labelGIF;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelMaximumVideoTime;
        private System.Windows.Forms.Label labelMinVideoLoops;
    }
}