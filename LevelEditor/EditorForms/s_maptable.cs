using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LevelEditor.CustomControls;

namespace LevelEditor.EditorForms
{
    public partial class s_maptable : Form
    {
        public s_mapdisp editor;

        public s_maptable(s_mapdisp editor)
        {
            this.editor = editor;
            if (editor.tiles == null)
                DestroyHandle();
            InitializeComponent();
            label1.Text = "";
            if (editor.tiles == null)
            {
                label1.Text = "A map is not loaded";
                label2.Text = "A map is not loaded";
                return;
            }
            for (int i = 0; i < editor.tiles.Length; i++)
            {
                if (i % editor.GetWorldSize().X == 0 && i != 0)
                {
                    label1.Text += "\n";
                }
                label1.Text += editor.tiles[i] + ", ";
            }

            label2.Text = "";
            for (int i = 0; i < editor.entities.Count; i++)
            {
                label2.Text += 
                    i + ". " + "Entity: " + editor.entities[i].id 
                    + "Position: (" + editor.entities[i].position.X  + ", " + editor.entities[i].position.Y + ")" 
                    + " Label No. " + editor.entities[i].labelToCall + " ." ;
                label2.Text += "\n";
            }
        }
    }
}
