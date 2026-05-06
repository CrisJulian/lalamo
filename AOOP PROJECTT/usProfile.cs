using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class usProfile : UserControl
    {
        public usProfile()
        {
            InitializeComponent();

            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM dd, yyyy");
            myDateTextBox.ReadOnly = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void darkpanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void myDateTextBox_TextChanged(object sender, EventArgs e)
        {
            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM dd, yyyy");
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void usProfile_Load(object sender, EventArgs e)
        {

        }
    }
}
