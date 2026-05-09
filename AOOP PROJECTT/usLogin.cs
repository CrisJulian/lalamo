using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace AOOP_PROJECTT
{
    public partial class usLogin : UserControl
    {
        // Tells the parent form that login was successful
        public event EventHandler LoginSuccessful;
        public event EventHandler NavigateToSignin;

        public usLogin()
        {
            InitializeComponent();

            // Hide password characters
            textBox15.PasswordChar = '●';
            button3.BringToFront();

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.VisibleChanged += (s, e) => { if (this.Visible) textBox14.Focus(); };

        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox6_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox7_TextChanged(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox14.Text.Trim();
            string password = textBox15.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your email and password.", "Validation");
                return;
            }

            if (ValidateLogin(email, password))
            {
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Invalid email or password.", "Login Failed");
            }
        }

        private bool ValidateLogin(string email, string password)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT UserId, FullName, Email,
                       ISNULL(Phone, '')            AS Phone,
                       ISNULL(Location, '')         AS Location,
                       ISNULL(AccountType,'PERSONAL') AS AccountType,
                       CreatedAt
                FROM Users
                WHERE Email = @email
                  AND PasswordHash = @password";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                SessionManager.UserId = reader.GetInt32(0);
                                SessionManager.FullName = reader.GetString(1);
                                SessionManager.Email = reader.GetString(2);
                                SessionManager.Phone = reader.GetString(3);
                                SessionManager.Location = reader.GetString(4);
                                SessionManager.AccountType = reader.GetString(5);
                                SessionManager.MemberSince = reader.GetDateTime(6);
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error");
            }
            return false;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            NavigateToSignin?.Invoke(this, EventArgs.Empty);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void usLogin_Load(object sender, EventArgs e)
        {
            panelDemo.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.Gray, 1f);
                e.Graphics.DrawRectangle(pen, 0, 0, panelDemo.Width - 1, panelDemo.Height - 1);
            };
            panelDemo.BackColor = Color.Transparent;
        }

        private void panelDemo_Paint(object sender, PaintEventArgs e)
        {
            panelDemo.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.Gray, 1f);
                e.Graphics.DrawRectangle(pen, 0, 0, panelDemo.Width - 1, panelDemo.Height - 1);
            };
            panelDemo.BackColor = Color.Transparent;
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click_1(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SessionManager.UserId = 99;
            SessionManager.FullName = "Demo User";
            SessionManager.Email = "demo@example.com";
            SessionManager.AccountType = "PERSONAL";
            SessionManager.Phone = "";
            SessionManager.Location = "";
            SessionManager.MemberSince = DateTime.Now;

            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }
    }
}