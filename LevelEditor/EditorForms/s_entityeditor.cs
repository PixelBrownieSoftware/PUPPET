using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LevelEditor;

namespace LevelEditor.EditorForms
{
    public partial class s_entityeditor : Form
    {
        public List<o_entity> entities;
        o_entity ent;
        public int selectedFlag = 0;
        Point pos;
        ushort oldID = 0; //If we want to go back in case of a cancel in change of entity ID
        bool initialized = false;

        Tuple<string, string> currentFlag;

        public s_entityeditor(ref o_entity entity)
        {
            InitializeComponent();
            ent = entity;
            entityID.Value = ent.id;
            oldID = (ushort)entityID.Value;
            initialized = true;

            numericUpDown2.Value = ent.position.X / CustomControls.s_mapdisp.tilesize.X;
            numericUpDown3.Value = ent.position.Y / CustomControls.s_mapdisp.tilesize.Y;
            if (entity.stringlist != null)
            {
                foreach (Tuple<string, string> s in entity.stringlist)
                {
                    listOfFlags.Items.Add(s);
                }
            }
        }
        
        public static void SetEntityFlags(ref o_entity enti)
        {
            switch (enti.id)
            {
                case 0:
                    enti.AddString("direction", "up");
                    break;
                case 2:
                    enti.AddString("locked", "0");
                    enti.AddString("blocks", "0");
                    break;

                /*
                case 1:
                    enti.AddString("locked", "0");
                    enti.AddString("name", "DoorName");
                    break;

                case 10:
                    enti.AddString("name", "DoorName");
                    enti.AddString("portalFlag", "OtherPortal");
                    break;

                case 2:
                    enti.AddString("doorFlag", "DoorName");
                    break;

                case 6:
                    enti.AddString("itemType", "0");
                    break;

                case 7:
                    enti.AddString("name", "DoorName");
                    break;

                case 9:
                case 8:
                    enti.AddString("dir", "1");
                    break;
            */
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            ent.id = (ushort)entityID.Value;
            Point entitypos = new Point((ushort)numericUpDown2.Value * CustomControls.s_mapdisp.tilesize.X, (ushort)numericUpDown3.Value * CustomControls.s_mapdisp.tilesize.Y);
            o_entity entOverride = entities.Find(x => x.position == entitypos);

            if (entOverride == null || entOverride == ent)
            {
                ent.position = entitypos;
                if (listOfFlags != null)
                {
                    ent.stringlist = new List<Tuple<string, string>>();
                    foreach (Tuple<string, string> s in listOfFlags.Items)
                    {
                        ent.stringlist.Add(s);
                    }
                }
                Close();
            }
            else {
                MessageBox.Show("An entity already exists at position " + entitypos);
            }
        }

        private void flagAdd_Click(object sender, EventArgs e)
        {
            Tuple<string, string> fl = new Tuple<string, string>(name.Text, flagval.Text);
            listOfFlags.BeginUpdate();
            listOfFlags.Items.Add(fl);
            listOfFlags.EndUpdate();
        }

        private void listOfFlags_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox cmbx = (ListBox)sender;
            selectedFlag = cmbx.SelectedIndex;
            if (selectedFlag != -1)
            {
                currentFlag = (Tuple<string, string>)cmbx.Items[selectedFlag];
                name.Text = currentFlag.Item1;
                flagval.Text = currentFlag.Item2;
            }
            Invalidate();
        }

        private void name_TextChanged(object sender, EventArgs e)
        {
            currentFlag = new Tuple<string, string>(name.Text, currentFlag.Item2);
            listOfFlags.Items[selectedFlag] = currentFlag;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            selectedFlag = (int)numericUpDown4.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void flagval_TextChanged(object sender, EventArgs e)
        {
            currentFlag = new Tuple<string, string>(currentFlag.Item1, flagval.Text);
            listOfFlags.Items[selectedFlag] = currentFlag;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                DialogResult dr = MessageBox.Show("Change entity ID", "Clear all flags?", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                { 
                    oldID = (ushort)entityID.Value;
                    ent.id = oldID;

                    if (ent.stringlist != null)
                        ent.stringlist.Clear();
                    else
                        ent.stringlist = new List<Tuple<string, string>>();

                    listOfFlags.BeginUpdate();
                    listOfFlags.Items.Clear();
                    listOfFlags.EndUpdate();

                    SetEntityFlags(ref ent);

                    listOfFlags.BeginUpdate();
                    foreach (Tuple<string, string> st in ent.stringlist)
                    {
                        listOfFlags.Items.Add(st);
                    }
                    listOfFlags.EndUpdate();
                    Invalidate();
                }
                else if (dr == DialogResult.No)
                {
                    entityID.Value = oldID;
                }
            }
        }
    }
}
