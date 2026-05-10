namespace AOOP_PROJECTT.MiniTabs
{
    public class AddBudgetForm : Form
    {
        public string BudgetName => txtName.Text.Trim();
        public double Limit => double.TryParse(txtLimit.Text, out var v) ? v : 0;
        public Color ChosenColor { get; private set; }

        private TextBox txtName, txtLimit;

        private static readonly Color[] Palette = new[]
        {
            Color.FromArgb(230, 170,   0),
            Color.FromArgb( 80, 200, 120),
            Color.FromArgb( 80, 180, 240),
            Color.FromArgb(180,  80, 240),
            Color.FromArgb(240,  80, 120),
            Color.FromArgb(240, 140,  60),
            Color.FromArgb( 80, 220, 200),
            Color.FromArgb(220,  80, 200),
        };

        private static readonly Random _rng = new Random();

        public AddBudgetForm()
        {
            ChosenColor = Palette[_rng.Next(Palette.Length)];
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Add Budget Category";
            Size = new Size(340, 200);
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
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int row = 0;
            txtName = AddCategoryDropdown(panel, "Category Name:", row++);
            txtLimit = AddRow(panel, "Monthly Limit (₱):", row++);

            var btnPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            var btnCancel = MakeBtn("Cancel", Color.FromArgb(80, 88, 108));
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            var btnOk = MakeBtn("Add Budget", Color.FromArgb(230, 170, 0));
            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(BudgetName)) { Warn("Enter a category name."); return; }
                if (Limit <= 0) { Warn("Enter a valid limit."); return; }
                DialogResult = DialogResult.OK;
                Close();
            };

            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnOk);
            panel.SetColumnSpan(btnPanel, 2);
            panel.Controls.Add(btnPanel, 0, row);

            Controls.Add(panel);
        }

        private TextBox AddCategoryDropdown(TableLayoutPanel p, string lbl, int row)
        {
            p.Controls.Add(MakeLbl(lbl), 0, row);
            var cmb = new ComboBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 52, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmb.Items.AddRange(new object[] { "Food", "Transport", "Entertainment",
        "Utilities", "Healthcare", "Shopping", "Health", "Software", "Other" });
            cmb.SelectedIndex = 0;
            p.Controls.Add(cmb, 1, row);
            var proxy = new TextBox { Visible = false };
            cmb.SelectedIndexChanged += (s, e) => proxy.Text = cmb.Text;
            proxy.Text = cmb.Text;
            Controls.Add(proxy);
            return proxy;
        }

        private TextBox AddRow(TableLayoutPanel p, string lbl, int row)
        {
            p.Controls.Add(MakeLbl(lbl), 0, row);
            var tb = new TextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 52, 70),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            p.Controls.Add(tb, 1, row);
            return tb;
        }

        private Label MakeLbl(string t) => new Label
        {
            Text = t,
            ForeColor = Color.FromArgb(160, 170, 190),
            TextAlign = ContentAlignment.MiddleRight,
            Dock = DockStyle.Fill
        };

        private Button MakeBtn(string t, Color bg)
        {
            var b = new Button
            {
                Text = t,
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Height = 34,
                Width = 110,
                Margin = new Padding(6, 4, 0, 0)
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private void Warn(string msg) =>
            MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}