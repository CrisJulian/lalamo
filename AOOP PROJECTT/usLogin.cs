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
                    string query = "SELECT COUNT(*) FROM Users WHERE Email = @email AND PasswordHash = @password";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);

                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error");
                return false;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            NavigateToSignin?.Invoke(this, EventArgs.Empty);
        }
    }
}