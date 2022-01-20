using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelEditor.CustomControls;
using LevelEditor.EditorForms;

namespace LevelEditor
{
    public class ev_details
    {
        public byte bits;
        public byte EV_TYPE;
        public int int0;
        public int int1;
        public short short0;
        public short short1;
        public string string0;
        public string string1;
        public float float0;
        public float float1;
    }
    public class s_levelloader
    {
        public string directory;
        public tileditor te;

        public void LoadMapData(ref s_mapdisp disp)
        {
            OpenFileDialog s = new OpenFileDialog();
            s.Filter = "PUPPET/Pixel Level format file (*.PLF))|*.PLF";
            if (s.ShowDialog() == DialogResult.OK)
            {
                directory = s.FileName;
                te.directory = directory;
                try
                {
                    using (BinaryReader br = new BinaryReader(File.Open(s.FileName, FileMode.Open)))
                    {
                        string version = br.ReadString();
                        Console.Out.Write(version);
                        switch (version)
                        {
                            case "MAP_MADE_WITH_PUPPET_V0.01":
                                var b = 0;
                                var s1 = br.ReadByte();
                                Console.Out.WriteLine("Integer: " + s1 + " At memory address: " + br.BaseStream.Position);
                                var s2 = br.ReadByte();
                                Console.Out.WriteLine("Integer: " + s2 + " At memory address: " + br.BaseStream.Position);

                                disp.SetTileSize(new Point(s1, s2));

                                var ws1 = br.ReadUInt16();
                                var ws2 = br.ReadUInt16();
                                disp.SetWorldSize(new Point(ws1, ws2));

                                //Tiles
                                ushort[] tiles = new ushort[ws1 * ws2];
                                for (int i = 0; i < tiles.Length; i++)
                                {
                                    tiles[i] = br.ReadUInt16();
                                    Console.Out.WriteLine("Integer: " + b + " At memory address: " + br.BaseStream.Position);
                                    //bw.BaseStream.Position += sizeof(ushort);
                                }
                                string filename = br.ReadString();  //This is a safe name
                                string currdir = directory.Remove(directory.Length - s.SafeFileName.Length, s.SafeFileName.Length);
                                string fileDir = currdir + filename;
                                te.LoadTilesetFromFile(fileDir);
                                
                                disp.SetTiles(tiles);
                                if ((int)(br.BaseStream.Position + sizeof(ushort)) > (int)br.BaseStream.Length)
                                {
                                    br.Close();
                                    return;
                                }
                                ushort leng = br.ReadUInt16();

                                disp.entities = new List<o_entity>();
                                for (int i = 0; i < leng; i++)
                                {
                                    o_entity ent = new o_entity();
                                    ushort id = br.ReadUInt16();
                                    int x = br.ReadInt32();
                                    int y = br.ReadInt32();
                                    ushort label = br.ReadUInt16();

                                    ent.id = id;
                                    ent.position = new Point(x, y);
                                    ent.labelToCall = label;

                                    if (br.ReadBoolean())
                                    {
                                        ent.stringlist = new List<Tuple<string, string>>();
                                        int lengthOfFlags = br.ReadInt16();
                                        for (int i2 = 0; i2 < lengthOfFlags; i2++)
                                        {
                                            string st = br.ReadString();
                                            short l = br.ReadInt16();
                                            for (int i3 = 0; i3 < l; i3++)
                                            {
                                                Tuple<string, string> str = new Tuple<string, string>(st, br.ReadString());
                                                Console.Out.WriteLine("Flag: " + str.Item1 + ", " + str.Item2);
                                                ent.stringlist.Add(str);
                                            }
                                        }
                                    }
                                    disp.entities.Add(ent);
                                }
                                break;
                        }
                        br.Close();
                        te.Text = tileditor.appNameText + " - " + s.SafeFileName;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Couldn't Load. Error: " + e.Message, "Error");
                }
            }
        }
        public void SaveMapData(Point tilSiz, Point wdSiz, ushort[] tiles, string tilesetname, List<o_entity> entities)
        {
            if (directory == null || directory == "")
            {
                using (SaveFileDialog s = new SaveFileDialog())
                {
                    s.FileName = "Untitled.PLF";
                    s.Filter = "PUPPET/Pixel Level format file (*.PLF)|*.PLF";

                    DialogResult dr = s.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        // s.FileName += "Untitled.PLF";
                        directory = s.FileName;
                    }
                    if (dr == DialogResult.Cancel)
                        return;
                    if (s != null)
                    {
                        if (dr == DialogResult.None)
                        {
                            return;
                        }
                    }
                    else
                        return;
                }
            }
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.Create(directory + ".tmp")))
                {
                    //Write the version of the editor
                    bw.Write(tileditor.versionName);
                    //Tilesize
                    //bw.Write("TileSize");
                    byte s1 = (byte)tilSiz.X, s2 = (byte)tilSiz.Y;
                    ushort m1 = (ushort)wdSiz.X, m2 = (ushort)wdSiz.Y;

                    bw.Write(s1);
                    Console.Out.WriteLine("Memory address: " + bw.BaseStream.Position);
                    bw.Write(s2);
                    Console.Out.WriteLine("Tile size: " + s1 + " x " + s2 + " At memory address: " + bw.BaseStream.Position);

                    bw.Write(m1);
                    Console.Out.WriteLine("Memory address: " + bw.BaseStream.Position);
                    bw.Write(m2);
                    Console.Out.WriteLine("Map size: " + m1 + " x " + m2 + " At memory address: " + bw.BaseStream.Position);

                    for (int i = 0; i < tiles.Length; i++)
                    {
                        ushort b = tiles[i];
                        bw.Write(b);
                        //Console.Out.WriteLine("Integer: " + b + " At memory address: " + bw.BaseStream.Position);
                        //bw.BaseStream.Position += sizeof(ushort);
                    }
                    if(tilesetname != null)
                        bw.Write(tilesetname);
                    else
                        bw.Write("");
                    ushort enititycount = (ushort)entities.Count;
                    bw.Write(enititycount);
                    Console.Out.WriteLine("Entity count: " + enititycount);
                    int count = 0;
                    foreach (o_entity ent in entities)
                    {
                        if (ent == null)
                            continue;
                        count++;
                        Console.Out.WriteLine("Entity " + count + " at " + bw.BaseStream.Position);
                        bw.Write(ent.id);
                        Console.Out.WriteLine("Entity " + count + " id: " + ent.id + " at " + bw.BaseStream.Position);
                        bw.Write(ent.position.X);
                        bw.Write(ent.position.Y);
                        Console.Out.WriteLine("Entity " + count + " id: Position (" + ent.position.X + ", " + ent.position.Y + ") at " + bw.BaseStream.Position);
                        bw.Write(ent.labelToCall);
                        Console.Out.WriteLine("Entity " + count + " id: label" + ent.labelToCall + " at " + bw.BaseStream.Position);

                        //Flag if the string list exists
                        Console.Out.WriteLine(ent.stringlist != null);
                        if (ent.stringlist == null)
                            bw.Write(false);
                        else
                        {
                            bw.Write(ent.stringlist.Count > 0);
                        }

                        //List Out all the entity's flags
                        if (ent.stringlist != null)
                        {
                            if (ent.stringlist.Count > 0)
                            {
                                List<Tuple<string, string>> stri = new List<Tuple<string, string>>(ent.stringlist);
                                Tuple<string, string> current = stri[0];

                                List<short> lengths = new List<short>();
                                List<List<Tuple<string, string>>> groupOfLists = new List<List<Tuple<string, string>>>();

                                //Find all common flag names and store their values incrementally
                                while (stri.Count > 0)
                                {
                                    List<Tuple<string, string>> SameTuples = stri.FindAll(x => x.Item1 == current.Item1);
                                    groupOfLists.Add(SameTuples);
                                    lengths.Add((short)SameTuples.Count);
                                    Console.Out.WriteLine(current.Item1 + " with length " + (short)SameTuples.Count);
                                    stri.RemoveAll(x => x.Item1 == current.Item1);

                                    if (stri.Count > 0)
                                        current = stri[0];
                                }

                                bw.Write((short)groupOfLists.Count);
                                int leng = 0;
                                foreach (List<Tuple<string, string>> st in groupOfLists)
                                {
                                    //Name
                                    bw.Write(st[0].Item1);
                                    Console.Out.WriteLine(st[0].Item1);
                                    //Lenght
                                    bw.Write(lengths[leng]);
                                    Console.Out.WriteLine(lengths[leng]);
                                    for (int i = 0; i < lengths[leng]; i++)
                                    {
                                        Console.Out.WriteLine(st[i].Item2);
                                        bw.Write(st[i].Item2);
                                    }
                                    leng++;
                                }
                            }
                        }
                    }
                    bw.Close();
                    File.Copy(directory + ".tmp", directory, true);

                    if (File.Exists(directory + ".tmp"))
                        File.Delete(directory + ".tmp");

                    if (te != null)
                    {
                        te.directory = directory;
                        te.Text = tileditor.appNameText + " - " + directory;
                    }
                    MessageBox.Show("Map successfuly saved at memory address");
                }
            }
            catch (Exception e)
            {
                if (File.Exists(directory + ".tmp"))
                    File.Delete(directory + ".tmp");
                MessageBox.Show("Couldn't save. Error: " + e.Message, "Error");
            }
        }

    }
}
