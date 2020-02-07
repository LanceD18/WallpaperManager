using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Tagging
{
    public partial class TagClickerForm : Form
    {
        public TagTabControl tagTabControl { get; private set; }

        public TagClickerForm(int startingTabIndex, TagFormStyle tagFormStyle, WallpaperData.ImageData activeImage = null, TagData activeTag = null)
        {
            InitializeComponent();

            tagTabControl = new TagTabControl(startingTabIndex, tagFormStyle, activeImage, activeTag);

            //this.Size = tagTabControl.Size;
            Controls.Add(tagTabControl);
            tagTabControl.Location = new Point(0,0);
        }
    }
}
