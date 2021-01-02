using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mpv.NET.Player;
using LanceTools;

namespace WallpaperManager.ControlLibrary
{
    public partial class MpvDisplay : UserControl
    {
        //?VariableRef<MpvPlayer> player;

        public MpvDisplay()
        {
            InitializeComponent();
        }

        public void SetPlayer(MpvPlayer player)
        {
            //?this.player = new VariableRef<MpvPlayer>(() => player);
        }
    }
}
