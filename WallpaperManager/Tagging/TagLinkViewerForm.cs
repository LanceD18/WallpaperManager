using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public enum LinkType
    {
        Parent,
        Child
    }

    public partial class TagLinkViewerForm : Form
    {
        private TagData activeTag;

        public TagLinkViewerForm(TagData activeTag, LinkType linkType)
        {
            InitializeComponent();

            this.activeTag = activeTag;
            linkedTagsFLP.AutoScroll = true;
            linkedTagsFLP.HorizontalScroll.Visible = false;

            List<TagData> tagsToAdd = new List<TagData>();
            if (linkType == LinkType.Parent)
            {
                labelXTags.Text = "Parent Tags";
                foreach (Tuple<string, string> tagInfo in activeTag.ParentTags)
                {
                    tagsToAdd.Add(WallpaperData.TaggingInfo.GetTag(tagInfo.Item1, tagInfo.Item2));
                }
            }
            else
            {
                labelXTags.Text = "Child Tags";
                foreach (Tuple<string, string> tagInfo in activeTag.ChildTags)
                {
                    tagsToAdd.Add(WallpaperData.TaggingInfo.GetTag(tagInfo.Item1, tagInfo.Item2));
                }
            }

            foreach (TagData tag in tagsToAdd)
            {
                Button tagButton = new Button();
                tagButton.AutoSize = true;
                tagButton.Text = tag.Name + " (" + tag.ParentCategoryName + ")";
                tagButton.Click += buttonTag_Click;
                tagButton.ForeColor = Color.Black;
                tagButton.BackColor = Color.White;
                linkedTagsFLP.Controls.Add(tagButton);
            }
        }

        public void buttonTag_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Unlink tag?", "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Button tagButton = sender as Button;
                string tagName = TaggingTools.GetButtonTagName(tagButton);
                string categoryName = TaggingTools.GetButtonCategoryName(tagButton);
                TagData linkedTag = WallpaperData.TaggingInfo.GetTag(categoryName, tagName);

                foreach (Tuple<string, string> tagInfo in linkedTag.ChildTags)
                {
                    if (activeTag.ParentTags.Contains(tagInfo))
                    {
                        MessageBox.Show("You cannot unlink " + linkedTag.Name + " while it's child tag " + tagInfo.Item2 + " is also linked");
                        return;
                    }
                }

                if (MessageBox.Show("Would you like to also remove the tag " + tagName + " from images tagged with " + activeTag.Name + "?", "Choose an option", MessageBoxButtons.YesNo) 
                    == DialogResult.Yes)
                {
                    foreach (string image in activeTag.GetLinkedImages())
                    {
                        WallpaperData.GetImageData(image).RemoveTag(linkedTag);
                    }
                }

                activeTag.UnlinkTag(linkedTag);
                tagButton.Dispose();
            }
        }
    }
}
