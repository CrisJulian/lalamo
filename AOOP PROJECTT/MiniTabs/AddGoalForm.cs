namespace AOOP_PROJECTT.MiniTabs
{
    public class AddGoalForm : Form
    {
        public string GoalName => txtName.Text.Trim();
        public double Target => double.TryParse(txtTarget.Text, out var v) ? v : 0;
        public double InitialSaved => double.TryParse(txtSaved.Text, out var v) ? v : 0;
        public Color ChosenColor { get; private set; }

        private TextBox txtName, txtTarget, txtSaved;

        private static readonly Color[] Palette = new[]
        {
            Color.FromArgb( 80, 220, 120),
            Color.FromArgb( 80, 180, 240),
            Color.FromArgb(230, 170,   0),
            Color.FromArgb(180,  80, 240),
            Color.FromArgb(240,  80, 120),
            Color.FromArgb( 80, 220, 200),
            Color.FromArgb(240, 140,  60),
            Color.FromArgb(220,  80, 200),
        };

        private static readonly Random _rng = new Random();

        public AddGoalForm()
        {
            ChosenColor = Palette[_rng.Next(Palette.Length)];
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Add Savings Goal";
            Size = new Size(340, 230);
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
            txtName = AddRow(panel, "Goal Name:", row++);
            txtTarget = AddRow(panel, "Target Amount (₱):", row++);
            txtSaved = AddRow(panel, "Already Saved (₱):", row++);

            var btnPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            var btnCancel = MakeBtn("Cancel", Color.FromArgb(80, 88, 108));
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            var btnOk = MakeBtn("Add Goal", Color.FromArgb(230, 170, 0));
            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(GoalName)) { Warn("Enter a goal name."); return; }
                if (Target <= 0) { Warn("Enter a valid target amount."); return; }
                DialogResult = DialogResult.OK;
                Close();
            };
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnOk);
            panel.SetColumnSpan(btnPanel, 2);
            panel.Controls.Add(btnPanel, 0, row);

            Controls.Add(panel);
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