using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LevelEditor.CustomControls;

namespace LevelEditor.EditorForms
{
    public partial class test : Form
    {
        public s_mapdisp mapdisp;
        public tileditor tiled;

        public test()
        {
            InitializeComponent();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            tiled.SetScroller((int)MapXUpDown.Value, (int)MapYUpDown.Value);
            tiled.SetTilePicker();
            mapdisp.SetTileAndMapSize( 
                new Point(
                    (int)MapXUpDown.Value, 
                    (int)MapYUpDown.Value));
            tiled.directory = "";
            tiled.Text = tileditor.appNameText;
            DestroyHandle();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DestroyHandle();
        }
    }
}
