using System;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class usRecurring_Payment : UserControl
    {
        private readonly RecurringRepository _repo = new RecurringRepository();

        public usRecurring_Payment()
        {
            InitializeComponent();
            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM dd, yyyy");
            myDateTextBox.ReadOnly = true;

            _repo.EnsureTableExists();
            LoadFromDatabase();
            AddRoundedBorders();
        }

        private void LoadFromDatabase()
        {
            pnlRecurringList.Controls.Clear();

            foreach (var payment in _repo.GetAll(SessionManager.UserId))
            {
                AddRowToPanel(payment);
            }

            UpdateTotalAmount();
        }

        private void AddRowToPanel(RecurringPayment payment)
        {
            var newRow = new usRecurringRow();

            newRow.RecurringId = payment.Id;
            newRow.lblName.Text = payment.Name;
            newRow.lblAmount.Text = "₱" + payment.Amount.ToString("N2");
            newRow.lblFrequency.Text = payment.Frequency;
            newRow.lblNextDate.Text = payment.NextDate.ToString("MMM d, yyyy");
            newRow.lblCategory.Text = payment.Category;

            // 👈 Add this line
            newRow.ApplyStyles(payment.NextDate, payment.Category);

            newRow.RowDeleted += (s, ev) =>
            {
                if (s is usRecurringRow deletedRow)
                    _repo.Delete(deletedRow.RecurringId);

                UpdateTotalAmount();
            };

            pnlRecurringList.Controls.Add(newRow);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var popup = new AddRecurringForm())
            {
                popup.StartPosition = FormStartPosition.CenterParent;
                if (popup.ShowDialog(ParentForm) != DialogResult.OK) return;

                if (!decimal.TryParse(popup.FinalAmount, out decimal amount))
                {
                    MessageBox.Show("Please enter a valid amount.");
                    return;
                }

                var payment = new RecurringPayment
                {
                    Name = popup.FinalName,
                    Amount = amount,
                    Frequency = popup.FinalFrequency,
                    NextDate = popup.FinalDate,
                    Category = popup.FinalCategory
                };

                payment.Id = _repo.Add(SessionManager.UserId, payment);
                AddRowToPanel(payment);
                UpdateTotalAmount();
            }
        }

        private void AddRoundedBorders()
        {
            UIHelper.ApplyRoundedStyle(panel3);
            UIHelper.ApplyRoundedStyle(pnlRecurringList);
            //UIHelper.ApplyRoundedStyle(panel4);
            //UIHelper.ApplyRoundedStyle(panel5);
        }

        public void UpdateTotalAmount()
        {
            double total = 0;

            foreach (Control control in pnlRecurringList.Controls)
            {
                if (control is usRecurringRow row)
                {
                    string amtText = row.lblAmount.Text
                        .Replace("₱", "").Replace(",", "").Trim();

                    if (double.TryParse(amtText, out double val))
                        total += val;
                }
            }

            lblMonthlyTotal.Text = "₱" + total.ToString("N2");
        }

        // Unused stubs — keep to avoid designer errors
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void textBox6_TextChanged(object sender, EventArgs e) { }
        private void textBox7_TextChanged(object sender, EventArgs e) { }
        private void textBox8_TextChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.OpenAddTransaction(ParentForm, null);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(50, 60, 80), 1f);
            e.Graphics.DrawLine(pen, 0, panel7.Height - 1,
                                panel7.Width, panel7.Height - 1);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlRecurringList_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}