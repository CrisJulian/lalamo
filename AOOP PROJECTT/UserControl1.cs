using AOOP_PROJECTT;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class UserControl1 : UserControl
    {
        public event EventHandler LogoutRequested;
        public UserControl1()
        {
            InitializeComponent();
            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM d, yyyy"); // 👈 add this
            myDateTextBox.ReadOnly = true; // 👈 add this too so it can't be edited
            ApplyIconBadges();
            AddRoundedBorders();
        }

        private void ApplyIconBadges()
        {
            // LOGIN — orange circle with L
            label11.Text = "L";
            label11.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            label11.ForeColor = Color.White;
            label11.BackColor = Color.FromArgb(255, 128, 0);
            label11.Size = new Size(36, 36);
            label11.TextAlign = ContentAlignment.MiddleCenter;
            label11.AutoSize = false;
            MakeCircle(label11);

            // EXPORT DATA — white circle with ED
            label17.Text = "ED";
            label17.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            label17.ForeColor = Color.FromArgb(24, 28, 38);
            label17.BackColor = Color.White;
            label17.Size = new Size(36, 36);
            label17.TextAlign = ContentAlignment.MiddleCenter;
            label17.AutoSize = false;
            MakeCircle(label17);

            // BACKUP DATA — white circle with BD
            label20.Text = "BD";
            label20.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            label20.ForeColor = Color.FromArgb(24, 28, 38);
            label20.BackColor = Color.White;
            label20.Size = new Size(36, 36);
            label20.TextAlign = ContentAlignment.MiddleCenter;
            label20.AutoSize = false;
            MakeCircle(label20);

            // CLEAR ALL DATA — red circle with CLD
            label13.Text = "CLD";
            label13.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            label13.ForeColor = Color.White;
            label13.BackColor = Color.FromArgb(192, 0, 0);
            label13.Size = new Size(36, 36);
            label13.TextAlign = ContentAlignment.MiddleCenter;
            label13.AutoSize = false;
            MakeCircle(label13);

            // APP VERSION — grey square with APV
            label9.Text = "APV";
            label9.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            label9.ForeColor = Color.White;
            label9.BackColor = Color.FromArgb(90, 95, 110);
            label9.Size = new Size(36, 36);
            label9.TextAlign = ContentAlignment.MiddleCenter;
            label9.AutoSize = false;
            // No MakeCircle call = stays as square
        }

        private void AddRoundedBorders()
        {
            UIHelper.ApplyRoundedStyle(panel3);
            UIHelper.ApplyRoundedStyle(panel4);
            UIHelper.ApplyRoundedStyle(panel8);
            UIHelper.ApplyRoundedStyle(panel6);
        }

        private void MakeCircle(Label lbl)
        {
            Color circleColor = lbl.BackColor;
            Color textColor = lbl.ForeColor;
            string text = lbl.Text;
            Font font = lbl.Font;

            lbl.Text = "";  // Clear text so label doesn't draw it
            lbl.BackColor = lbl.Parent?.BackColor ?? Color.FromArgb(24, 28, 38);

            lbl.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw circle
                using var brush = new SolidBrush(circleColor);
                e.Graphics.FillEllipse(brush, 0, 0, lbl.Width - 1, lbl.Height - 1);

                // Draw text centered
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                using var textBrush = new SolidBrush(textColor);
                e.Graphics.DrawString(text, font, textBrush,
                    new RectangleF(0, 0, lbl.Width, lbl.Height), sf);
            };

            lbl.Invalidate();
        }

        private void MakeSquare(Label lbl)
        {
            Color squareColor = lbl.BackColor;
            Color textColor = lbl.ForeColor;
            string text = lbl.Text;
            Font font = lbl.Font;

            lbl.Text = "";
            lbl.BackColor = lbl.Parent?.BackColor ?? Color.FromArgb(24, 28, 38);

            lbl.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw rounded square
                using var brush = new SolidBrush(squareColor);
                e.Graphics.FillRoundedRectangle(brush, 0, 0, lbl.Width - 1, lbl.Height - 1, 6);

                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                using var textBrush = new SolidBrush(textColor);
                e.Graphics.DrawString(text, font, textBrush,
                    new RectangleF(0, 0, lbl.Width, lbl.Height), sf);
            };

            lbl.Invalidate();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv";
                sfd.FileName = $"CacheFlow_Export_{DateTime.Now:yyyyMMdd}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var conn = DatabaseHelper.GetConnection())
                        {
                            conn.Open();
                            // We use t.* for Transactions and c.* for Categories
                            // I'm using c.Name here as a common standard, but let's 
                            // wrap it in a try-catch to be safe.
                            string query = @"
                        SELECT t.Date, t.Amount, t.Type, c.Name, t.Description 
                        FROM Transactions t
                        LEFT JOIN Categories c ON t.CategoryId = c.CategoryId
                        WHERE t.UserId = @id 
                        ORDER BY t.Date DESC";

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", SessionManager.UserId);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName))
                                {
                                    sw.WriteLine("Date,Amount,Type,Category,Description");

                                    while (reader.Read())
                                    {
                                        string date = Convert.ToDateTime(reader["Date"]).ToString("yyyy-MM-dd");
                                        string amount = reader["Amount"].ToString();
                                        string type = reader["Type"].ToString();

                                        // Safe check: if 'Name' isn't the right column in Categories, 
                                        // it will fall back to the CategoryId number so it doesn't crash.
                                        string categoryName = "General";
                                        try { categoryName = reader["Name"].ToString(); }
                                        catch { categoryName = reader.GetSchemaTable().Columns.Contains("CategoryName") ? reader["CategoryName"].ToString() : "ID: " + reader.GetInt32(reader.GetOrdinal("CategoryId")); }

                                        string desc = $"\"{reader["Description"]}\"";
                                        sw.WriteLine($"{date},{amount},{type},{categoryName},{desc}");
                                    }
                                }
                            }
                        }
                        MessageBox.Show("Export successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Still hitting a snag: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void label20_Click(object sender, EventArgs e)
        {
            label17_Click(sender, e);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
        "This will permanently delete ALL your data. Are you sure?",
        "Clear All Data",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using var conn = DatabaseHelper.GetConnection();
                    conn.Open();
                    int id = SessionManager.UserId;
                    new Microsoft.Data.SqlClient.SqlCommand($"DELETE FROM DebtPayments WHERE DebtId IN (SELECT DebtId FROM Debts WHERE UserId={id})", conn).ExecuteNonQuery();
                    new Microsoft.Data.SqlClient.SqlCommand($"DELETE FROM Debts WHERE UserId={id}", conn).ExecuteNonQuery();
                    new Microsoft.Data.SqlClient.SqlCommand($"DELETE FROM Transactions WHERE UserId={id}", conn).ExecuteNonQuery();
                    new Microsoft.Data.SqlClient.SqlCommand($"DELETE FROM Budgets WHERE UserId={id}", conn).ExecuteNonQuery();
                    new Microsoft.Data.SqlClient.SqlCommand($"DELETE FROM SavingsGoals WHERE UserId={id}", conn).ExecuteNonQuery();
                    new Microsoft.Data.SqlClient.SqlCommand($"DELETE FROM RecurringPayments WHERE UserId={id}", conn).ExecuteNonQuery();
                    MessageBox.Show("All data cleared successfully.", "Done");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error");
                }
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {
            SessionManager.UserId = 0;
            SessionManager.FullName = string.Empty;
            SessionManager.Email = string.Empty;

            // 2. Trigger an event to tell the Main Form to show the Login Control
            // You likely have a custom event handler for this in your project architecture
            LogoutRequested?.Invoke(this, EventArgs.Empty);

            MessageBox.Show("You have been logged out.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.OpenAddTransaction(ParentForm, null);
        }

        private void myDateTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}