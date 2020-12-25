using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public partial class TagTabControl : UserControl
    {
        private HashSet<string> loadedCategories = new HashSet<string>();
        private readonly Color defaultTabColor = Color.FromArgb(32, 32, 32);

        public void CategoryChanged(object sender, EventArgs e)
        {
            if (tabControlImageTagger.TabPages.Count > 0)
            {
                if (ParentTagForm == null || ParentTagForm.ActiveCategory != WallpaperData.TaggingInfo.GetCategory(tabControlImageTagger.SelectedIndex)) // prevents this from being called when dragging tabs
                {
                    LoadTagContainer(tabControlImageTagger.SelectedIndex);

                    // This can only be called after ActiveCategory has been set
                    ParentTagForm?.UpdateCategoryControls();
                }
            }
        }

        public bool CreateCategory(CategoryData category)
        {
            //!TaggingTools.GetCategoryTagContainer(category, this) = null; // needs to be refreshed for a number of reasons such as: Resetting events & reloading disposed objects
            return CreateCategory(category.Name, category.Tags, category.Enabled, category.UseForNaming);
        }

        public bool CreateCategory(string categoryName, HashSet<TagData> tags = null, bool enabled = true, bool useForNaming = true)
        {
            if (loadedCategories.Contains(categoryName)) return false;

            if (!WallpaperData.TaggingInfo.ContainsCategory(categoryName))
            {
                tags = tags ?? new HashSet<TagData>();
                CategoryData newCategory = new CategoryData(categoryName, tags, enabled, useForNaming);
                newCategory.Initialize(true);
            }

            SuspendLayout();
            TabPage categoryTabPage = new TabPage
            {
                Text = categoryName,
                BackColor = defaultTabColor
            };

            tabControlImageTagger.TabPages.Add(categoryTabPage);
            ResumeLayout();

            loadedCategories.Add(categoryName);
            return true;
        }

        public void RemoveCategory(CategoryData category)
        {
            RemoveCategory(WallpaperData.TaggingInfo.GetCategoryIndex(category));
        }

        public void RemoveCategory(string categoryName)
        {
            for (int i = 0; i < WallpaperData.TaggingInfo.CategoryCount(); i++)
            {
                if (WallpaperData.TaggingInfo.GetCategory(i).Name == categoryName)
                {
                    RemoveCategory(i);
                    return;
                }
            }

            MessageBox.Show(categoryName + " does not exist");
        }

        public void RemoveCategory(int categoryIndex)
        {
            if (categoryIndex != -1)
            {
                CategoryData category = WallpaperData.TaggingInfo.GetCategory(categoryIndex);

                loadedCategories.Remove(category.Name);
                WallpaperData.TaggingInfo.RemoveCategory(category);
                tabControlImageTagger.TabPages[categoryIndex].Dispose();
            }
            else
            {
                MessageBox.Show(WallpaperData.TaggingInfo.GetCategory(categoryIndex).Name + " does not exist");
            }
        }
    }
}
