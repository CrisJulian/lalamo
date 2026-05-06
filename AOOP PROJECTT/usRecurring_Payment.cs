using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class usRecurring_Payment : UserControl
    {
        public usRecurring_Payment()
        {
            InitializeComponent();

            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM dd, yyyy");
            myDateTextBox.ReadOnly = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var popup = new AddRecurringForm())
            {
                if (popup.ShowDialog() == DialogResult.OK)
                {
                    // 1. Create the new row template
                    usRecurringRow newRow = new usRecurringRow();

                    // 2. Map the popup data directly to the new row labels
                    newRow.lblName.Text = popup.FinalName;
                    newRow.lblAmount.Text = "₱" + popup.FinalAmount;
                    newRow.lblFrequency.Text = popup.FinalFrequency;
                    newRow.lblNextDate.Text = popup.FinalDate.ToString("MM/dd/yyyy");
                    newRow.lblCategory.Text = popup.FinalCategory;
                    newRow.RowDeleted += (s, ev) => UpdateTotalAmount();
                    // 3. Stamp it into your list
                    pnlRecurringList.Controls.Add(newRow);
                    UpdateTotalAmount();

                }
            }
        }

        public void UpdateTotalAmount()
        {
            double total = 0;

            // Loop through every 'stamp' inside your panel
            foreach (Control control in pnlRecurringList.Controls)
            {
                // Check if the control is one of your row templates
                if (control is usRecurringRow row)
                {
                    // Clean the string (remove ₱ and commas) so C# can do math
                    string amtText = row.lblAmount.Text.Replace("₱", "").Replace(",", "").Trim();

                    if (double.TryParse(amtText, out double val))
                    {
                        total += val;
                    }
                }
            }

            // Update the big label on your dashboard
            // Note: Ensure your big total label is named 'lblMonthlyTotal' in Properties
            lblMonthlyTotal.Text = "₱" + total.ToString("N2");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
