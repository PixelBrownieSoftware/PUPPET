
using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;

namespace LevelEditor.CustomControls
{
    public class tiledisplay : GraphicsDeviceControl
    {
        public mapdisp editor;
        public img image;
        public SpriteBatch sb;

        public tiledisplay(ref mapdisp map)
        {
            editor = map;
            editor.onInit += LoadContent;
        }

        void LoadContent(object sender, EventArgs e)
        {
            image = editor.layer.image;
        }

        protected override void Initialize()
        {
            sb = new SpriteBatch(GraphicsDevice);
        }

        protected override void Draw()
        {
            sb.Begin();
            image.Draw(sb);
            sb.End();
        }
    }
}
