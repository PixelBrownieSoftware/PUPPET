using System;
using System.IO;
using System.Drawing.Imaging;
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
    public partial class tileditor : Form
    {
        string tilesetDir;
        string tilesetDirSafe;
        List<ev_details> events;
        public const string appNameText = "Pixel\'s ultimate proprietary placement editing toolkit";
        int entitynum = 0;
        public string directory;

        public static string versionName = "MAP_MADE_WITH_PUPPET_V0.01";

        public void SetScroller(int x, int y)
        {
            vScrollBar1.Maximum = y;
            hScrollBar1.Maximum = x;
        }
        public void SetTilePicker()
        {
            s_mapdisp1.SetTileSize(s_tiledisp1.GetTileSize);
        }

        public tileditor()
        {
            InitializeComponent();
            ToolTab.SelectedIndexChanged += selection;
            s_mapdisp1.tiledisply = s_tiledisp1;
            s_tiledisp1.Initilaize(ref s_mapdisp1);
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }

        private void selection(object sender, EventArgs e)
        {
            TabControl tab = (TabControl)sender;
            int i = tab.SelectedIndex;
            s_mapdisp1.SELECT_MODE = (CustomControls.s_mapdisp.SELECTION_MODE)i;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (s_tiledisp1.GetTileSize == null)
                s_mapdisp1.LoadMap(LoadMap(), new Point(20,20));
            else
                s_mapdisp1.LoadMap(LoadMap(), s_tiledisp1.GetTileSize);
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.AddExtension = true;
            if (s.ShowDialog() == DialogResult.OK)
            {
                s_mapdisp1.Canvas.Save(s.FileName, ImageFormat.Bmp);
            }
        }
        public static Bitmap LoadMap()
        {
            OpenFileDialog fileDia = new OpenFileDialog();
            if (fileDia.ShowDialog() == DialogResult.OK)
            {
                return (Bitmap)Image.FromFile(fileDia.FileName);
            }
            return null;
        }

        public static void MapToBMP(Bitmap bmp)
        {
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test d1 = new test();
            d1.tiled = this;
            d1.mapdisp = s_mapdisp1;
            d1.ShowDialog();
        }

        private void s_tiledisp1_Click(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            s_mapdisp1.SetPosY(-e.NewValue * s_mapdisp1.GetTileSize().Y);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            s_mapdisp1.SetPosX(-e.NewValue * s_mapdisp1.GetTileSize().X);
        }

        public void SetTileStuff(Bitmap bit, int x, int y)
        {
            s_tiledisp1.SetTileset(bit);
            s_tiledisp1.SetTileSize(x, y);
        }

        private void LoadTileset_Click(object sender, EventArgs e)
        {
            LoadTilesetFromFile();
        }

        public void LoadTilesetFromFile()
        {
            OpenFileDialog openF = new OpenFileDialog();
            openF.Title = "Load tile-set";
            openF.Filter = "Image (*.png))|*.png";

            if (openF.ShowDialog() == DialogResult.OK)
            {
                Bitmap bit = (Bitmap)Image.FromFile(openF.FileName);
                tilesetDir = openF.FileName;
                tilesetDirSafe = openF.SafeFileName;
                if (bit == null)
                    return;
                else
                {
                    s_settilesize setSize = new s_settilesize(this, ref bit);
                    //if (setSize.ShowDialog() == DialogResult.OK) { }
                }
            }
        }
        public void LoadTilesetFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                goto reload;
            }
            Bitmap bit = (Bitmap)Image.FromFile(filename);
            if (bit != null)
            {
                tilesetDir = filename;
                s_tiledisp1.SetTileset(bit);
                return;
            }
            reload:
            MessageBox.Show("Couldn't find the tileset", "Error");

            OpenFileDialog openF = new OpenFileDialog();
            openF.Title = "Load tile-set";
            openF.Filter = "Image (*.png))|*.png";

            if (openF.ShowDialog() == DialogResult.OK)
            {
                bit = (Bitmap)Image.FromFile(openF.FileName);
                tilesetDir = openF.FileName;
                tilesetDirSafe = openF.SafeFileName;
                if (bit == null)
                    return;
                else
                {
                    s_tiledisp1.SetTileset(bit);
                    return;
                    //s_tiledisp1.SetTileset(bit);
                    s_settilesize setSize = new s_settilesize(this, ref bit);
                    if (setSize.ShowDialog() == DialogResult.OK) { }
                }
            }
        }

        public void BinarySave()
        {
            s_levelloader ll = new s_levelloader();
            ll.directory = directory;
            ll.te = this;
            ll.SaveMapData(s_mapdisp1.GetTileSize(), s_mapdisp1.GetWorldSize(), s_mapdisp1.tiles, tilesetDirSafe, s_mapdisp1.entities);
        }
        /*
            SaveFileDialog s = new SaveFileDialog();
            s.AddExtension = true;
            s.FileName = "Untitled.PLF";
            s.Filter = "PUPPET/Pixel Level format file (*.PLF)";
            if (s.ShowDialog() == DialogResult.OK)
            {
                List<int> ints = new List<int>();
                Random ran = new Random();
                FileStream fs = File.Create(s.FileName);
                BinaryWriter bw = new BinaryWriter(fs, Encoding.ASCII);
                for (int x = 0; x < 20; x++)
                {
                    bw.Write(ran.Next());
                }
                bw.Close();
                fs.Close();
            }
            */

        private void button1_Click(object sender, EventArgs e)
        {
            s_tiledisp1.RotateImage();
        }

        private void button2_Click(object sender, EventArgs e)
        { 
            bool vertiflip = s_tiledisp1.vertiFlip;
            s_tiledisp1.vertiFlip = vertiflip == true ? false : true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool horiflip = s_tiledisp1.horizFlip;
            s_tiledisp1.horizFlip = horiflip == true ? false : true;
        }

        private void saveAsBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            directory = null;
            BinarySave();
        }

        private void openBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            s_levelloader ll = new s_levelloader();
            ll.te = this;
            ll.LoadMapData(ref s_mapdisp1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            s_maptable maptab = new s_maptable(s_mapdisp1);
            if (maptab.ShowDialog() == DialogResult.OK){}
        }

        private void button5_Click(object sender, EventArgs e)
        {
            s_POUCH pouch = new s_POUCH();
            if (pouch.ShowDialog() == DialogResult.OK) { }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void s_tiledisp1_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbx = (ComboBox)sender;
            s_mapdisp1.entityID = (ushort)cmbx.SelectedIndex;
        }

        private void LoadTileset_Click_1(object sender, EventArgs e)
        {
            LoadTilesetFromFile();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbx = (ComboBox)sender;
            s_mapdisp1.TOOLS = (CustomControls.s_mapdisp.TOOL_MODE)cmbx.SelectedIndex;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbx = (ComboBox)sender;
            s_mapdisp1.TOOLS = (CustomControls.s_mapdisp.TOOL_MODE)cmbx.SelectedIndex;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (s_mapdisp1.map_init)
            {
                if (MessageBox.Show("Are you sure you want to delete entity number '" + entitynum + "'", "Delete entity", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    s_mapdisp1.DeleteEntityAt(entitynum);
                    Invalidate();
                    MessageBox.Show("Entity deleted.");
                }
            }

        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save?", "Save map", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                BinarySave();
            }
        }

        private void tileditor_Load(object sender, EventArgs e)
        {

        }

        private void s_mapdisp1_Click(object sender, EventArgs e)
        {

        }
    }

    public struct levelstuff
    {
        public List<List<int>> tiles;
    }
}
