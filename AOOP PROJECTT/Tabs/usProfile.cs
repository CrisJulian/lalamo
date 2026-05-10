using Microsoft.Data.SqlClient;
using AOOP_PROJECTT.SupportClasses;

namespace AOOP_PROJECTT
{
    public partial class usProfile : UserControl
    {
        public usProfile()
        {
            InitializeComponent();

            lblInfoPhone = new TextBox();
            lblInfoLocation = new Label();
            AddRoundedBorders();
            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM dd, yyyy");
            myDateTextBox.ReadOnly = true;

            lblUserFullName.AutoSize = false;
            lblUserFullName.Dock = DockStyle.Fill;
            lblUserFullName.TextAlign = ContentAlignment.MiddleCenter;
        }

        // --- Existing UI Event Handlers ---
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

                    // ── 1. Personal Information ──────────────────────────────
                    string userQuery = @"
                SELECT FullName, Email, Phone, Location, CreatedAt
                FROM Users
                WHERE UserId = @id";

                    using (SqlCommand cmd = new SqlCommand(userQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", SessionManager.UserId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Header name
                                lblUserFullName.Text = reader["FullName"].ToString();

                                // Full Name
                                label1.Text = reader["FullName"].ToString();

                                // Phone — optional, show suggestion if empty
                                string phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString().Trim() : "";
                                textBox1.Text = string.IsNullOrEmpty(phone) ? "e.g. +63 912 345 6789" : phone;
                                textBox1.ForeColor = string.IsNullOrEmpty(phone) ? Color.FromArgb(80, 95, 120) : Color.White;

                                // Member Since
                                if (DateTime.TryParse(reader["CreatedAt"].ToString(), out DateTime joinDate))
                                    label3.Text = joinDate.ToString("MMMM dd, yyyy");
                                else
                                    label3.Text = "N/A";

                                // Email
                                label4.Text = reader["Email"].ToString();

                                // Location — optional, show suggestion if empty
                                // Location — textBox2 instead of label6
                                string loc = reader["Location"] != DBNull.Value ? reader["Location"].ToString().Trim() : "";
                                textBox2.Text = string.IsNullOrEmpty(loc) ? "e.g. Manila, Philippines" : loc;
                                textBox2.ForeColor = string.IsNullOrEmpty(loc) ? Color.FromArgb(80, 95, 120) : Color.White;
                            }
                            else
                            {
                                // Fallback to session data if no DB row found
                                lblUserFullName.Text = SessionManager.FullName;
                                label1.Text = SessionManager.FullName;
                                textBox1.Text = "e.g. +63 912 345 6789";
                                textBox1.ForeColor = Color.FromArgb(80, 95, 120);
                                label3.Text = SessionManager.MemberSince.ToString("MMMM dd, yyyy");
                                label4.Text = SessionManager.Email;
                                textBox2.Text = "e.g. Manila, Philippines";
                                textBox2.ForeColor = Color.FromArgb(80, 95, 120);
                            }
                        }
                    }

                    // ── 2. Transaction & Budget Stats ────────────────────────
                    string statsQuery = @"
                SELECT
                    (SELECT COUNT(*) FROM Transactions WHERE UserId = @id) AS TotalTrans,
                    (SELECT COUNT(*) FROM Budgets     WHERE UserId = @id) AS TotalBudgets";

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

                    // ── 3. Savings Rate ──────────────────────────────────────
                    string savingsQuery = @"
                SELECT TotalIncome, TotalExpenses
                FROM vw_UserBalance
                WHERE UserId = @id";

                    using (SqlCommand cmdSavings = new SqlCommand(savingsQuery, conn))
                    {
                        cmdSavings.Parameters.AddWithValue("@id", SessionManager.UserId);
                        using (SqlDataReader saveReader = cmdSavings.ExecuteReader())
                        {
                            if (saveReader.Read())
                            {
                                decimal income = saveReader["TotalIncome"] != DBNull.Value
                                                   ? Convert.ToDecimal(saveReader["TotalIncome"]) : 0;
                                decimal expenses = saveReader["TotalExpenses"] != DBNull.Value
                                                   ? Convert.ToDecimal(saveReader["TotalExpenses"]) : 0;

                                if (income > 0)
                                {
                                    decimal rate = ((income - expenses) / income) * 100;
                                    rate = Math.Max(0, Math.Min(100, rate));
                                    lblSavingsRate.Text = $"{(int)rate}%";
                                }
                                else
                                {
                                    lblSavingsRate.Text = "0%";
                                }
                            }
                            else
                            {
                                lblSavingsRate.Text = "N/A";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message, "Profile Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddRoundedBorders()
        {
            UIHelper.ApplyRoundedStyle(panel4);
            UIHelper.ApplyRoundedStyle(darkpanel);
            UIHelper.ApplyRoundedStyle(panel6);
            UIHelper.ApplyRoundedStyle(panel7);
            UIHelper.ApplyRoundedStyle(panel8);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
                LoadProfileData();
        }

        private void usProfile_VisibleChanged(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.OpenAddTransaction(ParentForm, null);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(50, 60, 80), 1f);
            e.Graphics.DrawLine(pen, 0, panel1.Height - 1, panel1.Width, panel1.Height - 1);
        }

        private void darkpanel_Paint(object sender, PaintEventArgs e) { }
        private void panel6_Paint(object sender, PaintEventArgs e) { }
        private void panel7_Paint(object sender, PaintEventArgs e) { }
        private void panel8_Paint(object sender, PaintEventArgs e) { }
        private void lblUserFullName_Click(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void lblSavingsRate_Click(object sender, EventArgs e)
        {

        }
    }
}