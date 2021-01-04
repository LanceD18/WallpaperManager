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
using LanceTools.FormUtil;
using LanceTools.Mpv;
using Mpv.NET.Player;
using WallpaperManager.ApplicationData;
using WallpaperManager.Tagging;

namespace WallpaperManager.ImageSelector
{
    public partial class ImageEditorForm : Form
    {
        private WallpaperData.ImageData activeImage;
        private Dictionary<string, HashSet<Tuple<string, Button>>> tagButtons = new Dictionary<string, HashSet<Tuple<string, Button>>>();

        private HashSet<string> tagNames = new HashSet<string>();

        private Button activeButton;

        private TagClickerForm tagClickerForm;

        public ImageEditorForm(WallpaperData.ImageData imageData)
        {
            InitializeComponent();

            activeImage = imageData;

            if (activeImage.imageType != ImageType.Video)
            {
                pictureBoxImage.ImageLocation = imageData.Path;
                pictureBoxImage.Visible = true; axWindowsMediaPlayer.Visible = false;
            }
            else
            {
                pictureBoxImage.Visible = false; axWindowsMediaPlayer.Visible = true;
                axWindowsMediaPlayer.settings.autoStart = true;
                axWindowsMediaPlayer.settings.setMode("loop", true);
                axWindowsMediaPlayer.URL = imageData.Path;
                axWindowsMediaPlayer.settings.volume = imageData.VideoSettings.Volume;
                axWindowsMediaPlayer.settings.rate = imageData.VideoSettings.PlaybackSpeed;
            }

            imageTagsFLP.SuspendLayout();
            foreach (CategoryData category in WallpaperData.TaggingInfo.GetAllCategories())
            {
                string categoryName = category.Name;
                if (imageData.Tags.ContainsKey(categoryName)) // remember that imageData.Tags may be ordered improperly
                {
                    tagButtons.Add(categoryName, new HashSet<Tuple<string, Button>>());

                    HashSet<string> orderedTags = (
                        from t
                            in imageData.Tags[categoryName]
                        orderby WallpaperData.TaggingInfo.GetTag(category, t).GetLinkedImageCount()
                            descending
                        select t).ToHashSet();

                    foreach (string tag in orderedTags)
                    {
                        CreateTagButton(categoryName, tag);
                    }
                }
            }
            imageTagsFLP.ResumeLayout();

            if (Options.OptionsData.ThemeOptions.EnableDetectionOfInactiveImages)
            {
                if (!activeImage.Active) // the Enabled setting already shows by default
                {
                    labelEnabled.Text = "Disabled";
                    labelEnabled.ForeColor = Color.Red;
                }
            }
            else // no need to see this labeled if you're only able to select enabled images
            {
                labelEnabled.Visible = false;
            }
        }

        private void buttonAddTag_Click(object sender, EventArgs e)
        {
            if (tagClickerForm == null || tagClickerForm.IsDisposed || !tagClickerForm.Visible)
            {
                tagClickerForm = new TagClickerForm(0, TagFormStyle.Adder, activeImage, tagClickEvent: TagClickerResponse);
                tagClickerForm.Show();
            }
            else
            {
                tagClickerForm.Focus();
            }
        }

        private void TagClickerResponse(TagData tag)
        {
            if (!tagNames.Contains(tag.Name))
            {
                CreateTagButton(tag.ParentCategoryName, tag.Name);

                foreach (Tuple<string, string> parentTag in tag.ParentTags)
                {
                    if (!tagNames.Contains(tag.Name))
                    {
                        CreateTagButton(parentTag.Item1, parentTag.Item2);
                    }
                }
            }
        }

        private void CreateTagButton(string category, string tag)
        {
            Button tagButton = new Button
            {
                BackColor = Color.White,
                ForeColor = activeImage.TagNamingExceptions.Contains(new Tuple<string, string>(category, tag)) ? Color.Red : Color.Black,
                AutoSize = true,
                Text = tag + " (" + WallpaperData.TaggingInfo.GetTag(category, tag).ParentCategoryName + ")"
            };
            tagButton.Click += TagButton_OpenOptionsDialog;

            imageTagsFLP.Controls.Add(tagButton);

            if (!tagButtons.ContainsKey(category))
            {
                tagButtons.Add(category, new HashSet<Tuple<string, Button>>());
            }

            tagButtons[category].Add(new Tuple<string, Button>(tag, tagButton));
            tagNames.Add(tag);
        }

        public void TagButton_OpenOptionsDialog(object sender, EventArgs e)
        {
            activeButton = sender as Button;

            Button removeButton = new Button
            {
                AutoSize = true,
                Text = "Remove Tag"
            };
            removeButton.Click += TagButton_Remove;

            Button exceptionButton = new Button
            {
                AutoSize = true,
                Text = "Toggle Naming Exception"
            };
            exceptionButton.Click += TagButton_ToggleTagNamingException;

            MessageBoxDynamic.Show("Choose an option", "(Naming exceptions allow a tag to be used for naming regardless of constraints)" + 
                                                       "\n(Tags with a naming exception will be colored red)", 
                new Button[] {removeButton, exceptionButton}, true);
        }

        public void TagButton_Remove(object sender, EventArgs e)
        {
            string tagName = TaggingTools.GetButtonTagName(activeButton);

            // Get the tag
            TagData tag = null;
            foreach (string category in tagButtons.Keys)
            {
                foreach (Tuple<string, Button> tagTuple in tagButtons[category])
                {
                    if (activeButton == tagTuple.Item2)
                    {
                        tag = WallpaperData.TaggingInfo.GetTag(category, tagTuple.Item1);
                    }
                }
            }

            if (tag != null)
            {
                // Determine if a child tag of this tag is present in the image, if so then the tag cannot be removed
                if (activeImage.CheckIfTagIsParent(tag, out var childTagName))
                {
                    MessageBox.Show("You cannot remove the tag " + tag.Name + " while it's child tag " + childTagName + " is also tagged");
                    return;
                }

                activeImage.RemoveTag(tag);

                // remove tagbutton
                foreach (Control control in imageTagsFLP.Controls)
                {
                    string controlTagName = TaggingTools.GetButtonTagName(control as Button);
                    if (controlTagName == tag.Name)
                    {
                        Tuple<string, Button> tagTuple = new Tuple<string, Button>(tag.Name, control as Button);
                        tagButtons[tag.ParentCategoryName].Remove(tagTuple);
                        tagNames.Remove(tag.Name);
                        imageTagsFLP.Controls.Remove(control);
                    }
                }
            }
            else
            {
                MessageBox.Show("Error: Tag " + tagName + " was not found");
            }
        }

        public void TagButton_ToggleTagNamingException(object sender, EventArgs e)
        {
            bool givenException = activeImage.ToggleTagNamingException(TaggingTools.GetButtonTagName(activeButton));
            activeButton.ForeColor = givenException ? Color.Red : Color.Black;
        }
    }
}
