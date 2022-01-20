using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    public class s_map
    {
        [XmlElement("Layer")]
        public List<s_layer> lay;
        public Vector2 tileDim;

        public s_map()
        {
            lay = new List<s_layer>();
            lay.Add(new s_layer());
            tileDim = new Vector2(20, 20);
        }

        public void Initialize(ContentManager cont)
        {
            foreach (s_layer l in lay)
                l.Intialize(cont, tileDim);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (s_layer l in lay)
                l.Draw(sb);
        }
    }

    public class img
    {
        private Texture2D texture;
        public Rectangle srcRect;
        private ContentManager cm;
        public float alpha;
        public string path;
        public Vector2 positon;

        [XmlIgnore]
        public Texture2D Texture
        {
            get { return texture; }
        }

        public img()
        {
            alpha = 1f;
            srcRect = Rectangle.Empty;
        }

        public void Initalizie(ContentManager cm)
        {
            this.cm = new ContentManager(cm.ServiceProvider, "Content");
            texture = cm.Load<Texture2D>("pinkroomtileset2");
            /*
             * pinkroomtileset2
            if (!String.IsNullOrEmpty(path))
                texture = cm.Load<Texture2D>(path);
            */

            if (srcRect == Rectangle.Empty)
                srcRect = texture.Bounds;
                    //texture.Bounds;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positon, srcRect, Color.White);
        }
    }


    public class s_layer
    {
        public class tilemap
        {
            [XmlElement("Tiles")]
            public List<string> row;
        }
        public Vector2 tiledimensions;

        public Vector2 ImgagePOS;

        public List<List<Vector2>> tiles;
        public tilemap tilelayout;

        public img image;

        public s_layer()
        {
            tiledimensions = new Vector2();
            tiles = new List<List<Vector2>>();
        }

        public void Intialize(ContentManager cont, Vector2 tiledim)
        {
            /*
        foreach (string row in tilelayout.row)
        {

            List<Vector2> tilemapTemp = new List<Vector2>();
            string[] spl = row.Split(']');
            foreach (string s in spl)
            {
                int val1= 0, val2 = 0;
                if (s!= string.Empty && !s.Contains('x'))
                {
                    string str = s.Replace('[', ' ');
                    val1 = int.Parse(str.Substring(0, s.IndexOf(':')));
                    val2 = int.Parse(str.Substring(str.IndexOf(':') + 1));
                }
                else
                    val1 = val2 = -1;

                tilemapTemp.Add(new Vector2(val1, val2));
            }
        }
            */
            image = new img();
            image.Initalizie(cont);
            for (int x = 0; x < 45; x++)
            {
                List<Vector2> tilemapTemp = new List<Vector2>();
                for (int y = 0; y < 45; y++)
                {
                    tilemapTemp.Add(new Vector2(0, 0));
                }
                tiles.Add(tilemapTemp);
            }

            tiledimensions = tiledim;
        }

        public void SetTile(Vector2 pos, Vector2 ind)
        {
            int x = (int)((pos.X - tiledimensions.X) / tiledimensions.X);
            int y = (int)(pos.Y / tiledimensions.Y);
            tiles[x][y] = ind;
        }

        public void Draw(SpriteBatch sb)
        {
            image.positon = new Vector2((0 * tiledimensions.X), 0 * tiledimensions.Y);
            image.srcRect = new Rectangle(
               (int)(ImgagePOS.X * tiledimensions.X)
               ,
               (int)(ImgagePOS.Y * tiledimensions.Y)
               ,
                (int)tiledimensions.X,
                (int)tiledimensions.Y);
            image.Draw(sb);

            for (int x = 0; x < tiles.Count; x++)
            {
                for (int y = 0; y < tiles[x].Count; y++)
                {
                    if (tiles[x][y] != -Vector2.One)
                    {
                        image.positon = new Vector2((x * tiledimensions.X) + 20, y * tiledimensions.Y);
                        image.srcRect = new Rectangle(
                           (int)(tiles[x][y].X * tiledimensions.X)
                           ,
                           (int)(tiles[x][y].Y * tiledimensions.Y)
                           ,
                            (int)tiledimensions.X,
                            (int)tiledimensions.Y);
                        image.Draw(sb);
                    }
                }
            }
        }

        public void Save()
        {
            tilelayout.row = new List<string>();

            //for(int i = 0; i < tiles.Count;)_
        }
    }
}
