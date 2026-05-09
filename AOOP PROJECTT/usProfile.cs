using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class usProfile : UserControl
    {
        public usProfile()
        {
            InitializeComponent();

            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM dd, yyyy");
            myDateTextBox.ReadOnly = true;
        }

        // --- Existing UI Event Handlers (Kept for Designer Compatibility) ---
        private void label3_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void darkpanel3_Paint(object sender, PaintEventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void lblInfoLocation_Click(object sender, EventArgs e) { }
        private void myDateTextBox_TextChanged(object sender, EventArgs e)
        {
            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM dd, yyyy");
        }

        private void usProfile_Load(object sender, EventArgs e)
        {
            LoadProfileData();
        }

        private void LoadProfileData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // 1. Fetch Personal Information
                    string userQuery = "SELECT FullName, Email, Phone, Location, AccountType, CreatedAt FROM Users WHERE UserId = @id";
                    using (SqlCommand cmd = new SqlCommand(userQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", SessionManager.UserId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblUserFullName.Text = reader["FullName"].ToString();
                                lblInfoFullName.Text = reader["FullName"].ToString();
                                lblInfoEmail.Text = reader["Email"].ToString();

                                string phone = reader["Phone"].ToString();
                                lblInfoPhone.Text = string.IsNullOrEmpty(phone) ? "Not Provided" : phone;

                                string loc = reader["Location"].ToString();
                                lblInfoLocation.Text = string.IsNullOrEmpty(loc) ? "Not Provided" : loc;

                                if (DateTime.TryParse(reader["CreatedAt"].ToString(), out DateTime joinDate))
                                {
                                    lblMemberSince.Text = joinDate.ToString("MMMM dd, yyyy");
                                }
                            }
                        }
                    }

                    // 2. Fetch Transaction and Budget Stats
                    // This pulls live counts from your tables
                    string statsQuery = @"
                        SELECT 
                            (SELECT COUNT(*) FROM Transactions WHERE UserId = @id) AS TotalTrans,
                            (SELECT COUNT(*) FROM Budgets WHERE UserId = @id) AS TotalBudgets";

                    using (SqlCommand cmdStats = new SqlCommand(statsQuery, conn))
                    {
                        cmdStats.Parameters.AddWithValue("@id", SessionManager.UserId);
                        using (SqlDataReader statsReader = cmdStats.ExecuteReader())
                        {
                            if (statsReader.Read())
                            {
                                lblTotalTransactions.Text = statsReader["TotalTrans"].ToString();
                                lblActiveBudgets.Text = statsReader["TotalBudgets"].ToString();
                            }
                        }
                    }

                    // 3. Fetch Savings Rate from vw_UserBalance
                    string savingsQuery = "SELECT TotalIncome, TotalExpenses FROM vw_UserBalance WHERE UserId = @id";
                    using (SqlCommand cmdSavings = new SqlCommand(savingsQuery, conn))
                    {
                        cmdSavings.Parameters.AddWithValue("@id", SessionManager.UserId);
                        using (SqlDataReader saveReader = cmdSavings.ExecuteReader())
                        {
                            if (saveReader.Read())
                            {
                                decimal income = saveReader["TotalIncome"] != DBNull.Value ? Convert.ToDecimal(saveReader["TotalIncome"]) : 0;
                                decimal expenses = saveReader["TotalExpenses"] != DBNull.Value ? Convert.ToDecimal(saveReader["TotalExpenses"]) : 0;

                                if (income > 0)
                                {
                                    decimal rate = ((income - expenses) / income) * 100;
                                    lblSavingsRate.Text = $"{(int)rate}%";
                                }
                                else
                                {
                                    lblSavingsRate.Text = "0%";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing profile: " + ex.Message);
            }
        }

        // This is the trigger that refreshes the page when you switch tabs
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                LoadProfileData();
            }
        }

        private void usProfile_VisibleChanged(object sender, EventArgs e)
        {
            // Leave this empty. 
            // It just exists to stop the error in usProfile.Designer.cs
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.OpenAddTransaction(ParentForm, null);
        }
    }
}