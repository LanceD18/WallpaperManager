using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public partial class TagForm : Form
    {
        public CategoryData ActiveCategory;


        private TagFormStyle tagFormStyle;

        public TagTabControl TabControlImageTagger { get; private set; }

        public TagForm()
        {
            InitializeComponent();

            TabControlImageTagger = new TagTabControl(0, tagFormStyle, null, null, this);
            Controls.Add(TabControlImageTagger);
            TabControlImageTagger.Location = new Point(119, 8);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            // Reload tag data for just in case of potential changes
            TaggingTools.GetCategoryTagContainer(ActiveCategory, TabControlImageTagger)?.RefreshTagCount();
        }

        public void UpdateCategoryControls()
        {
            labelContainsTags.Text = "Contains " + ActiveCategory.Tags.Count + (ActiveCategory.Tags.Count != 1 ? " tags" : " tag");
            checkBoxEnabled.Checked = ActiveCategory.Enabled;
            checkBoxUseForNaming.Checked = ActiveCategory.UseForNaming;
        }
    }
}
