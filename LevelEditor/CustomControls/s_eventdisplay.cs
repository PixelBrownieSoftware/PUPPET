using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using LevelEditor.EditorForms;

namespace LevelEditor.CustomControls
{
    public class s_eventdisplay : Control
    {
        Font font;
        SolidBrush b = new SolidBrush(Color.Black);
        public s_POUCH syst;

        public ev_details eventDetail = new ev_details();
        public s_eventdisplay()
        {
            font = new Font("Times New Roman", 12.0f);
        }
        public void SetDisp(ref ev_details det)
        {
            eventDetail = det;
            Invalidate();
        }

        public void IncrementDisp(ref ev_details det)
        {
            if (det.EV_TYPE > byte.MaxValue)
                return;
            det.EV_TYPE++;
            syst.ChangeEvDet(det);
            Invalidate();
        }
        public void DecrementDisp(ref ev_details det)
        {
            if (det.EV_TYPE == 0)
                return;
            det.EV_TYPE--;
            syst.ChangeEvDet(det);
            Invalidate();
        }

        void DrawEvent(Graphics g)
        {
            string not = "\nPress Up or down to change " + eventDetail.EV_TYPE;
            s_POUCH.EVENT_TYPE ev = (s_POUCH.EVENT_TYPE)eventDetail.EV_TYPE;
            switch (ev)
            {
                case s_POUCH.EVENT_TYPE.NONE:
                    g.DrawString("NONE" + not, font, b, 10, 60);
                    break;

                case s_POUCH.EVENT_TYPE.MOVE:
                    g.DrawString("" + "Position: " + eventDetail.float0 + ", " + eventDetail.float1 + not, font, b, 10, 60);
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            DrawEvent(g);
            g.DrawString("Select a handler\n", font, b, 10, 10);
            base.OnPaint(e);
        }
    }
}
