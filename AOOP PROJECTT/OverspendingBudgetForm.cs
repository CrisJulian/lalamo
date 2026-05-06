// warning if the user spend the budget limit money
using System.Drawing;
using System.Windows.Forms;

namespace CommonCents
{
    public class AddSpendingForm : Form
    {
        public double Amount => double.TryParse(txtAmount.Text, out var v) ? v : 0;
        private TextBox txtAmount;

        public AddSpendingForm(Budget budget)
        {
            BuildUI(budget);
        }

        private void BuildUI(Budget budget)
        {
            Text = $"Add Spending — {budget.Name}";
            Size = new Size(320, 200);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            BackColor = Color.FromArgb(30, 35, 48);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10f);

            var panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(20), RowCount = 4 };

            panel.Controls.Add(new Label
            {
                Text = $"{budget.Name}  —  ₱{budget.Remaining:N2} remaining of ₱{budget.Limit:N2} limit",
                ForeColor = Color.FromArgb(180, 190, 210),
                AutoSize = false,
                Dock = DockStyle.Fill,
                Height = 40
            });

            panel.Controls.Add(new Label { Text = "Spending Amount (₱):", ForeColor = Color.FromArgb(160, 170, 190), AutoSize = true });

            txtAmount = new TextBox { Dock = DockStyle.Fill, BackColor = Color.FromArgb(45, 52, 70), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            panel.Controls.Add(txtAmount);

            var btnRow = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill, AutoSize = true };
            var btnCancel = MakeBtn("Cancel", Color.FromArgb(80, 88, 108));
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            var btnOk = MakeBtn("Add", Color.FromArgb(230, 170, 0));
            btnOk.Click += (s, e) =>
            {
                if (Amount <= 0) { MessageBox.Show("Enter a valid amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                DialogResult = DialogResult.OK; Close();
            };
            btnRow.Controls.Add(btnCancel);
            btnRow.Controls.Add(btnOk);
            panel.Controls.Add(btnRow);
            Controls.Add(panel);
        }

        private Button MakeBtn(string t, Color bg)
        {
            var b = new Button { Text = t, BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Height = 34, Width = 100, Margin = new Padding(6, 4, 0, 0) };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }
    }
}
