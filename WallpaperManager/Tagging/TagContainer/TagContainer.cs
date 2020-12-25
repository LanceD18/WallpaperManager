using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools.FormUtil;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public partial class TagContainer : UserControl
    {
        private CategoryData ActiveCategory;
        public TagTabControl ParentTagTabControl { get; }
        private TagFormStyle tagFormStyle;
        private WallpaperData.ImageData activeImage;
        private TagData activeTag;

        private List<TagData> orderedTags;

        private int page;
        private int Page
        {
            get => page;

            set
            {
                page = value;
                labelPageNumber.Text = value.ToString();
            }
        }

        private int currentStartIndex = 0;
        private LinkedList<int> pageIndexList = new LinkedList<int>();

        private bool OnLastPage = true;

        private SortType sortType = SortType.Count;
        private SortDirection sortDirection = SortDirection.Decreasing;

        private enum LoadDirection
        {
            Left,
            Right,
            None
        }

        private enum SortType
        {
            None,
            Count,
            Name
        }

        private enum SortDirection
        {
            None,
            Decreasing,
            Increasing
        }

        public TagContainer(CategoryData parentCategory, Size size, TagTabControl parentTagTabControl, 
            TagFormStyle tagFormStyle = TagFormStyle.Editor, WallpaperData.ImageData activeImage = null, TagData activeTag = null)
        {
            InitializeComponent();

            Page = 1;
            this.ParentTagTabControl = parentTagTabControl;
            this.ActiveCategory = parentCategory;

            this.tagFormStyle = tagFormStyle;
            this.activeImage = activeImage;
            this.activeTag = activeTag;

            // Resize TagContainer
            Size = size;
            buttonRight.Location = new Point(Size.Width - buttonRight.Width - 5, (Size.Height / 2) - (buttonRight.Size.Height / 2));
            buttonLeft.Location = new Point(buttonLeft.Location.X, (Size.Height / 2) - (buttonLeft.Size.Height / 2));
            buttonSort.Location = new Point(Size.Width - buttonSort.Width - 5, buttonSort.Location.Y);
            tagContainerFLP.Size = new Size(Size.Width - 62, Size.Height - tagContainerFLP.Bounds.Top);
            textBoxSearchTag.Size = new Size(buttonSort.Bounds.Left - textBoxSearchTag.Bounds.Left - 5, textBoxSearchTag.Size.Height);

            LoadSortOptions(false);
            LoadPage(0, LoadDirection.None);
        }

        public void RefreshTagCount()
        {
            foreach (Control control in tagContainerFLP.Controls)
            {
                UpdateTagButtonImageCount(WallpaperData.TaggingInfo.GetTag(ActiveCategory, TaggingTools.GetButtonTagName(control as Button)));
            }
        }

        private void LoadPage(int startIndex, LoadDirection loadDirection)
        {
            currentStartIndex = startIndex;

            tagContainerFLP.SuspendLayout();
            tagContainerFLP.Flush();
            tagContainerFLP.ResumeLayout();

            //! Cannot use SuspendLayout here as it'll mess up the programming behind the positioning of the buttons
            for (int i = startIndex; i < orderedTags.Count; i++)
            {
                TagData tag = orderedTags[i];

                if (tag.Name.ToLower().Contains(textBoxSearchTag.Text.ToLower()))
                {
                    Button tagButton = CreateTagButton(tag);

                    if (tagButton.Bounds.Bottom >= tagContainerFLP.Size.Height)
                    {
                        OnLastPage = false;

                        switch (loadDirection)
                        {
                            case LoadDirection.Left: // Updating Last Value
                                pageIndexList.Last.Value = i;
                                break;

                            case LoadDirection.Right: // Adding Last Value
                                pageIndexList.AddLast(i);
                                break;

                            case LoadDirection.None: // Refreshing Page
                                if (pageIndexList.Last != null)
                                {
                                    if (pageIndexList.Last.Value <= startIndex) // Adding Last Value
                                    {
                                        pageIndexList.AddLast(i);
                                    }
                                    else // Updating Last Value
                                    {
                                        pageIndexList.Last.Value = i;
                                    }
                                }
                                else // Adding First Value (also fixing specific scenarios)
                                {
                                    pageIndexList.AddLast(i);
                                }
                                break;
                        }

                        tagButton.Dispose();
                        break;
                    }
                    else
                    {
                        OnLastPage = true;
                    }
                }
            }

            // this occurs when you remove enough tags to make the current page the last page
            if (pageIndexList.Last?.Value >= orderedTags.Count && startIndex != pageIndexList.Last?.Value)
            {
                pageIndexList.RemoveLast();
            }
        }

        private void LoadSortOptions(bool refreshPage)
        {
            string sortTypeString = WallpaperData.TagSortOption.Substring(0, WallpaperData.TagSortOption.IndexOf('_'));
            string sortDirectionString = WallpaperData.TagSortOption.Substring(WallpaperData.TagSortOption.IndexOf('_') + 1);

            switch (sortTypeString)
            {
                case "Count":
                    sortType = SortType.Count;
                    break;

                case "Name":
                    sortType = SortType.Name;
                    break;
            }

            switch (sortDirectionString)
            {
                case "Decreasing":
                    sortDirection = SortDirection.Decreasing;
                    break;

                case "Increasing":
                    sortDirection = SortDirection.Increasing;
                    break;
            }

            UpdateSortOptions(refreshPage);
        }

        private void UpdateSortOptions(bool refreshPages)
        {
            switch (sortType)
            {
                case SortType.Count:
                    switch (sortDirection)
                    {
                        case SortDirection.Decreasing:
                            orderedTags = (from t in ActiveCategory.Tags orderby t.GetLinkedImageCount() descending select t).ToList();
                            WallpaperData.TagSortOption = "Count_Decreasing";
                            break;

                        case SortDirection.Increasing:
                            orderedTags = (from t in ActiveCategory.Tags orderby t.GetLinkedImageCount() ascending select t).ToList();
                            WallpaperData.TagSortOption = "Count_Increasing";
                            break;
                    }
                    break;

                case SortType.Name:
                    switch (sortDirection)
                    {
                        case SortDirection.Decreasing:
                            orderedTags = (from t in ActiveCategory.Tags orderby t.Name descending select t).ToList();
                            WallpaperData.TagSortOption = "Name_Decreasing";
                            break;

                        case SortDirection.Increasing:
                            orderedTags = (from t in ActiveCategory.Tags orderby t.Name ascending select t).ToList();
                            WallpaperData.TagSortOption = "Name_Increasing";
                            break;
                    }
                    break;
            }

            if (refreshPages)
            {
                ResetPages();
            }
        }

        /// <summary>
        /// Validates that the tag container's sort option matches the currently saved sort option. If not, update's container's sorting
        /// </summary>
        public void ValidateSortOptions()
        {
            string sortTypeString = WallpaperData.TagSortOption.Substring(0, WallpaperData.TagSortOption.IndexOf('_'));
            string sortDirectionString = WallpaperData.TagSortOption.Substring(WallpaperData.TagSortOption.IndexOf('_') + 1);

            bool validSortOptions = true;
            switch (sortTypeString)
            {
                case "Count":
                    if (sortType != SortType.Count) validSortOptions = false;
                    break;

                case "Name":
                    if (sortType != SortType.Name) validSortOptions = false;
                    break;
            }

            switch (sortDirectionString)
            {
                case "Decreasing":
                    if (sortDirection != SortDirection.Decreasing) validSortOptions = false;
                    break;

                case "Increasing":
                    if (sortDirection != SortDirection.Increasing) validSortOptions = false;
                    break;
            }

            Debug.WriteLine(validSortOptions);
            if (!validSortOptions)
            {
                LoadSortOptions(true);
            }
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            if (Page != 1)
            {
                Page--;

                if (!OnLastPage) // nothing will be added for the last page so there's no need to remove anything
                {
                    pageIndexList.RemoveLast(); // allows the list to shift back since the node to the next page is no longer needed
                }

                int startIndex = pageIndexList.Last?.Previous?.Value ?? 0; // AFTER removing, the last node holds the index for the current page so you'll need to go back once more

                LoadPage(startIndex, LoadDirection.Left);
            }
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            if (pageIndexList.Count > 0 && !OnLastPage)
            {
                Page++;

                int startIndex = pageIndexList.Last.Value;
                LoadPage(startIndex, LoadDirection.Right);
            }
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            SortType newSortType = SortType.None;
            SortDirection newSortDirection = SortDirection.None;

            Button countDecreasing = new Button
            {
                Text = "Count Decreasing",
                AutoSize = true
            };
            countDecreasing.Click += (o, i) => { newSortType = SortType.Count; newSortDirection = SortDirection.Decreasing; };

            Button countIncreasing = new Button
            {
                Text = "Count Increasing",
                AutoSize = true
            };
            countIncreasing.Click += (o, i) => { newSortType = SortType.Count; newSortDirection = SortDirection.Increasing; };

            Button nameDecreasing = new Button
            {
                Text = "Name Decreasing",
                AutoSize = true
            };
            nameDecreasing.Click += (o, i) => { newSortType = SortType.Name; newSortDirection = SortDirection.Decreasing; };

            Button nameIncreasing = new Button
            {
                Text = "Name Increasing",
                AutoSize = true
            };
            nameIncreasing.Click += (o, i) => { newSortType = SortType.Name; newSortDirection = SortDirection.Increasing; };

            MessageBoxDynamic.Show("Choose an option", "Select a sorting option", new Button[] { countDecreasing, countIncreasing, nameDecreasing, nameIncreasing }, true);

            if (sortType == newSortType && sortDirection == newSortDirection) return;
            if (newSortType == SortType.None && newSortDirection == SortDirection.None) return;

            sortType = newSortType;
            sortDirection = newSortDirection;

            UpdateSortOptions(true);
        }

        private void textBoxSearchTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ResetPages();
                e.SuppressKeyPress = true; // prevents windows 'ding' sound
            }
        }

        private void ResetPages()
        {
            Page = 1;
            currentStartIndex = 0;
            pageIndexList = new LinkedList<int>();

            LoadPage(0, LoadDirection.None);
        }

        public HashSet<Button> GetTagButtons()
        {
            HashSet<Button> buttons = new HashSet<Button>();
            foreach (Control control in tagContainerFLP.Controls)
            {
                buttons.Add(control as Button);
            }

            return buttons;
        }
    }
}
