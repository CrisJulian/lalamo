using System;
using System.Drawing;
using System.Windows.Forms;

namespace CommonCents
{
    public class LogPaymentForm : Form
    {
        public double PaymentAmount => double.TryParse(txtAmount.Text, out var v) ? v : 0;

        private TextBox txtAmount;
        private Label lblInfo;

        public LogPaymentForm(Debt debt)
        {
            BuildUI(debt);
        }

        private void BuildUI(Debt debt)
        {
            Text = $"Log Payment — {debt.Name}";
            Size = new Size(360, 300);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(30, 35, 48);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10f);

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                Padding = new Padding(24),
                RowCount = 5
            };

            lblInfo = new Label
            {
                Text = $"{debt.Name}\n" +
                       $"Remaining: ₱{debt.Remaining:N2}\n" +
                       $"Monthly Payment: ₱{debt.MonthlyPayment:N2}",
                ForeColor = Color.FromArgb(200, 210, 230),
                AutoSize = false,
                Dock = DockStyle.Fill,
                Height = 65
            };
            panel.Controls.Add(lblInfo);

            var btnMonthly = new Button
            {
                Text = debt.PaidThisMonth
                    ? $"✓ Monthly Already Paid (₱{debt.MonthlyPayment:N2})"
                    : $"Pay Monthly (₱{debt.MonthlyPayment:N2})",
                BackColor = debt.PaidThisMonth
                    ? Color.FromArgb(50, 80, 50)
                    : Color.FromArgb(40, 130, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Height = 38,
                Enabled = !debt.PaidThisMonth
            };
            btnMonthly.FlatAppearance.BorderSize = 0;
            btnMonthly.Click += (s, e) =>
            {
                txtAmount.Text = debt.MonthlyPayment.ToString("F2");
                DialogResult = DialogResult.OK;
                Close();
            };
            panel.Controls.Add(btnMonthly);

            panel.Controls.Add(new Label
            {
                Text = "— or enter a custom amount —",
                ForeColor = Color.FromArgb(100, 110, 130),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Height = 30
            });

            txtAmount = new TextBox
            {
                PlaceholderText = "Enter amount (₱)",
                BackColor = Color.FromArgb(45, 52, 70),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Height = 32
            };
            panel.Controls.Add(txtAmount);

            var btnPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                Height = 44
            };

            var btnCancel = MakeBtn("Cancel");
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            var btnOk = MakeBtn("Log Payment", primary: true);
            btnOk.Click += (s, e) =>
            {
                if (PaymentAmount <= 0)
                {
                    MessageBox.Show("Enter a valid amount.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult = DialogResult.OK;
                Close();
            };

            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnOk);
            panel.Controls.Add(btnPanel);

            Controls.Add(panel);
        }

        private Button MakeBtn(string text, bool primary = false)
        {
            var b = new Button
            {
                Text = text,
                Height = 36,
                Width = 110,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(6, 4, 0, 0),
                Cursor = Cursors.Hand,
                BackColor = primary ? Color.Transparent : Color.FromArgb(80, 88, 108),
                ForeColor = primary ? Color.FromArgb(230, 170, 0) : Color.White
            };
            b.FlatAppearance.BorderSize = primary ? 1 : 0;
            b.FlatAppearance.BorderColor = Color.FromArgb(230, 170, 0);
            b.FlatAppearance.MouseOverBackColor = primary
                ? Color.FromArgb(30, 230, 170, 0)
                : Color.FromArgb(100, 108, 128);
            return b;
        }
    }
}