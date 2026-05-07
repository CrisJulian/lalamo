using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class usTransaction : UserControl
    {
        public usTransaction()
        {

            InitializeComponent();
            this.Dock = DockStyle.Fill;

            panel6.Dock = DockStyle.Top;
            panel6.Height = 56;

            panel1.Dock = DockStyle.Top;
            panel1.Height = 54;

            panel2.Dock = DockStyle.Fill;

            panel3.Dock = DockStyle.Top;
            panel3.Height = 45;

            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.AutoSize = false;

            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.AutoSize = false;
            this.Dock = DockStyle.Fill;  // add this line
            var lbl = new Label() { Text = "TRANSACTION PAGE", ForeColor = Color.White, Dock = DockStyle.Top };
            this.Controls.Add(lbl);

        }

        private void usTransaction_Load(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
