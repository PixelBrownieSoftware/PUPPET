using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor.EditorForms
{
    public partial class s_settilesize : Form
    {
        tileditor tiled;
        Bitmap tileData;

        public s_settilesize(tileditor tiled, ref Bitmap tiledat)
        {
            this.tiled = tiled;
            tileData = tiledat;
            InitializeComponent();
            SetStuff();
        }

        public Point TileSize
        {
            get {
                return new Point((int)TileXupdown.Value, (int)TileYupdown.Value);
            }
        }

        public void SetStuff()
        {
            tiled.SetTilePicker();
            //I've decided to set the tilesize to 20x20 automatically
            //Previously you could set it to what you like, but I decided to remove that in order to not risk bugs

            //tiled.SetTileStuff(tileData, TileSize.X, TileSize.Y);

            tiled.SetTileStuff(tileData, 20, 20);
            DestroyHandle();
        }
        public void SetStuff2()
        {
            tiled.SetTilePicker();
            DestroyHandle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetStuff();
        }
        
    }
}
