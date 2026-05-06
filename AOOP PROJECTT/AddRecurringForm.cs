using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class AddRecurringForm : Form

    {
        // These 'grab' the current values from your controls
        public string FinalName => textBox1.Text;
        public string FinalAmount => textBox2.Text;
        public string FinalFrequency => comboBox1.Text;
        public string FinalCategory => comboBox2.Text;
        public DateTime FinalDate => dateTimePicker1.Value;

        public AddRecurringForm()
        {
            InitializeComponent();
            textBox1.Text = "e.g. Netflix";
            textBox1.ForeColor = Color.Gray;

            textBox2.Text = "0.00";
            textBox2.ForeColor = Color.Gray;
            this.ActiveControl = label1;
        }




        private void textBox1_Enter(object sender, EventArgs e)
        {
            // If the text currently in the box is the placeholder, clear it
            if (textBox1.Text == "e.g. Netflix")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.White; // Changes text to your main theme color
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            // If the user left the box empty or just typed spaces
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "e.g. Netflix";
                textBox1.ForeColor = Color.Gray; // Dim the color to show it's a placeholder
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "0.00")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.White; // Changes text to your main theme color
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "0.00";
                textBox2.ForeColor = Color.Gray; // Dim the color to show it's a placeholder
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "e.g. Netflix" || string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter a valid name for the recurring item.");
                return;
            }

            // 2. Set the result to OK so the main dashboard knows to update
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
