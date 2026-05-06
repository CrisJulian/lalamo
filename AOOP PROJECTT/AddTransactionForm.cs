using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class AddTransactionForm : Form
    {
        // Outputs
        public string TransactionName => txtName.Text.Trim();
        public double Amount => double.TryParse(txtAmount.Text, out var v) ? v : 0;
        public string Category => cmbCategory.SelectedItem?.ToString() ?? "";
        public DateTime TransactionDate => dtpDate.Value;
        public string TransactionType => cmbType.SelectedItem?.ToString() ?? "Expense";

        // Controls
        private TextBox txtName, txtAmount;
        private ComboBox cmbCategory, cmbType;
        private DateTimePicker dtpDate;
        private Label lblPreview;

        public AddTransactionForm()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Add Transaction";
            Size = new Size(420, 480);
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

            // Transaction Name
            txtName = AddRow(panel, "Transaction Name:", row++);

            // Amount
            txtAmount = AddRow(panel, "Amount (₱):", row++);

            // Transaction Type (Income/Expense)
            panel.Controls.Add(MakeLabel("Type:"), 0, row);
            cmbType = new ComboBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 52, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbType.Items.AddRange(new[] { "Expense", "Income" });
            cmbType.SelectedIndex = 0; // Default to Expense
            cmbType.SelectedIndexChanged += UpdatePreview;
            panel.Controls.Add(cmbType, 1, row++);

            // Category
            panel.Controls.Add(MakeLabel("Category:"), 0, row);
            cmbCategory = new ComboBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 52, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cmbCategory.Items.AddRange(new[] {
                "Food & Dining",
                "Transportation",
                "Shopping",
                "Entertainment",
                "Bills & Utilities",
                "Healthcare",
                "Education",
                "Salary",
                "Business",
                "Other"
            });
            cmbCategory.SelectedIndex = 0;
            panel.Controls.Add(cmbCategory, 1, row++);

            // Date
            panel.Controls.Add(MakeLabel("Date:"), 0, row);
            dtpDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 52, 70),
                ForeColor = Color.White,
                CalendarForeColor = Color.White,
                CalendarMonthBackground = Color.FromArgb(45, 52, 70)
            };
            panel.Controls.Add(dtpDate, 1, row++);

            // Preview label
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

            // Buttons
            var btnOk = MakeButton("Add Transaction", Color.FromArgb(230, 170, 0));
            btnOk.Click += (s, e) =>
            {
                if (!ValidateForm()) return;
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

            // Live preview
            txtAmount.TextChanged += UpdatePreview;
            cmbType.SelectedIndexChanged += UpdatePreview;
        }

        private void UpdatePreview(object sender, EventArgs e)
        {
            if (double.TryParse(txtAmount.Text, out var amount) && amount > 0)
            {
                string type = cmbType.SelectedItem?.ToString() ?? "Expense";
                string symbol = type == "Income" ? "+" : "-";
                string color = type == "Income" ? "Green" : "Red";

                lblPreview.Text = $"{type}: {symbol}₱{amount:N2}";
                lblPreview.ForeColor = type == "Income" ? Color.LightGreen : Color.FromArgb(255, 150, 150);
            }
            else
            {
                lblPreview.Text = "Enter a valid amount";
                lblPreview.ForeColor = Color.FromArgb(255, 200, 50);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(TransactionName))
            {
                Warn("Please enter a transaction name.");
                return false;
            }
            if (Amount <= 0)
            {
                Warn("Please enter a valid amount.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Category))
            {
                Warn("Please select a category.");
                return false;
            }
            return true;
        }

        private void Warn(string msg)
        {
            MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Helpers
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
                Width = 130,
                Margin = new Padding(6, 4, 0, 0)
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }
    }
}