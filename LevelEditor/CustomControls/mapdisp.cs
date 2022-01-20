using System.IO;
using System;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;

namespace LevelEditor.CustomControls
{
    public class mapdisp : GraphicsDeviceControl
    {
        public int tile_Ind_x = 0;
        public int tile_Ind_y = 0;
        bool ismousedown;
        bool ismouseonscreen;
        Vector2 mousepos;
        public ContentManager content;
        SpriteBatch sprite;
        s_map map;
        char key;

        public event EventHandler onInit;

        public mapdisp()
        {
            map = new s_map();
        }

        public s_layer layer
        {
            get { return map.lay[0]; }
        }

        protected override void Initialize()
        {
            content = new ContentManager(Services, "Content");
            sprite = new SpriteBatch(GraphicsDevice);
            //https://www.youtube.com/watch?v=jWOjc-VwjvM 21:24
            /*
            XmlSerializer xml = new XmlSerializer(map.GetType());
            Stream str = File.Open("Load/map.xml", FileMode.Open);
            map = (s_map)xml.Deserialize(str);
            */
            map = new s_map();
            map.Initialize(content);
            MouseUp += delegate { ismousedown = false; };
            MouseDown += Edit_MouseDown;
            MouseMove += Edit_MouseMove;
            KeyPress += Edit_Key;
            
            MouseEnter += delegate { ismouseonscreen = true; };
            MouseLeave += delegate { ismouseonscreen = false; };
            
            if (onInit != null)
                onInit(this, null);
        }


        void Edit_Key(object sender, KeyPressEventArgs e)
        {
            key = e.KeyChar;
            
            switch (key)
            {
                case 'a':
                    tile_Ind_x -= 1;
                    break;

                case 'd':
                    tile_Ind_x += 1;
                    break;

                case 'w':
                    tile_Ind_y -= 1;
                    break;

                case 's':
                    tile_Ind_y += 1;
                    break;
            }
            tile_Ind_x = MathHelper.Clamp(tile_Ind_x, 0, 5);
            tile_Ind_y = MathHelper.Clamp(tile_Ind_y, 0, 1);
            layer.ImgagePOS = new Vector2(tile_Ind_x, tile_Ind_y);
        }

        void Edit_MouseDown(object sender, MouseEventArgs e)
        {
            layer.SetTile(mousepos, new Vector2(tile_Ind_x, tile_Ind_y));
            ismousedown = true;
        }

        void Edit_MouseMove(object sender, MouseEventArgs e)
        {
            mousepos = new Vector2(e.X / layer.tiledimensions.X, e.Y / layer.tiledimensions.Y);
            mousepos *= layer.tiledimensions.X;
            if (ismousedown)
                Edit_MouseDown(this,null);
            Invalidate();
        }
        
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Blue);
            sprite.Begin();
            layer.ImgagePOS = new Vector2(tile_Ind_x, tile_Ind_y);
            map.Draw(sprite);
            sprite.End();
            /*
            */
        }
    }
}
