using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools.FormUtil;

namespace WallpaperManager
{
    public enum SelectionType
    {
        None,
        Active,
        All
    }

    public static class WallpaperManagerTools
    {
        public static SelectionType ChooseSelectionType()
        {
            SelectionType selectionType = SelectionType.None;

            Button selectedImageButton = new Button();
            selectedImageButton.AutoSize = true;
            selectedImageButton.Text = "Active Selected Image";
            selectedImageButton.Click += (o, i) => { selectionType = SelectionType.Active; };

            Button allImagesButton = new Button();
            allImagesButton.AutoSize = true;
            allImagesButton.Text = "All Selected Images";
            allImagesButton.Click += (o, i) => { selectionType = SelectionType.All; };

            MessageBoxDynamic.Show("Choose a selection type", "Choose an option", new Button[] { selectedImageButton, allImagesButton }, true);

            return selectionType;
        }
    }
}
