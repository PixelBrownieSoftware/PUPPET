using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor.EditorForms
{

    public partial class s_POUCH : Form
    {
        List<ev_details> PEvents = new List<ev_details>();

        public enum EVENT_TYPE
        {
            NONE,
            MOVE,
            DISPLAY_TEXTBOX,
            DISPLAY_IMAGE,
            WRITE_TEXT,
            SET_FLAG,
            CHECK_FLAG,
            BREAK,
            LABEL
        }

        public s_POUCH()
        {
            InitializeComponent();
            Click += OnSelectEvent;
            Click += OnKeyDown;
        }

        void OnKeyDown(object sender, EventArgs e)
        {
        }

        public void ShowDisp(ev_details det)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PEvents.Add(new ev_details());
            listBox1.BeginUpdate();
            listBox1.Items.Add(PEvents[PEvents.Count - 1]);
            listBox1.EndUpdate();
        }

        private void OnSelectEvent(object sender, EventArgs e)
        {
            if (PEvents.Count == 0)
                return;
           // s_eventdisplay1.eventDetail = PEvents[PEvents.Count - 1];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ev_details det = (ev_details)listBox1.SelectedItem;
            eventdisplay.IncrementDisp(ref det);
            Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ev_details det = (ev_details)listBox1.SelectedItem;
            eventdisplay.DecrementDisp(ref det);
            Invalidate();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        void BinarySave()
        {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "Untitled";
            s.Filter = "PALS/Pixel Action Language Script (*.PALS))|*.PALS";
            if (s.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (BinaryWriter bw = new BinaryWriter(File.Create(s.FileName)))
                    {
                        ushort leng = (ushort)PEvents.Count;
                        bw.Write(leng);
                        for (ushort i = 0; i < (ushort)PEvents.Count; i++)
                        {
                            ev_details ev = PEvents[i];
                            byte ev_type = ev.EV_TYPE;
                            short short0 = ev.short0;
                            short short1 = ev.short1;
                            int int0 = ev.int0;
                            int int1 = ev.int1;
                            string string0 = ev.string0;
                            string string1 = ev.string1;
                            float float0 = ev.float0;
                            float float1 = ev.float1;

                            bw.Write(ev_type);
                            bw.Write(short0);
                            bw.Write(short1);
                            bw.Write(int0);
                            bw.Write(int1);
                            bw.Write(string0);
                            bw.Write(string1);
                            bw.Write(float0);
                            bw.Write(float1);
                        }
                        bw.Close();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Couldn't save. Error: " + e.Message, "Error");
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BinarySave();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        public void ChangeEvDet(ev_details det)
        {
            listBox1.SelectedItem = det;
            Invalidate();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                ev_details det = (ev_details)listBox1.SelectedItem;
                eventdisplay.syst = this;
                eventdisplay.SetDisp(ref det);
            }
        }

        private void save_Click(object sender, EventArgs e)
        {

        }

        private void XPOS_ValueChanged(object sender, EventArgs e)
        {
            ev_details det = (ev_details)listBox1.SelectedItem;
            ChangeEvDet(det);
            
            ev_details det2 = (ev_details)listBox1.SelectedItem;
            Invalidate();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            ev_details det = (ev_details)listBox1.SelectedItem;
            det.int0 = (int)numericUpDown1.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            ev_details det = (ev_details)listBox1.SelectedItem;
            det.float0 = (float)numericUpDown1.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ev_details det = (ev_details)listBox1.SelectedItem;
            det.float1 = (float)numericUpDown1.Value;
        }
    }
}
