
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Resources;
using LevelEditor.EditorForms;

public class o_entity
{
    public void AddString(string n, string v)
    {
        if (stringlist == null)
            stringlist = new List<Tuple<string, string>>();
        stringlist.Add(new Tuple<string, string>(n,v));
    }
    public string name;
    public ushort id;
    public Point position = new Point(0, 0);
    public ushort labelToCall;
    public List<Tuple<string, string>> stringlist;
    public List<Tuple<string, float>> floatlist;
    public List<Tuple<string, int>> intlist;
    public List<Tuple<string, short>> shortlist;
}

namespace LevelEditor.CustomControls
{
    public static class s_helper
    {
        public static Point MouseToWorldPosition(Point mouse, Point worldpos)
        {
            return new Point(mouse.X - worldpos.X, mouse.Y - worldpos.Y);
        }

        public static Point mousePosRound(Point mouse, Point restraint)
        {
            int x = (mouse.X / restraint.X) * restraint.X;
            int y = (mouse.Y / restraint.Y) * restraint.Y;
            return new Point(x, y);
        }

    }

    public class s_mapdisp : Control
    {
        public ushort entityID = 0;
        public Bitmap Canvas;
        public Bitmap img;
        public Rectangle rect;
        Color alphaColour = Color.Green;
        bool mousedown = false;
        public static Point tilesize = new Point(20, 20);
        public Point WorldSize = new Point(0, 0);
        public Point WorldPos = new Point(0, 0);
        public ushort[] tiles;
        public List<o_entity> entities = new List<o_entity>();
        public s_tiledisp tiledisply;

        public o_entity selectedEntity;

        public enum SELECTION_MODE
        {
            TILES,
            ENTITIES
        };
        public SELECTION_MODE SELECT_MODE;

        public enum TOOL_MODE
        {
            BRUSH,
            ERASE,
            SELECT
        };
        public TOOL_MODE TOOLS;

        public ushort CurrentTile;

        public bool map_init = false;

        public Point GetWorldSize()
        {
            return WorldSize;
        }
        public Point GetTileSize()
        {
            return tilesize;
        }

        public s_mapdisp()
        {
            MouseMove += OnMouseClick;
            MouseClick += OnMouseClick;
            MouseDown += delegate {
                Invalidate();
                mousedown = true;
            };
            MouseLeave += delegate {
                Invalidate();
                mousedown = false;
            };
            MouseUp += delegate {
                Invalidate();
                mousedown = false;
            };
            TOOLS = TOOL_MODE.BRUSH;
        }

        public void SetPosX(int x)
        {
            WorldPos.X = x;
            Invalidate();
        }
        public void SetPosY(int y)
        {
            WorldPos.Y = y;
            Invalidate();
        }
        public void AddPosX(int x)
        {
            WorldPos.X += x;
            Invalidate();
        }
        public void AddPosY(int y)
        {
            WorldPos.Y += y;
            Invalidate();
        }

        public void SetTiles(ushort[] tiles)
        {
            this.tiles = tiles;
            tiledisply.SetTileSize(tilesize.X, tilesize.Y);
            SetTileAndMapSize();
        }

        public void SetTileSize(Point tile)
        {
            tilesize = tile;
        }
        public void SetWorldSize(Point world)
        {
            WorldSize = world;
        }

