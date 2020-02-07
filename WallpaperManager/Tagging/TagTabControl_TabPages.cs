using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public partial class TagTabControl : UserControl
    {
        // derived from Fabio Lotti through https://stackoverflow.com/questions/4983361/how-to-make-tabpages-draggable

        public TabPage selectedTab { get; private set; }

        private void tabControlImageTagger_MouseDown(object sender, MouseEventArgs e)
        {
            selectedButtonPotentialTabPage = null; // prevents this from staying valid after releasing | MouseUp would require much more code to work properly
            selectedTab = GetPointedTab();
        }

        private void tabControlImageTagger_MouseUp(object sender, MouseEventArgs e)
        {
            selectedTab = null;
        }

        private void tabControlImageTagger_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && selectedTab != null)
            {
                tabControlImageTagger.DoDragDrop(selectedTab, DragDropEffects.Move);
            }
        }

        private void tabControlImageTagger_DragOver(object sender, DragEventArgs e)
        {
            if (selectedTab != null)
            {
                // requires tabControlImageTagger.AllowDrop to equal true to work
                TabPage draggedTab = (TabPage)e.Data.GetData(typeof(TabPage));
                TabPage pointedTab = GetPointedTab();
                
                if (draggedTab == selectedTab && pointedTab != null)
                {
                    e.Effect = DragDropEffects.Move;

                    if (pointedTab != draggedTab)
                    {
                        SwapTabPages(draggedTab, pointedTab);
                    }
                }
            }
        }

        private TabPage GetPointedTab()
        {
            for (int i = 0; i < tabControlImageTagger.TabPages.Count; i++)
            {
                if (tabControlImageTagger.GetTabRect(i).Contains(tabControlImageTagger.PointToClient(Cursor.Position)))
                {
                    return tabControlImageTagger.TabPages[i];
                }
            }

            return null;
        }

        private void SwapTabPages(TabPage srcTab, TabPage dstTab)
        {
            int srcIndex = tabControlImageTagger.TabPages.IndexOf(srcTab);
            int dstIndex = tabControlImageTagger.TabPages.IndexOf(dstTab);
            CategoryData srcCategory = WallpaperData.TaggingInfo.GetCategory(srcIndex);
            CategoryData dstCategory = WallpaperData.TaggingInfo.GetCategory(dstIndex);

            tabControlImageTagger.TabPages[dstIndex] = srcTab;
            tabControlImageTagger.TabPages[srcIndex] = dstTab;
            WallpaperData.TaggingInfo.SetCategory(dstIndex, srcCategory);
            WallpaperData.TaggingInfo.SetCategory(srcIndex, dstCategory);

            if (tabControlImageTagger.SelectedIndex == srcIndex)
            {
                tabControlImageTagger.SelectedIndex = dstIndex;
            }
            else if (tabControlImageTagger.SelectedIndex == dstIndex)
            {
                tabControlImageTagger.SelectedIndex = srcIndex;
            }

            tabControlImageTagger.Refresh();
        }

        // Moving Tags to other categories
        public Button selectedButton { get; private set; }
        public TabPage selectedButtonPotentialTabPage { get; private set; }

        public void tagContainerButton_MouseDown(object sender, MouseEventArgs e)
        {
            selectedButton = sender as Button;
        }

        public void tagContainerButton_MouseMove(object sender, MouseEventArgs e)
        {
            selectedButtonPotentialTabPage = null; // prevents this from staying valid after releasing | MouseUp would require much more code to work properly

            if (e.Button == MouseButtons.Left && selectedButton != null)
            {
                // the parent here will be tagContainerFLP
                (sender as Button).Parent.DoDragDrop(selectedButton, DragDropEffects.Move);
            }
        }

        public void tagContainerButton_MouseUp(object sender, MouseEventArgs e)
        {
            selectedButtonPotentialTabPage = null;
        }

        public void tagContainerButton_DragOver(object sender, DragEventArgs e)
        {
            if (selectedButton != null) // a button is being selected
            {
                // requires tabControlImageTagger.AllowDrop to equal true to work
                Button draggedButton = (Button)e.Data.GetData(typeof(Button));
                TabPage pointedTab = GetPointedTab();

                if (pointedTab != null && pointedTab != tabControlImageTagger.SelectedTab)
                {
                    e.Effect = DragDropEffects.Move;
                    selectedButtonPotentialTabPage = pointedTab;
                }
            }
        }

        public void tagContainerButton_DragDrop(object sender, DragEventArgs e)
        {
            if (selectedButtonPotentialTabPage != null)
            {
                string selectedTag = TaggingTools.GetButtonTagName(selectedButton);
                string selectedCategory = selectedButtonPotentialTabPage.Text;
                if (MessageBox.Show("Move " + selectedTag + " to " + selectedCategory + "?", "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ParentTagForm.MoveTag(selectedTag, selectedCategory);
                }
            }

            selectedButton = null;
            selectedButtonPotentialTabPage = null;
        }
    }
}
