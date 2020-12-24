namespace WallpaperManager
{
    partial class WallpaperManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WallpaperManager));
            this.notifyIconWallpaperManager = new System.Windows.Forms.NotifyIcon(this.components);
            this.buttonNextWallpaper = new System.Windows.Forms.Button();
            this.buttonPreviousWallpaper = new System.Windows.Forms.Button();
            this.buttonChangeDisplay = new System.Windows.Forms.Button();
            this.panelImageFolders = new System.Windows.Forms.Panel();
            this.buttomRemoveFolder = new System.Windows.Forms.Button();
            this.buttonAddFolder = new System.Windows.Forms.Button();
            this.flowLayoutPanelImageFolders = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonSaveTheme = new System.Windows.Forms.Button();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonLoadTheme = new System.Windows.Forms.Button();
            this.buttonUpdateTheme = new System.Windows.Forms.Button();
            this.buttonTagSettings = new System.Windows.Forms.Button();
            this.buttonOptions = new System.Windows.Forms.Button();
            this.panelImageSettings = new System.Windows.Forms.Panel();
            this.buttonRankImages = new System.Windows.Forms.Button();
            this.buttonNameImage = new System.Windows.Forms.Button();
            this.buttonClearSelection = new System.Windows.Forms.Button();
            this.buttonInspectImage = new System.Windows.Forms.Button();
            this.buttonMoveImage = new System.Windows.Forms.Button();
            this.buttonDeleteImage = new System.Windows.Forms.Button();
            this.comboBoxSelectStyle = new System.Windows.Forms.ComboBox();
            this.labelTimeLeft = new System.Windows.Forms.Label();
            this.comboBoxWallpaperInterval = new System.Windows.Forms.ComboBox();
            this.labelWallpaperStyle = new System.Windows.Forms.Label();
            this.panelImageSelector = new System.Windows.Forms.Panel();
            this.tabControlImagePages = new System.Windows.Forms.TabControl();
            this.labelSelectedImage = new System.Windows.Forms.Label();
            this.panelImageInspector = new System.Windows.Forms.Panel();
            this.inspector_axWindowsMediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.inspector_buttonRight = new System.Windows.Forms.Button();
            this.inspector_buttonLeft = new System.Windows.Forms.Button();
            this.inspector_buttonPlus = new System.Windows.Forms.Button();
            this.inspector_textBoxRankEditor = new System.Windows.Forms.TextBox();
            this.inspector_buttonMinus = new System.Windows.Forms.Button();
            this.inspector_pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.labelImageSize = new System.Windows.Forms.Label();
            this.timerVisualUpdater = new System.Windows.Forms.Timer(this.components);
            this.panelImageFolders.SuspendLayout();
            this.panelImageSettings.SuspendLayout();
            this.panelImageSelector.SuspendLayout();
            this.panelImageInspector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inspector_axWindowsMediaPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inspector_pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIconWallpaperManager
            // 
            this.notifyIconWallpaperManager.Text = "Wallpaper Manager";
            this.notifyIconWallpaperManager.Visible = true;
            // 
            // buttonNextWallpaper
            // 
            this.buttonNextWallpaper.Location = new System.Drawing.Point(12, 53);
            this.buttonNextWallpaper.Name = "buttonNextWallpaper";
            this.buttonNextWallpaper.Size = new System.Drawing.Size(125, 25);
            this.buttonNextWallpaper.TabIndex = 0;
            this.buttonNextWallpaper.Text = "Next Wallpaper";
            this.buttonNextWallpaper.UseVisualStyleBackColor = true;
            this.buttonNextWallpaper.Click += new System.EventHandler(this.buttonNextWallpaper_Click);
            // 
            // buttonPreviousWallpaper
            // 
            this.buttonPreviousWallpaper.Location = new System.Drawing.Point(12, 84);
            this.buttonPreviousWallpaper.Name = "buttonPreviousWallpaper";
            this.buttonPreviousWallpaper.Size = new System.Drawing.Size(125, 25);
            this.buttonPreviousWallpaper.TabIndex = 1;
            this.buttonPreviousWallpaper.Text = "Previous Wallpaper";
            this.buttonPreviousWallpaper.UseVisualStyleBackColor = true;
            this.buttonPreviousWallpaper.Click += new System.EventHandler(this.buttonPreviousWallpaper_Click);
            // 
            // buttonChangeDisplay
            // 
            this.buttonChangeDisplay.Location = new System.Drawing.Point(12, 115);
            this.buttonChangeDisplay.Name = "buttonChangeDisplay";
            this.buttonChangeDisplay.Size = new System.Drawing.Size(125, 25);
            this.buttonChangeDisplay.TabIndex = 2;
            this.buttonChangeDisplay.Text = "Flip Display";
            this.buttonChangeDisplay.UseVisualStyleBackColor = true;
            // 
            // panelImageFolders
            // 
            this.panelImageFolders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.panelImageFolders.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelImageFolders.Controls.Add(this.buttomRemoveFolder);
            this.panelImageFolders.Controls.Add(this.buttonAddFolder);
            this.panelImageFolders.Controls.Add(this.flowLayoutPanelImageFolders);
            this.panelImageFolders.Location = new System.Drawing.Point(12, 162);
            this.panelImageFolders.Name = "panelImageFolders";
            this.panelImageFolders.Size = new System.Drawing.Size(600, 290);
            this.panelImageFolders.TabIndex = 3;
            // 
            // buttomRemoveFolder
            // 
            this.buttomRemoveFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttomRemoveFolder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttomRemoveFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttomRemoveFolder.ForeColor = System.Drawing.Color.White;
            this.buttomRemoveFolder.Location = new System.Drawing.Point(100, 5);
            this.buttomRemoveFolder.Name = "buttomRemoveFolder";
            this.buttomRemoveFolder.Size = new System.Drawing.Size(90, 25);
            this.buttomRemoveFolder.TabIndex = 2;
            this.buttomRemoveFolder.Text = "Remove Folder";
            this.buttomRemoveFolder.UseVisualStyleBackColor = false;
            this.buttomRemoveFolder.Click += new System.EventHandler(this.buttomRemoveFolder_Click);
            // 
            // buttonAddFolder
            // 
            this.buttonAddFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.buttonAddFolder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.buttonAddFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddFolder.ForeColor = System.Drawing.Color.White;
            this.buttonAddFolder.Location = new System.Drawing.Point(5, 5);
            this.buttonAddFolder.Name = "buttonAddFolder";
            this.buttonAddFolder.Size = new System.Drawing.Size(90, 25);
            this.buttonAddFolder.TabIndex = 1;
            this.buttonAddFolder.Text = "Add Folder";
            this.buttonAddFolder.UseVisualStyleBackColor = false;
            this.buttonAddFolder.Click += new System.EventHandler(this.buttonAddFolder_Click);
            // 
            // flowLayoutPanelImageFolders
            // 
            this.flowLayoutPanelImageFolders.AutoScroll = true;
            this.flowLayoutPanelImageFolders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.flowLayoutPanelImageFolders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelImageFolders.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelImageFolders.Location = new System.Drawing.Point(0, 35);
            this.flowLayoutPanelImageFolders.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.flowLayoutPanelImageFolders.Name = "flowLayoutPanelImageFolders";
            this.flowLayoutPanelImageFolders.Size = new System.Drawing.Size(598, 250);
            this.flowLayoutPanelImageFolders.TabIndex = 0;
            this.flowLayoutPanelImageFolders.WrapContents = false;
            this.flowLayoutPanelImageFolders.Click += new System.EventHandler(this.flowLayoutPanelImageFolders_Click);
            // 
            // buttonSaveTheme
            // 
            this.buttonSaveTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.buttonSaveTheme.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Olive;
            this.buttonSaveTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveTheme.ForeColor = System.Drawing.Color.White;
            this.buttonSaveTheme.Location = new System.Drawing.Point(12, 12);
            this.buttonSaveTheme.Name = "buttonSaveTheme";
            this.buttonSaveTheme.Size = new System.Drawing.Size(90, 25);
            this.buttonSaveTheme.TabIndex = 3;
            this.buttonSaveTheme.Text = "Save Theme";
            this.buttonSaveTheme.UseVisualStyleBackColor = false;
            this.buttonSaveTheme.Click += new System.EventHandler(this.buttonSaveTheme_Click);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(487, 84);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(125, 25);
            this.buttonSelect.TabIndex = 5;
            this.buttonSelect.Text = "Select Image(s)";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonLoadTheme
            // 
            this.buttonLoadTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonLoadTheme.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonLoadTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadTheme.ForeColor = System.Drawing.Color.White;
            this.buttonLoadTheme.Location = new System.Drawing.Point(108, 12);
            this.buttonLoadTheme.Name = "buttonLoadTheme";
            this.buttonLoadTheme.Size = new System.Drawing.Size(90, 25);
            this.buttonLoadTheme.TabIndex = 6;
            this.buttonLoadTheme.Text = "Load Theme";
            this.buttonLoadTheme.UseVisualStyleBackColor = false;
            this.buttonLoadTheme.Click += new System.EventHandler(this.buttonLoadTheme_Click);
            // 
            // buttonUpdateTheme
            // 
            this.buttonUpdateTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonUpdateTheme.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Purple;
            this.buttonUpdateTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpdateTheme.ForeColor = System.Drawing.Color.White;
            this.buttonUpdateTheme.Location = new System.Drawing.Point(204, 12);
            this.buttonUpdateTheme.Name = "buttonUpdateTheme";
            this.buttonUpdateTheme.Size = new System.Drawing.Size(90, 25);
            this.buttonUpdateTheme.TabIndex = 7;
            this.buttonUpdateTheme.Text = "Update Theme";
            this.buttonUpdateTheme.UseVisualStyleBackColor = false;
            this.buttonUpdateTheme.Click += new System.EventHandler(this.buttonUpdateTheme_Click);
            // 
            // buttonTagSettings
            // 
            this.buttonTagSettings.Location = new System.Drawing.Point(487, 115);
            this.buttonTagSettings.Name = "buttonTagSettings";
            this.buttonTagSettings.Size = new System.Drawing.Size(125, 25);
            this.buttonTagSettings.TabIndex = 8;
            this.buttonTagSettings.Text = "Tag Settings";
            this.buttonTagSettings.UseVisualStyleBackColor = true;
            this.buttonTagSettings.Click += new System.EventHandler(this.buttonTagSettings_Click);
            // 
            // buttonOptions
            // 
            this.buttonOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.buttonOptions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.buttonOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOptions.ForeColor = System.Drawing.Color.White;
            this.buttonOptions.Location = new System.Drawing.Point(300, 12);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(90, 25);
            this.buttonOptions.TabIndex = 10;
            this.buttonOptions.Text = "Options";
            this.buttonOptions.UseVisualStyleBackColor = false;
            this.buttonOptions.Click += new System.EventHandler(this.buttonOptions_Click);
            // 
            // panelImageSettings
            // 
            this.panelImageSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.panelImageSettings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelImageSettings.Controls.Add(this.buttonRankImages);
            this.panelImageSettings.Controls.Add(this.buttonNameImage);
            this.panelImageSettings.Controls.Add(this.buttonClearSelection);
            this.panelImageSettings.Controls.Add(this.buttonInspectImage);
            this.panelImageSettings.Controls.Add(this.buttonMoveImage);
            this.panelImageSettings.Controls.Add(this.buttonDeleteImage);
            this.panelImageSettings.Location = new System.Drawing.Point(618, 638);
            this.panelImageSettings.Name = "panelImageSettings";
            this.panelImageSettings.Size = new System.Drawing.Size(645, 47);
            this.panelImageSettings.TabIndex = 12;
            // 
            // buttonRankImages
            // 
            this.buttonRankImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonRankImages.Location = new System.Drawing.Point(322, 10);
            this.buttonRankImages.Name = "buttonRankImages";
            this.buttonRankImages.Size = new System.Drawing.Size(100, 25);
            this.buttonRankImages.TabIndex = 33;
            this.buttonRankImages.Text = "Rank Images";
            this.buttonRankImages.UseVisualStyleBackColor = true;
            this.buttonRankImages.Click += new System.EventHandler(this.buttonRankImages_Click);
            // 
            // buttonNameImage
            // 
            this.buttonNameImage.Location = new System.Drawing.Point(5, 10);
            this.buttonNameImage.Name = "buttonNameImage";
            this.buttonNameImage.Size = new System.Drawing.Size(100, 25);
            this.buttonNameImage.TabIndex = 32;
            this.buttonNameImage.Text = "Name Image(s)";
            this.buttonNameImage.UseVisualStyleBackColor = true;
            this.buttonNameImage.Click += new System.EventHandler(this.buttonNameImage_Click);
            // 
            // buttonClearSelection
            // 
            this.buttonClearSelection.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonClearSelection.Location = new System.Drawing.Point(532, 10);
            this.buttonClearSelection.Name = "buttonClearSelection";
            this.buttonClearSelection.Size = new System.Drawing.Size(100, 25);
            this.buttonClearSelection.TabIndex = 30;
            this.buttonClearSelection.Text = "Clear Selection";
            this.buttonClearSelection.UseVisualStyleBackColor = true;
            this.buttonClearSelection.Click += new System.EventHandler(this.buttonClearSelection_Click);
            // 
            // buttonInspectImage
            // 
            this.buttonInspectImage.Location = new System.Drawing.Point(426, 10);
            this.buttonInspectImage.Name = "buttonInspectImage";
            this.buttonInspectImage.Size = new System.Drawing.Size(100, 25);
            this.buttonInspectImage.TabIndex = 31;
            this.buttonInspectImage.Text = "Inspect Image";
            this.buttonInspectImage.UseVisualStyleBackColor = true;
            this.buttonInspectImage.Click += new System.EventHandler(this.buttonInspectImage_Click);
            // 
            // buttonMoveImage
            // 
            this.buttonMoveImage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonMoveImage.Location = new System.Drawing.Point(111, 10);
            this.buttonMoveImage.Name = "buttonMoveImage";
            this.buttonMoveImage.Size = new System.Drawing.Size(100, 25);
            this.buttonMoveImage.TabIndex = 23;
            this.buttonMoveImage.Text = "Move Image(s)";
            this.buttonMoveImage.UseVisualStyleBackColor = true;
            this.buttonMoveImage.Click += new System.EventHandler(this.buttonMoveImage_Click);
            // 
            // buttonDeleteImage
            // 
            this.buttonDeleteImage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonDeleteImage.Location = new System.Drawing.Point(217, 10);
            this.buttonDeleteImage.Name = "buttonDeleteImage";
            this.buttonDeleteImage.Size = new System.Drawing.Size(100, 25);
            this.buttonDeleteImage.TabIndex = 28;
            this.buttonDeleteImage.Text = "Delete Image(s)";
            this.buttonDeleteImage.UseVisualStyleBackColor = true;
            this.buttonDeleteImage.Click += new System.EventHandler(this.buttonDeleteImage_Click);
            // 
            // comboBoxSelectStyle
            // 
            this.comboBoxSelectStyle.DisplayMember = "Sel";
            this.comboBoxSelectStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelectStyle.FormattingEnabled = true;
            this.comboBoxSelectStyle.Items.AddRange(new object[] {
            "Fill",
            "Stretch",
            "Zoom"});
            this.comboBoxSelectStyle.Location = new System.Drawing.Point(12, 492);
            this.comboBoxSelectStyle.Name = "comboBoxSelectStyle";
            this.comboBoxSelectStyle.Size = new System.Drawing.Size(80, 21);
            this.comboBoxSelectStyle.TabIndex = 34;
            this.comboBoxSelectStyle.Tag = "";
            this.comboBoxSelectStyle.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectStyle_SelectedIndexChanged);
            // 
            // labelTimeLeft
            // 
            this.labelTimeLeft.AutoSize = true;
            this.labelTimeLeft.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelTimeLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelTimeLeft.Location = new System.Drawing.Point(9, 525);
            this.labelTimeLeft.Name = "labelTimeLeft";
            this.labelTimeLeft.Size = new System.Drawing.Size(76, 13);
            this.labelTimeLeft.TabIndex = 36;
            this.labelTimeLeft.Text = "Awaiting Timer";
            // 
            // comboBoxWallpaperInterval
            // 
            this.comboBoxWallpaperInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWallpaperInterval.FormattingEnabled = true;
            this.comboBoxWallpaperInterval.Items.AddRange(new object[] {
            "None",
            "5 sec",
            "10 sec",
            "15 sec",
            "30 sec",
            "45 sec",
            "1 min",
            "2 min",
            "3 min",
            "5 min",
            "10 min",
            "15 min",
            "30 min",
            "45 min",
            "1 hour",
            "2 hours",
            "3 hours",
            "5 hours",
            "6 hours",
            "8 hours",
            "10 hours",
            "12 hours",
            "16 hours",
            "20 hours",
            "1 day",
            "3 days",
            "1 week"});
            this.comboBoxWallpaperInterval.Location = new System.Drawing.Point(12, 541);
            this.comboBoxWallpaperInterval.Name = "comboBoxWallpaperInterval";
            this.comboBoxWallpaperInterval.Size = new System.Drawing.Size(80, 21);
            this.comboBoxWallpaperInterval.TabIndex = 35;
            this.comboBoxWallpaperInterval.SelectedIndexChanged += new System.EventHandler(this.comboBoxWallpaperInterval_SelectedIndexChanged);
            // 
            // labelWallpaperStyle
            // 
            this.labelWallpaperStyle.AutoSize = true;
            this.labelWallpaperStyle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelWallpaperStyle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelWallpaperStyle.Location = new System.Drawing.Point(9, 475);
            this.labelWallpaperStyle.Name = "labelWallpaperStyle";
            this.labelWallpaperStyle.Size = new System.Drawing.Size(81, 13);
            this.labelWallpaperStyle.TabIndex = 37;
            this.labelWallpaperStyle.Text = "Wallpaper Style";
            // 
            // panelImageSelector
            // 
            this.panelImageSelector.Controls.Add(this.tabControlImagePages);
            this.panelImageSelector.Controls.Add(this.labelSelectedImage);
            this.panelImageSelector.Location = new System.Drawing.Point(618, 0);
            this.panelImageSelector.Name = "panelImageSelector";
            this.panelImageSelector.Size = new System.Drawing.Size(645, 639);
            this.panelImageSelector.TabIndex = 38;
            this.panelImageSelector.Visible = false;
            // 
            // tabControlImagePages
            // 
            this.tabControlImagePages.ItemSize = new System.Drawing.Size(20, 18);
            this.tabControlImagePages.Location = new System.Drawing.Point(0, 25);
            this.tabControlImagePages.Name = "tabControlImagePages";
            this.tabControlImagePages.SelectedIndex = 0;
            this.tabControlImagePages.Size = new System.Drawing.Size(647, 612);
            this.tabControlImagePages.TabIndex = 4;
            // 
            // labelSelectedImage
            // 
            this.labelSelectedImage.AutoSize = true;
            this.labelSelectedImage.ForeColor = System.Drawing.Color.White;
            this.labelSelectedImage.Location = new System.Drawing.Point(3, 9);
            this.labelSelectedImage.Name = "labelSelectedImage";
            this.labelSelectedImage.Size = new System.Drawing.Size(144, 13);
            this.labelSelectedImage.TabIndex = 3;
            this.labelSelectedImage.Text = "Select an image for more info";
            // 
            // panelImageInspector
            // 
            this.panelImageInspector.Controls.Add(this.inspector_axWindowsMediaPlayer);
            this.panelImageInspector.Controls.Add(this.inspector_buttonRight);
            this.panelImageInspector.Controls.Add(this.inspector_buttonLeft);
            this.panelImageInspector.Controls.Add(this.inspector_buttonPlus);
            this.panelImageInspector.Controls.Add(this.inspector_textBoxRankEditor);
            this.panelImageInspector.Controls.Add(this.inspector_buttonMinus);
            this.panelImageInspector.Controls.Add(this.inspector_pictureBoxImage);
            this.panelImageInspector.Location = new System.Drawing.Point(618, 25);
            this.panelImageInspector.Name = "panelImageInspector";
            this.panelImageInspector.Size = new System.Drawing.Size(645, 612);
            this.panelImageInspector.TabIndex = 39;
            this.panelImageInspector.Visible = false;
            // 
            // inspector_axWindowsMediaPlayer
            // 
            this.inspector_axWindowsMediaPlayer.Enabled = true;
            this.inspector_axWindowsMediaPlayer.Location = new System.Drawing.Point(0, 0);
            this.inspector_axWindowsMediaPlayer.Name = "inspector_axWindowsMediaPlayer";
            this.inspector_axWindowsMediaPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("inspector_axWindowsMediaPlayer.OcxState")));
            this.inspector_axWindowsMediaPlayer.Size = new System.Drawing.Size(645, 576);
            this.inspector_axWindowsMediaPlayer.TabIndex = 34;
            // 
            // inspector_buttonRight
            // 
            this.inspector_buttonRight.Location = new System.Drawing.Point(620, 582);
            this.inspector_buttonRight.Name = "inspector_buttonRight";
            this.inspector_buttonRight.Size = new System.Drawing.Size(20, 20);
            this.inspector_buttonRight.TabIndex = 33;
            this.inspector_buttonRight.Text = ">";
            this.inspector_buttonRight.UseVisualStyleBackColor = true;
            // 
            // inspector_buttonLeft
            // 
            this.inspector_buttonLeft.Location = new System.Drawing.Point(7, 582);
            this.inspector_buttonLeft.Name = "inspector_buttonLeft";
            this.inspector_buttonLeft.Size = new System.Drawing.Size(20, 20);
            this.inspector_buttonLeft.TabIndex = 32;
            this.inspector_buttonLeft.Text = "<";
            this.inspector_buttonLeft.UseVisualStyleBackColor = true;
            // 
            // inspector_buttonPlus
            // 
            this.inspector_buttonPlus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("inspector_buttonPlus.BackgroundImage")));
            this.inspector_buttonPlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.inspector_buttonPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inspector_buttonPlus.Location = new System.Drawing.Point(356, 582);
            this.inspector_buttonPlus.Name = "inspector_buttonPlus";
            this.inspector_buttonPlus.Size = new System.Drawing.Size(20, 20);
            this.inspector_buttonPlus.TabIndex = 8;
            this.inspector_buttonPlus.UseVisualStyleBackColor = true;
            // 
            // inspector_textBoxRankEditor
            // 
            this.inspector_textBoxRankEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.inspector_textBoxRankEditor.ForeColor = System.Drawing.Color.White;
            this.inspector_textBoxRankEditor.Location = new System.Drawing.Point(299, 582);
            this.inspector_textBoxRankEditor.Name = "inspector_textBoxRankEditor";
            this.inspector_textBoxRankEditor.Size = new System.Drawing.Size(50, 20);
            this.inspector_textBoxRankEditor.TabIndex = 7;
            this.inspector_textBoxRankEditor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // inspector_buttonMinus
            // 
            this.inspector_buttonMinus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("inspector_buttonMinus.BackgroundImage")));
            this.inspector_buttonMinus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.inspector_buttonMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inspector_buttonMinus.Location = new System.Drawing.Point(271, 582);
            this.inspector_buttonMinus.Name = "inspector_buttonMinus";
            this.inspector_buttonMinus.Size = new System.Drawing.Size(20, 20);
            this.inspector_buttonMinus.TabIndex = 6;
            this.inspector_buttonMinus.UseVisualStyleBackColor = true;
            // 
            // inspector_pictureBoxImage
            // 
            this.inspector_pictureBoxImage.Location = new System.Drawing.Point(0, 0);
            this.inspector_pictureBoxImage.Name = "inspector_pictureBoxImage";
            this.inspector_pictureBoxImage.Size = new System.Drawing.Size(647, 576);
            this.inspector_pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.inspector_pictureBoxImage.TabIndex = 0;
            this.inspector_pictureBoxImage.TabStop = false;
            // 
            // labelImageSize
            // 
            this.labelImageSize.AutoSize = true;
            this.labelImageSize.ForeColor = System.Drawing.Color.White;
            this.labelImageSize.Location = new System.Drawing.Point(588, 10);
            this.labelImageSize.Name = "labelImageSize";
            this.labelImageSize.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelImageSize.Size = new System.Drawing.Size(24, 13);
            this.labelImageSize.TabIndex = 34;
            this.labelImageSize.Text = "0x0";
            // 
            // timerVisualUpdater
            // 
            this.timerVisualUpdater.Enabled = true;
            this.timerVisualUpdater.Interval = 1000;
            this.timerVisualUpdater.Tick += new System.EventHandler(this.timerVisualUpdater_Tick);
            // 
            // WallpaperManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1264, 686);
            this.Controls.Add(this.labelImageSize);
            this.Controls.Add(this.labelWallpaperStyle);
            this.Controls.Add(this.labelTimeLeft);
            this.Controls.Add(this.comboBoxWallpaperInterval);
            this.Controls.Add(this.comboBoxSelectStyle);
            this.Controls.Add(this.panelImageSettings);
            this.Controls.Add(this.buttonOptions);
            this.Controls.Add(this.buttonTagSettings);
            this.Controls.Add(this.buttonUpdateTheme);
            this.Controls.Add(this.buttonLoadTheme);
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.buttonSaveTheme);
            this.Controls.Add(this.panelImageInspector);
            this.Controls.Add(this.panelImageSelector);
            this.Controls.Add(this.panelImageFolders);
            this.Controls.Add(this.buttonChangeDisplay);
            this.Controls.Add(this.buttonPreviousWallpaper);
            this.Controls.Add(this.buttonNextWallpaper);
            this.Name = "WallpaperManager";
            this.Text = "WallpaperManager";
            this.Click += new System.EventHandler(this.WallpaperManager_Click);
            this.panelImageFolders.ResumeLayout(false);
            this.panelImageSettings.ResumeLayout(false);
            this.panelImageSelector.ResumeLayout(false);
            this.panelImageSelector.PerformLayout();
            this.panelImageInspector.ResumeLayout(false);
            this.panelImageInspector.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inspector_axWindowsMediaPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inspector_pictureBoxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIconWallpaperManager;
        private System.Windows.Forms.Button buttonNextWallpaper;
        private System.Windows.Forms.Button buttonPreviousWallpaper;
        private System.Windows.Forms.Button buttonChangeDisplay;
        private System.Windows.Forms.Panel panelImageFolders;
        private System.Windows.Forms.Button buttonAddFolder;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelImageFolders;
        private System.Windows.Forms.Button buttomRemoveFolder;
        private System.Windows.Forms.Button buttonSaveTheme;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonLoadTheme;
        private System.Windows.Forms.Button buttonUpdateTheme;
        private System.Windows.Forms.Button buttonTagSettings;
        private System.Windows.Forms.Button buttonOptions;
        private System.Windows.Forms.Panel panelImageSettings;
        private System.Windows.Forms.ComboBox comboBoxSelectStyle;
        private System.Windows.Forms.Button buttonDeleteImage;
        private System.Windows.Forms.Button buttonMoveImage;
        private System.Windows.Forms.Label labelTimeLeft;
        private System.Windows.Forms.ComboBox comboBoxWallpaperInterval;
        private System.Windows.Forms.Label labelWallpaperStyle;
        private System.Windows.Forms.Panel panelImageSelector;
        private System.Windows.Forms.Label labelSelectedImage;
        private System.Windows.Forms.TabControl tabControlImagePages;
        private System.Windows.Forms.Button buttonClearSelection;
        private System.Windows.Forms.Button buttonInspectImage;
        private System.Windows.Forms.Panel panelImageInspector;
        private System.Windows.Forms.PictureBox inspector_pictureBoxImage;
        private System.Windows.Forms.Button inspector_buttonRight;
        private System.Windows.Forms.Button inspector_buttonLeft;
        private System.Windows.Forms.Button inspector_buttonPlus;
        private System.Windows.Forms.TextBox inspector_textBoxRankEditor;
        private System.Windows.Forms.Button inspector_buttonMinus;
        private System.Windows.Forms.Button buttonNameImage;
        private System.Windows.Forms.Label labelImageSize;
        private System.Windows.Forms.Timer timerVisualUpdater;
        private System.Windows.Forms.Button buttonRankImages;
        private AxWMPLib.AxWindowsMediaPlayer inspector_axWindowsMediaPlayer;
    }
}

