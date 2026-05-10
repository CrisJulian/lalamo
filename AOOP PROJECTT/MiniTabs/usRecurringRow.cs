using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace AOOP_PROJECTT
{
    public partial class usRecurringRow : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RecurringId { get; set; } = 0;
        public event EventHandler RowDeleted;

        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        public usRecurringRow()
        {
            InitializeComponent();

            btnDelete.Visible = false;

            var circleBtn = MakeDeleteButton();
            circleBtn.Location = new Point(this.Width - 40, (this.Height - 28) / 2);
            this.Controls.Add(circleBtn);

            // Wire up click
            circleBtn.Click += btnDelete_Click_1;

            // Draw separator line
            this.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(50, 60, 80), 1f);
                e.Graphics.DrawLine(pen, 0, this.Height - 1, this.Width, this.Height - 1);
            };
        }

        private Panel MakeDeleteButton()
        {
            var circle = new Panel
            {
                Width = 28,
                Height = 28,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            var lbl = new Label
            {
                Text = "X",
                Font = new Font("Segoe UI", 6.5f, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = false,
                Size = new Size(28, 28),
                Location = new Point(0, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Enabled = false
            };

            circle.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(Color.FromArgb(200, 60, 60));
                e.Graphics.FillEllipse(brush, 1, 1, circle.Width - 3, circle.Height - 3);
            };

            circle.Controls.Add(lbl);
            return circle;
        }

        public void ApplyStyles(DateTime nextDate, string category)
        {
            // 1. Colored dot
            panelDot.BackColor = GetCategoryColor(category);

            panelDot.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, panelDot.Width, panelDot.Height,
                                   panelDot.Width, panelDot.Height));

            // 2. Due date text
            int daysUntilDue = (nextDate.Date - DateTime.Today).Days;
            if (daysUntilDue < 0)
                lblDueStatus.Text = $"Overdue by {Math.Abs(daysUntilDue)} days";
            else if (daysUntilDue == 0)
                lblDueStatus.Text = "Due today";
            else
                lblDueStatus.Text = $"In {daysUntilDue} days";

            lblDueStatus.ForeColor = daysUntilDue <= 0 ? Color.Crimson : Color.FromArgb(100, 180, 100);

            // 3. Category badge 
            Color catColor = GetCategoryColor(category);
            lblCategory.ForeColor = catColor;
            lblCategory.BackColor = Color.FromArgb(22, 27, 38);
            lblCategory.Padding = new Padding(6, 2, 6, 2);
            lblCategory.AutoSize = true;
            lblCategory.Text = category?.ToUpper();

            lblCategory.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(catColor, 1.5f);
                e.Graphics.DrawRoundedRectangle(pen, 0, 0,
                    lblCategory.Width - 1, lblCategory.Height - 1, 8);
            };
        }

        private Color GetCategoryColor(string category)
        {
            return category?.ToLower() switch
            {
                "food" => Color.FromArgb(245, 166, 35),
                "transport" => Color.FromArgb(74, 144, 217),
                "entertainment" => Color.FromArgb(155, 89, 182),
                "utilities" => Color.FromArgb(243, 156, 18),
                "healthcare" => Color.FromArgb(231, 76, 60),
                "health" => Color.FromArgb(39, 174, 96),
                "shopping" => Color.FromArgb(233, 30, 99),
                "software" => Color.FromArgb(52, 152, 219),
                "income" => Color.FromArgb(46, 204, 113),
                _ => Color.FromArgb(149, 165, 166)
            };
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (this.Parent != null)
                this.Parent.Controls.Remove(this);

            RowDeleted?.Invoke(this, EventArgs.Empty);
            this.Dispose();
        }

        private void lblCategory_TextChanged(object sender, EventArgs e) { }

        private void lblName_TextChanged(object sender, EventArgs e) { }
    }

    public static class GraphicsExtensions
    {
        public static void DrawRoundedRectangle(this Graphics g, Pen pen,
            int x, int y, int width, int height, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(x, y, radius * 2, radius * 2, 180, 90);
            path.AddArc(width - radius * 2, y, radius * 2, radius * 2, 270, 90);
            path.AddArc(width - radius * 2, height - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(x, height - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            g.DrawPath(pen, path);
        }
    }
}