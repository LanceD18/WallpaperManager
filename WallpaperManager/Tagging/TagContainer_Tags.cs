using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools.FormUtil;
using Microsoft.VisualBasic;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public partial class TagContainer : UserControl
    {
        private Button CreateTagButton(TagData tag)
        {
            Button tagButton = new Button();
            tagButton.AutoSize = true;
            tagButton.BackColor = SystemColors.ButtonFace;
            TaggingTools.UpdateTagButton(tagButton, tag);

            ApplyTagFormStyle(ref tagButton);

            foreach (Control control in tagContainerFLP.Controls)
            {
                Button curTagButton = control as Button;
                if (tagButton.Text == curTagButton.Text) // this button already exists
                {
                    return curTagButton;
                }
            }

            tagContainerFLP.Controls.Add(tagButton);
            tagContainerFLP.Controls.SetChildIndex(tagButton, orderedTags.IndexOf(tag));

            return tagButton;
        }

        private void ApplyTagFormStyle(ref Button tagButton)
        {
            switch (tagFormStyle)
            {
                case TagFormStyle.Editor:
                    tagButton.Click += ButtonTagEditor;
                    tagButton.MouseDown += ParentTagTabControl.tagContainerButton_MouseDown;
                    tagButton.MouseUp += ParentTagTabControl.tagContainerButton_MouseUp;
                    tagButton.MouseMove += ParentTagTabControl.tagContainerButton_MouseMove;
                    tagButton.DragDrop += ParentTagTabControl.tagContainerButton_DragDrop;
                    break;

                case TagFormStyle.Adder:
                    tagButton.Click += ButtonTagAdder;
                    break;

                case TagFormStyle.Linker:
                    tagButton.Click += ButtonTagLinker;
                    break;
            }
        }

        private void ButtonTagEditor(object sender, EventArgs e)
        {
            Button selectedTagButton = sender as Button;
            TagData selectedTag = WallpaperData.TaggingInfo.GetTag(ActiveCategory, TaggingTools.GetButtonTagName(selectedTagButton));

            using (TagEditorForm f = new TagEditorForm(selectedTag, selectedTagButton, this.tagContainerFLP))
            {
                f.ShowDialog();
            }
            RefreshTagCount();
        }

        private void ButtonTagAdder(object sender, EventArgs e)
        {
            TagData selectedTag = GetSelectedTag(sender as Button);

            activeImage.AddTag(selectedTag);
            (ParentTagTabControl.Parent as Form).Close();
        }

        private void ButtonTagLinker(object sender, EventArgs e)
        {
            TagData selectedTag = GetSelectedTag(sender as Button);

            activeTag.LinkTag(selectedTag);
            UpdateTagButtonImageCount(selectedTag);

            (ParentTagTabControl.Parent as Form).Close();
        }

        private TagData GetSelectedTag(Button selectedTagButton)
        {
            return WallpaperData.TaggingInfo.GetTag(ActiveCategory, TaggingTools.GetButtonTagName(selectedTagButton));
        }

        // This is used to updated buttons that are already loaded, otherwise they'll be updated the next time their page is accessed
        public void UpdateTagButton(TagData tag)
        {
            Button tagButton = GetTagButton(tag);

            if (tagButton != null)
            {
                TaggingTools.UpdateTagButton(tagButton, tag);
            }
        }

        public void UpdateTagButtonImageCount(TagData tag)
        {
            Button tagButton = GetTagButton(tag);

            if (tagButton != null)
            {
                TaggingTools.UpdateTagButton(tagButton, tag);
            }
        }

        public Button GetTagButton(TagData tag)
        {
            foreach (Control control in tagContainerFLP.Controls)
            {
                Button tagButton = control as Button;
                if (TaggingTools.GetButtonTagName(tagButton) == tag.Name)
                {
                    return tagButton;
                }
            }

            return null;
        }

        public void InsertTag(TagData newTag)
        {
            if (newTag.IsInitialized)
            {
                CategoryData parentCategory = WallpaperData.TaggingInfo.GetTagParentCategory(newTag);

                /*x
                foreach (Control control in TaggingTools.GetCategoryTagContainer(parentCategory, ParentTagTabControl).tagContainerFLP.Controls)
                {
                    Button curTagButton = control as Button;
    
                    if (TaggingTools.GetButtonTagName(curTagButton) == newTag.Name)
                    {
                        MessageBox.Show(newTag.Name + " already exists in the " + parentCategory.Name + " category");
                        return false;
                    }
                }
                */

                if (newTag.GetLinkedImageCount() == 0) // this tag is completely new so there's no need to determine an insertion location
                {
                    orderedTags.Add(newTag);
                }
                else // tags must maintain ordering based on image count
                {
                    if (orderedTags.Count != 0)
                    {
                        for (int i = 0; i < orderedTags.Count; i++)
                        {
                            if (newTag.GetLinkedImageCount() > orderedTags[i].GetLinkedImageCount()) // inserting existing tag
                            {
                                orderedTags.Insert(i, newTag);
                                break;
                            }

                            if (i == orderedTags.Count - 1) // inserting smallest tag in container
                            {
                                orderedTags.Add(newTag);
                                break;
                            }
                        }
                    }
                    else // inserting first tag in container
                    {
                        orderedTags.Add(newTag);
                    }
                }

                LoadPage(currentStartIndex, LoadDirection.None);
                ParentTagTabControl.ParentTagForm.UpdateCategoryControls();
            }
        }

        public void RemoveTagFromActiveCategory(Button tagButton)
        {
            TagData tagToDelete = WallpaperData.TaggingInfo.GetTag(ActiveCategory, tagButton.Text.Substring(0, tagButton.Text.LastIndexOf('(') - 1));

            RemoveTag(tagToDelete, tagButton);
        }

        public void RemoveTag(TagData tagToDelete, Button tagButtonToDelete = null)
        {
            if (tagToDelete != null)
            {
                // Remove the tag from TagContainer and update controls | Check for if the button itself is present
                orderedTags.Remove(tagToDelete);
                if (tagButtonToDelete != null && tagContainerFLP.Contains(tagButtonToDelete))
                {
                    tagContainerFLP.Controls.Remove(tagButtonToDelete);
                    LoadPage(currentStartIndex, LoadDirection.None);
                }

                // Remove the tag itself
                WallpaperData.TaggingInfo.RemoveTag(tagToDelete);
                ParentTagTabControl.ParentTagForm.UpdateCategoryControls();
            }
            else
            {
                Debug.WriteLine("Attempted to remove a tag from a non-active category with TagContainer.RemoveTag()");
            }
        }

        public bool ContainsTag(TagData tag)
        {
            foreach (TagData curTag in orderedTags)
            {
                if (tag.Name == curTag.Name)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
