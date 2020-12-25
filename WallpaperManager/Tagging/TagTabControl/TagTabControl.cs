using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public partial class TagTabControl : UserControl
    {
        public TagForm ParentTagForm { get; private set; }
        private TagFormStyle tagFormStyle;
        private WallpaperData.ImageData activeImage;
        private TagData activeTag;

        private Size initialItemSize;

        public TabControl.TabPageCollection TabPages => tabControlImageTagger.TabPages;

        public TabPage SelectedTab => tabControlImageTagger.SelectedTab;

        private Action<TagData> tagClickEvent;

        public TagTabControl(int startingTabIndex, TagFormStyle tagFormStyle = TagFormStyle.Editor,
            WallpaperData.ImageData activeImage = null, TagData activeTag = null, TagForm parentTagForm = null, Action<TagData> tagClickEvent = null)
        {
            InitializeComponent();

            ParentTagForm = parentTagForm;
            this.activeImage = activeImage;
            this.activeTag = activeTag;
            this.tagFormStyle = tagFormStyle;
            this.tagClickEvent = tagClickEvent;

            this.tabControlImageTagger.MouseDown += tabControlImageTagger_MouseDown;
            this.tabControlImageTagger.MouseUp += tabControlImageTagger_MouseUp;
            this.tabControlImageTagger.MouseMove += tabControlImageTagger_MouseMove;
            this.tabControlImageTagger.DragOver += tabControlImageTagger_DragOver;
            this.tabControlImageTagger.DragOver += tagContainerButton_DragOver;
            this.tabControlImageTagger.DragDrop += tagContainerButton_DragDrop;
            this.tabControlImageTagger.SelectedIndexChanged += CategoryChanged;

            tabControlImageTagger.ItemSize = new Size(28, 18);
            initialItemSize = tabControlImageTagger.ItemSize;

            if (WallpaperData.TaggingInfo.CategoryCount() > 0)
            {
                LoadTaggingInfo();
            }
            else
            {
                CreateCategory("Default");
            }

            tabControlImageTagger.SelectedIndex = startingTabIndex;
            LoadTagContainer(startingTabIndex);
            ParentTagForm?.UpdateCategoryControls();
        }

        private void LoadTaggingInfo()
        {
            foreach (CategoryData category in WallpaperData.TaggingInfo.GetAllCategories())
            {
                CreateCategory(category);
            }
        }

        public void LoadTagContainer(int categoryIndex)
        {
            CategoryData category = WallpaperData.TaggingInfo.GetCategory(categoryIndex);
            TabPage categoryTab = tabControlImageTagger.TabPages[categoryIndex];

            if (categoryTab.Controls.Count == 0) // first time loading this page
            {
                if (TaggingTools.GetCategoryTagContainer(category, this) == null)
                {
                    SuspendLayout();
                    Size tagContainerSize = new Size(tabControlImageTagger.Size.Width - 5, tabControlImageTagger.Size.Height - initialItemSize.Height - 10);
                    TagContainer categoryTagContainer = new TagContainer(category, tagContainerSize, this, tagFormStyle, activeImage, activeTag, tagClickEvent)
                    {
                        AllowDrop = true,
                        Location = new Point(-2, 0)
                    };

                    categoryTab.Controls.Add(categoryTagContainer);
                    ResumeLayout();
                }

            }
            else
            {
                Debug.WriteLine("Validating Sort Options");
                TaggingTools.GetCategoryTagContainer(category, this).ValidateSortOptions();
            }
            if (ParentTagForm != null)
            {
                ParentTagForm.ActiveCategory = category;
            }
        }

    }
}
