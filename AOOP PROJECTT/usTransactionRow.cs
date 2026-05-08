using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class usTransactionRow : UserControl
    {
        public usTransactionRow()
        {
            InitializeComponent();
        }

        // The "Bridge" method to fill the row with data
        // Paste this inside the usTransactionRow class
        public void SetTransactionData(string date, string desc, string cat, string type, string amount)
        {
            lblDate.Text = date;
            lblDescription.Text = desc;
            lblCategory.Text = cat;
            lblType.Text = type;

            if (double.TryParse(amount, out double val))
            {
                string formatted = val.ToString("N2");
                if (type == "Income")
                {
                    lblAmount.Text = "+₱" + formatted;
                    lblAmount.ForeColor = Color.MediumSeaGreen;
                    lblType.ForeColor = Color.MediumSeaGreen;
                }
                else
                {
                    lblAmount.Text = "-₱" + formatted;
                    lblAmount.ForeColor = Color.LightCoral;
                    lblType.ForeColor = Color.LightCoral;
                }
            }
        }

        private void usTransactionRow_Load(object sender, EventArgs e)
        {

        }

        private void lblCategory_Click(object sender, EventArgs e)
        {

        }

        private void lblDate_Click(object sender, EventArgs e)
        {

        }

        private void lblAmount_Click(object sender, EventArgs e)
        {

        }

        private void lblDescription_Click(object sender, EventArgs e)
        {

        }
    }
}