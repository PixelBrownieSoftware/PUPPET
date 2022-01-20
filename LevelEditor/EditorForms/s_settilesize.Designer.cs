namespace LevelEditor.EditorForms
{
    partial class s_settilesize
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.TileYupdown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TileXupdown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileYupdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileXupdown)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TileYupdown);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.TileXupdown);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(61, 20);
            this.panel1.TabIndex = 7;
            // 
            // TileYupdown
            // 
            this.TileYupdown.Location = new System.Drawing.Point(23, 45);
            this.TileYupdown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.TileYupdown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TileYupdown.Name = "TileYupdown";
            this.TileYupdown.Size = new System.Drawing.Size(37, 20);
            this.TileYupdown.TabIndex = 4;
            this.TileYupdown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tile size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Y";
            // 
            // TileXupdown
            // 
            this.TileXupdown.Location = new System.Drawing.Point(23, 19);
            this.TileXupdown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.TileXupdown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TileXupdown.Name = "TileXupdown";
            this.TileXupdown.Size = new System.Drawing.Size(37, 20);
            this.TileXupdown.TabIndex = 2;
            this.TileXupdown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "X";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 32);
            this.button1.TabIndex = 8;
            this.button1.Text = "Done";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // s_settilesize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 152);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Name = "s_settilesize";
            this.Text = "s_settilesize";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileYupdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileXupdown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown TileYupdown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown TileXupdown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}