using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class AddTransactionForm : Form
    {
        // Add these attributes to kill the WFO100C errors
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TDate { get; set; }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TDescription { get; set; }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TCategory { get; set; }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TType { get; set; }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TAmount { get; set; }
        public AddTransactionForm()
        {
            InitializeComponent();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Capture data from your named controls
            TDate = dtpDate.Value.ToString("MMM dd");
            TDescription = txtDescription.Text.Trim();
            TCategory = cmbCategory.Text;
            TType = cmbType.Text;
            TAmount = txtAmount.Text.Trim();

            // 2. Validation: Don't allow empty entries
            if (string.IsNullOrWhiteSpace(TDescription) || TAmount == "" || TAmount == "0.00")
            {
                MessageBox.Show("Please enter a valid description and amount.");
                return;
            }

            this.DialogResult = DialogResult.OK; // Signal success
            this.Close();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
