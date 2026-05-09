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
    public partial class usSignin : UserControl
    {
        public event EventHandler AccountCreated;
        public event EventHandler NavigateToLogin;

        public usSignin()
        {
            InitializeComponent();

            button1.Click += button1_Click;
            button2.Click += button2_Click;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AccountCreated?.Invoke(this, EventArgs.Empty);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigateToLogin?.Invoke(this, EventArgs.Empty);
        }

        private void usSignin_Load(object sender, EventArgs e) 
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = Properties.Resources.backgroundremover_removebg_preview;
        }
        private void textBox16_TextChanged(object sender, EventArgs e) { }

        private void txtFullName_Enter(object sender, EventArgs e)
        {
            if (txtFullName.Text == "Juan Dela Cruz")
            {
                txtFullName.Text = "";
                txtFullName.ForeColor = Color.White;
            }
        }

        private void txtFullName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                txtFullName.Text = "Juan Dela Cruz";
                txtFullName.ForeColor = Color.Gray;
            }
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "you@example.com")
            {
                txtEmail.Text = "";
                txtEmail.ForeColor = Color.White;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "you@example.com";
                txtEmail.ForeColor = Color.Gray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "At least 8 characters")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.White;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "At least 8 characters";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void txtConfirmPassword_Enter(object sender, EventArgs e)
        {
            if (txtConfirmPassword.Text == "Re-enter your password")
            {
                txtConfirmPassword.Text = "";
                txtConfirmPassword.ForeColor = Color.White;
                txtConfirmPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtConfirmPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                txtConfirmPassword.Text = "Re-enter your password";
                txtConfirmPassword.ForeColor = Color.Gray;
                txtConfirmPassword.UseSystemPasswordChar = false;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || fullName == "Juan Dela Cruz" ||
                string.IsNullOrEmpty(email) || email == "you@example.com" ||
                string.IsNullOrEmpty(password) || password == "At least 8 characters")
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                using (var con = DatabaseHelper.GetConnection())
                {
                    con.Open();
                    string query = "INSERT INTO Users (FullName, Email, PasswordHash) VALUES (@FullName, @Email, @Password)";

                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Account created successfully!");
                            AccountCreated?.Invoke(this, EventArgs.Empty); // navigate after success
                        }
                        else
                            MessageBox.Show("Something went wrong.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }


    }
}