using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AOOP_PROJECTT
{
    public partial class usTransactionRow : UserControl
    {
        private int _transactionId;

        [DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        public usTransactionRow()
        {
            InitializeComponent();
        }

        public void SetTransactionData(int transactionId, string date, string desc, string cat, string type, string amount)
        {
            _transactionId = transactionId;

            lblDate.Text = date;
            lblDescription.Text = desc;
            lblCategory.Text = cat;
            lblType.Text = type;

            if (double.TryParse(amount, out double val))
            {
                string formatted = val.ToString("N2");
                if (type == "Income")
                {
                    lblAmount.Text = "+₱" + formatted;
                    lblAmount.ForeColor = Color.MediumSeaGreen;
                    lblType.ForeColor = Color.MediumSeaGreen;
                }
                else
                {
                    lblAmount.Text = "-₱" + formatted;
                    lblAmount.ForeColor = Color.LightCoral;
                    lblType.ForeColor = Color.LightCoral;
                }
            }
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

        private void usTransactionRow_Load(object sender, EventArgs e)
        {
            var deleteBtn = MakeDeleteButton();
            deleteBtn.Location = new Point(btnDelete.Location.X, 8);
            deleteBtn.Click += (s, ev) =>
            {
                var confirm = MessageBox.Show("Delete this transaction?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    TransactionRepository.DeleteTransaction(_transactionId);
                    this.Parent?.Controls.Remove(this);
                }
            };
            Controls.Add(deleteBtn);
            deleteBtn.BringToFront();
        }

        private void lblCategory_Click(object sender, EventArgs e) { }
        private void lblDate_Click(object sender, EventArgs e) { }
        private void lblAmount_Click(object sender, EventArgs e) { }
        private void lblDescription_Click(object sender, EventArgs e) { }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Delete this transaction?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                TransactionRepository.DeleteTransaction(_transactionId);
                this.Parent?.Controls.Remove(this);
            }
        }
    }
}