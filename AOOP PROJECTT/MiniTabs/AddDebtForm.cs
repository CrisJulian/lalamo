// AddDebtForm.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CommonCents
{
    public class AddDebtForm : Form
    {
        // outputs
        public string DebtName => txtName.Text.Trim();
        public double Principal => double.TryParse(txtPrincipal.Text, out var v) ? v : 0;
        public double InterestRate => double.TryParse(txtInterest.Text, out var v) ? v : 0;
        public double MonthlyPayment => double.TryParse(txtMonthly.Text, out var v) ? v : 0;
        public DateTime DueDate => dtpDue.Value;

        // controls
        private TextBox txtName, txtPrincipal, txtInterest, txtMonthly;
        private DateTimePicker dtpDue;
        private Label lblPreview;

        public AddDebtForm()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Add New Debt";
            Size = new Size(420, 430);
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
                ColumnCount = 2,
                Padding = new Padding(20),
                AutoSize = true
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int row = 0;

            txtName     = AddRow(panel, "Debt Name:",        row++);
            txtPrincipal = AddRow(panel, "Principal (₱):",   row++);
            txtInterest  = AddRow(panel, "Interest Rate (%):", row++);
            txtMonthly   = AddRow(panel, "Monthly Payment (₱):", row++);

            // due date
            panel.Controls.Add(MakeLabel("Due Date:"), 0, row);
            dtpDue = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddYears(1),
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 52, 70),
                ForeColor = Color.White,
                CalendarForeColor = Color.White,
                CalendarMonthBackground = Color.FromArgb(45, 52, 70)
            };
            panel.Controls.Add(dtpDue, 1, row++);

            // preview label
            lblPreview = new Label
            {
                Text = "",
                ForeColor = Color.FromArgb(255, 200, 50),
                AutoSize = false,
                Height = 40,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.SetColumnSpan(lblPreview, 2);
            panel.Controls.Add(lblPreview, 0, row++);

            // buttons
            var btnOk = MakeButton("Add Debt", Color.FromArgb(230, 170, 0));
            btnOk.Click += (s, e) =>
            {
                if (!Validate_()) return;
                DialogResult = DialogResult.OK;
                Close();
            };

            var btnCancel = MakeButton("Cancel", Color.FromArgb(80, 88, 108));
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            var btnPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnOk);
            panel.SetColumnSpan(btnPanel, 2);
            panel.Controls.Add(btnPanel, 0, row);

            Controls.Add(panel);

            // live preview
            foreach (Control c in new Control[] { txtPrincipal, txtInterest })
                if (c is TextBox tb) tb.TextChanged += UpdatePreview;
        }

        private void UpdatePreview(object s, EventArgs e)
        {
            if (double.TryParse(txtPrincipal.Text, out var p) &&
                double.TryParse(txtInterest.Text, out var r))
            {
                double total = p * (1 + r / 100);
                lblPreview.Text = $"Total with {r}% interest: ₱{total:N2}";
            }
            else
            {
                lblPreview.Text = "";
            }
        }

        private bool Validate_()
        {
            if (string.IsNullOrWhiteSpace(DebtName))   { Warn("Enter a debt name."); return false; }
            if (Principal <= 0)                         { Warn("Enter a valid principal amount."); return false; }
            if (InterestRate < 0)                       { Warn("Interest rate cannot be negative."); return false; }
            if (MonthlyPayment <= 0)                    { Warn("Enter a valid monthly payment."); return false; }
            return true;
        }

        private void Warn(string msg) =>
            MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        // helpers
        private TextBox AddRow(TableLayoutPanel panel, string label, int row)
        {
            panel.Controls.Add(MakeLabel(label), 0, row);
            var tb = new TextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 52, 70),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panel.Controls.Add(tb, 1, row);
            return tb;
        }

        private Label MakeLabel(string text) => new Label
        {
            Text = text,
            ForeColor = Color.FromArgb(160, 170, 190),
            TextAlign = ContentAlignment.MiddleRight,
            Dock = DockStyle.Fill
        };

        private Button MakeButton(string text, Color bg)
        {
            var b = new Button
            {
                Text = text,
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Height = 36,
                Width = 110,
                Margin = new Padding(6, 4, 0, 0)
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }
    }
}
