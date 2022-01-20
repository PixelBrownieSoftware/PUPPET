
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Resources;

namespace LevelEditor.CustomControls
{
    public class s_tiledisp : Control
    {
        public enum ROTATION
        {
            DEG_0,
            DEG_90,
            DEG_180,
            DEG_270
        }
        public ROTATION rot;

        public ROTATION GetRotation
        {
            get { return rot; }
        }
        public bool horizFlip = false;
        public bool vertiFlip = false;

        Rectangle rect;
        Bitmap tileset;
        Bitmap selectorImage;
        Point TileSize;
        Point MousePosRound = new Point(0, 0);
        s_mapdisp editor;
        bool mousedown = false;
        Point imageSize = new Point(0,0);

        Bitmap imagePart;

        public void SetTileSize(int x1, int y1)
        {
            TileSize = new Point(x1, y1);
            selectorImage = new Bitmap(x1, y1);
            for (int x = 0; x < TileSize.X; x++)
                selectorImage.SetPixel(x, 0, Color.Red);
            for (int x = MousePosRound.X; x < +TileSize.X; x++)
                selectorImage.SetPixel(x, TileSize.Y - 1, Color.Red);
            for (int y = MousePosRound.Y; y < TileSize.Y; y++)
                selectorImage.SetPixel(0, y, Color.Red);
            for (int y = MousePosRound.Y; y < TileSize.Y; y++)
                selectorImage.SetPixel(TileSize.X - 1, y, Color.Red);
        }

        public s_tiledisp()
        {
        }

        public Point GetTileSize
        {
            get {
                return TileSize;
            }
        }

        public void SetTileset(Image img)
        {
            tileset = (Bitmap)img;
            rect = new Rectangle(0, 0, tileset.Width, tileset.Height);
            imageSize = new Point(tileset.Width, tileset.Height);
            Invalidate();
        }

        public void HFlipImage()
        {
            if (imagePart != null)
            {
                imagePart.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
        }
        public void VFlipImage()
        {
            if (imagePart != null)
            {
                imagePart.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
        }
        public void RotateImage()
        {
            if (imagePart != null)
            {
                imagePart.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
        }

        public void Initilaize(ref s_mapdisp edit)
        {
            editor = edit;
            
            MouseClick += delegate { mousedown = true; };
            MouseClick += ClickMouse;
            MouseDown += delegate { mousedown = true; };
            MouseDown += HoverMouse;
            MouseMove += HoverMouse;
            MouseMove += ClickMouse;
            MouseLeave += delegate { mousedown = false; };
            MouseUp += delegate { mousedown = false; };
        }

        public void ClickMouse(object sender, MouseEventArgs e)
        {
            if (TileSize == Point.Empty)
                return;
            imagePart = new Bitmap(TileSize.X, TileSize.Y);
            editor.CurrentTile = (ushort)((MousePosRound.X / TileSize.X) + ((MousePosRound.Y / TileSize.Y) * (imageSize.X / TileSize.X)));

            for (int x = MousePosRound.X; x < MousePosRound.X + TileSize.X; x++)
                for (int y = MousePosRound.Y; y < MousePosRound.Y + TileSize.Y; y++)
                {
                    if (x > tileset.Width - 1 ||
                        y > tileset.Height - 1 ||
                        y < 0 ||
                        x < 0)
                        continue;
                    Color pix = tileset.GetPixel(x, y);
                    imagePart.SetPixel(x - MousePosRound.X, y - MousePosRound.Y, pix);
                }

            /*
            switch (rot)
            {
                case ROTATION.DEG_0:
                    imagePart.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                    break;
                case ROTATION.DEG_90:
                    break;
                case ROTATION.DEG_180:
                    imagePart.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case ROTATION.DEG_270:
                    imagePart.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;

            }
            */
            /*
            if (horizFlip)
                imagePart.RotateFlip(RotateFlipType.RotateNoneFlipX);

            if (vertiFlip)
                imagePart.RotateFlip(RotateFlipType.RotateNoneFlipY);
            */

            editor.SetImage(imagePart);
            Invalidate();
        }

        public void HoverMouse(object sender, MouseEventArgs e)
        {
            if (TileSize == Point.Empty)
                return;
            if (mousedown)
            {
                MousePosRound = s_helper.mousePosRound(new Point(e.X, e.Y), TileSize);
                if (MousePosRound.X >= tileset.Width)
                    MousePosRound.X = tileset.Width - TileSize.X;
                if (MousePosRound.X <= 0)
                    MousePosRound.X = 0;
                if (MousePosRound.Y >= tileset.Height)
                    MousePosRound.Y = tileset.Height - TileSize.Y;
                if (MousePosRound.Y <= 0)
                    MousePosRound.Y = 0;
            }
            Invalidate();
        }

        public Bitmap ShortToBitmap(ushort num)
        {
            if (num == 0)
                return null;

            Bitmap btmp = new Bitmap(TileSize.X, TileSize.Y);
            int x1 = 0;
            int y1 = 0;

            if (num - 1 == 0)
            {
                x1 = 0;
                y1 = 0;
            }
            else
            {
                x1 = (num - 1) % (imageSize.X / TileSize.X);
                y1 = (int)Math.Ceiling((decimal)((num - 1) / (imageSize.X/ TileSize.X)));
            }


            Point imgpt = new Point((x1 * TileSize.X) , (y1 * TileSize.Y) );

            for (int x = imgpt.X; x < imgpt.X + TileSize.X; x++)
                for (int y = imgpt.Y; y < imgpt.Y + TileSize.Y; y++)
                {
                    if (x > tileset.Width - 1 ||
                        y > tileset.Height - 1 ||
                        y < 0 ||
                        x < 0)
                        continue;
                    Color pix = tileset.GetPixel(x, y);
                    btmp.SetPixel(x - imgpt.X, y - imgpt.Y, pix);
                }
            return btmp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Green);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            if (tileset != null)
                g.DrawImage(tileset, rect);
            if (selectorImage != null)
                g.DrawImage(selectorImage, new Rectangle(MousePosRound.X, MousePosRound.Y, 20, 20));
            base.OnPaint(e);
        }
    }
}
