using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AOOP_PROJECTT
{
    public partial class usDashboard : UserControl
    {
        public usDashboard()
        {
            InitializeComponent();

            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM dd, yyyy");
            myDateTextBox.ReadOnly = true;
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void myDateTextBox_TextChanged(object sender, EventArgs e)
        {
     

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            panel3.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(50, 60, 80), 1.5f);
                using var path = RoundedRect(new Rectangle(0, 0, panel3.Width - 1, panel3.Height - 1), 10);
                e.Graphics.DrawPath(pen, path);
            };
        }
        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