        public void SetTileAndMapSize(Point map)
        {
            WorldSize = map;
            tiles = new ushort[WorldSize.X * WorldSize.Y];
            if (tilesize.IsEmpty)
            {
                MessageBox.Show("A tileset needs to be loaded in.", "Error");
                return;
            }
            Canvas = new Bitmap(tilesize.X * WorldSize.X, tilesize.Y * WorldSize.Y);
            for (int x = 0; x < Canvas.Width; x++)
                for (int y = 0; y < Canvas.Height; y++)
                {
                    Canvas.SetPixel(x, y, alphaColour);
                }
            entities.Clear();
            ResourceManager rm = new ResourceManager(typeof(Bitmap));
            rect = new Rectangle(0, 0, Canvas.Width, Canvas.Height);
            img = new Bitmap(20,20);
            map_init = true;
        }
        public void SetTileAndMapSize()
        {
            Console.Out.WriteLine("Tile size: " + tilesize.X + " World size: " + WorldSize.X);
            Canvas = new Bitmap(tilesize.X * WorldSize.X, tilesize.Y * WorldSize.Y);
            for (int x = 0; x < Canvas.Width; x++)
                for (int y = 0; y < Canvas.Height; y++)
                {
                    Canvas.SetPixel(x, y, alphaColour);
                }
            ResourceManager rm = new ResourceManager(typeof(Bitmap));
            rect = new Rectangle(0, 0, Canvas.Width, Canvas.Height);
            
            map_init = true;
            int x1 = 0, y1 = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                Bitmap TileGraphic = tiledisply.ShortToBitmap(tiles[i]);
                //If there is no tile here, skip it and go to the next one
                if (TileGraphic == null)
                {
                    x1++;
                    if (x1 == WorldSize.X)
                    {
                        y1++;
                        x1 = 0;
                    }
                    continue;
                }
                DrawTile(new Point(x1 * tilesize.X , y1 * tilesize.Y), TileGraphic);
                Invalidate();
                x1++;
                if (x1 == WorldSize.X)
                {
                    y1++;
                    x1 = 0;
                }
            }
            entities.Clear();
        }

        public void LoadMap(Bitmap image, Point ts)
        {
            if (image == null)
                return;
            Canvas = new Bitmap(image.Width, image.Height);
            rect = new Rectangle(0, 0, Canvas.Width, Canvas.Height);
            for (int x = 0; x < Canvas.Width; x++)
                for (int y = 0; y < Canvas.Height; y++)
                {
                    Canvas.SetPixel(x, y, Color.White);
                }
            tilesize = ts;
            Canvas = image;
            map_init = true;
            Invalidate();
        }

        public void SetImage(Bitmap image)
        {
            img = image;
        }

