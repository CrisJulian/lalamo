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
    }
}