        public void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (!map_init)
                return;
            if (mousedown)
            {
                Point mousepos = s_helper.MouseToWorldPosition(new Point(e.X, e.Y), WorldPos);
                mousepos = s_helper.mousePosRound(new Point(mousepos.X, mousepos.Y), tilesize);
                int x = (mousepos.X / tilesize.X), y = (mousepos.Y / tilesize.Y);

                switch (TOOLS)
                {
                    case TOOL_MODE.BRUSH:

                        switch (SELECT_MODE)
                        {
                            case SELECTION_MODE.ENTITIES:
                                if (entities.Find(ds => ds.position == new Point(x * tilesize.X, y * tilesize.Y)) == null)
                                    DrawEntity(new Point(mousepos.X, mousepos.Y), entityID);
                                break;

                            case SELECTION_MODE.TILES:
                                if (x + (y * (WorldSize.X)) > tiles.Length - 1)
                                    break;
                                if (x + (y * (WorldSize.X)) < 0)
                                    break;
                                tiles[x + (y * (WorldSize.X))] = (ushort)(CurrentTile + 1);
                                DrawTile(new Point(mousepos.X, mousepos.Y), img);
                                break;
                        }
                        break;

                    case TOOL_MODE.ERASE:

                        switch (SELECT_MODE)
                        {
                            case SELECTION_MODE.ENTITIES:
                                EraseEntity(new Point(mousepos.X, mousepos.Y));
                                break;

                            case SELECTION_MODE.TILES:
                                if (x + (y * (WorldSize.X)) > tiles.Length - 1)
                                    break;
                                tiles[x + (y * (WorldSize.X))] = 0;
                                EraseTile(new Point(mousepos.X, mousepos.Y));
                                break;
                        }
                        break;

                    case TOOL_MODE.SELECT:

                        switch (SELECT_MODE)
                        {
                            case SELECTION_MODE.ENTITIES:
                                SelectEntity(mousepos);
                                break;

                            case SELECTION_MODE.TILES:
                                TOOLS = TOOL_MODE.BRUSH;
                                break;
                        }
                        break;

                }
                Invalidate();
            }
        }

        private void EraseTile(Point pos)
        {
            for (int x = 0; x < tilesize.X; x++)
                for (int y = 0; y < tilesize.Y; y++)
                {
                    Color pix = alphaColour;
                    if (pos.X + x >= Canvas.Width ||
                        pos.X + x <= 0 ||
                        pos.Y + y <= 0 ||
                        pos.Y + y >= Canvas.Height)
                        continue;
                    Canvas.SetPixel(pos.X + x, pos.Y + y, pix);
                }
        }
        public void DrawTile(Point pos, Bitmap im)
        {
            if (im == null)
            {
                MessageBox.Show("You need a tile selected.");
                return;
            }
            for (int x = 0; x < im.Width; x++)
                for (int y = 0; y < im.Height; y++)
                {
                    Color pix = im.GetPixel(x, y);
                    if (pos.X + x >= Canvas.Width ||
                        pos.X + x <= 0 ||
                        pos.Y + y <= 0 ||
                        pos.Y + y >= Canvas.Height)
                        continue;
                    Canvas.SetPixel(pos.X + x, pos.Y + y, pix);
                }
        }

        public void DeleteEntityAt(int i)
        {
            entities.RemoveAt(i);
        }
        public void EraseEntity(Point pos)
        {
            entities.Remove(entities.Find(ent => ent.position == pos));
            Invalidate();
        }
        public void SelectEntity(Point pos)
        {
            selectedEntity = entities.Find(ent => ent.position == pos);
            if (selectedEntity == null)
                return;
            EditorForms.s_entityeditor maptab = new EditorForms.s_entityeditor(ref selectedEntity);
            maptab.entities = entities;
            if (maptab.ShowDialog() == DialogResult.OK) {
            }
            Invalidate();
        }
        public void DrawEntity(Point pos, ushort ind)
        {
            o_entity e = new o_entity();
            e.id = ind;
            s_entityeditor.SetEntityFlags(ref e);

            e.position = pos;
            entities.Add(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(alphaColour);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            if (map_init)
            {
                /*
                int x = 0, y = 0;
                for (int i = 0; i < tiles.Length; i++)
                {
                    x++;
                    if (i % WorldSize.X == 0)
                    {
                        y++;
                        x = 0;
                    }
                    Bitmap TileGraphic = tiledisply.ShortToBitmap(tiles[i]);
                    
                    Canvas.SetPixel.DrawImage(, new Rectangle(
                        WorldPos.X + (x * tilesize.X), 
                        WorldPos.Y + (y * tilesize.Y), 
                        tilesize.X, 
                        tilesize.Y));
                }
                */
                g.DrawImage(Canvas, new Rectangle(WorldPos.X, WorldPos.Y, rect.Width, rect.Height));
                foreach (o_entity entity in entities)
                {
                    Point pos = entity.position;
                    Color pix = Color.Black;
                    uint ind = entity.id;
                    switch (ind)
                    {
                        case 0:
                            pix = Color.HotPink;
                            break;

                        case 1:
                            pix = Color.AliceBlue;
                            break;

                        case 2:
                            pix = Color.GreenYellow;
                            break;

                        case 3:
                            pix = Color.OrangeRed;
                            break;

                        case 4:
                            pix = Color.Turquoise;
                            break;

                        case 5:
                            pix = Color.Orange;
                            break;

                        case 6:
                            pix = Color.DarkSeaGreen;
                            break;

                        case 7:
                            pix = Color.Crimson;
                            break;

                        case 8:
                            pix = Color.Violet;
                            break;

                        case 9:
                            pix = Color.Orange;
                            break;

                        case 10:
                            pix = Color.Magenta;
                            break;
                    }
                    Bitmap sprite = new Bitmap(tilesize.X, tilesize.Y);

                    for (int x = 0; x < tilesize.X - 1; x++)
                        sprite.SetPixel(x, 0, pix);

                    for (int x = 0; x < tilesize.X - 1; x++)
                        sprite.SetPixel(x, tilesize.Y - 1, pix);

                    for (int y = 0; y < tilesize.Y - 1; y++)
                        sprite.SetPixel(0, y, pix);

                    for (int y = 0; y < tilesize.Y - 1; y++)
                        sprite.SetPixel(tilesize.X - 1, y, pix);

                    g.DrawImage(sprite, new Point(WorldPos.X + entity.position.X, WorldPos.Y + entity.position.Y));
                }
            }
            base.OnPaint(e);
        }
    }
}
