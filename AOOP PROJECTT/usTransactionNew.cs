using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class usTransactionNew : UserControl
    {
        private string activeTypeFilter = "All";

        public usTransactionNew()
        {
            InitializeComponent();
            txtSearch.PlaceholderText = "Search transactions...";
        }

        private void UpdateResultCount()
        {
            int count = 0;
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                if (ctrl is usTransactionRow row && row.Visible) count++;
            }
            lblResultCount.Text = $"{count} results";
        }

        public void AddRowToList(string date, string desc, string cat, string type, string amount)
        {
            usTransactionRow row = new usTransactionRow();
            row.SetTransactionData(date, desc, cat, type, amount);
            flowLayoutPanel1.Controls.Add(row);
            flowLayoutPanel1.Controls.SetChildIndex(row, 0); // Keeps newest at the top
            UpdateResultCount();
        }

        private void LoadFromDatabase()
        {
            flowLayoutPanel1.Controls.Clear();
            var transactions = TransactionRepository.GetTransactions(SessionManager.UserId);
            foreach (var tx in transactions)
            {
                AddRowToList(
                    tx.Date.ToString("MMM dd"),
                    tx.Description,
                    tx.Type == "Income" ? "Income" : "Expense",
                    tx.Type,
                    tx.Amount.ToString()
                );
            }
        }

        // --- UPDATED MASTER FILTER ---
        private void FilterTransactions()
        {
            string search = txtSearch.Text.ToLower().Trim();
            if (search == "search transactions...") search = "";

            // ComboBox now handles CATEGORY (Food, etc.)
            string categoryFilter = cmbFilterType.SelectedItem?.ToString() ?? "All";

            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                if (ctrl is usTransactionRow row)
                {
                    // 1. Check Search
                    bool matchesSearch = row.lblDescription.Text.ToLower().Contains(search);

                    // 2. Check Category (Dropdown)
                    bool matchesCategory = (categoryFilter == "All") ||
                                           (row.lblCategory.Text.Equals(categoryFilter, StringComparison.OrdinalIgnoreCase));

                    // 3. Check Type (Buttons: All/Income/Expense)
                    bool matchesType = (activeTypeFilter == "All") ||
                                       (row.lblType.Text.Equals(activeTypeFilter, StringComparison.OrdinalIgnoreCase));

                    // Show if all conditions are met
                    row.Visible = matchesSearch && matchesCategory && matchesType;
                }
            }
            UpdateResultCount();
        }

        // --- EXISTING LOGIC CONNECTED TO CONTROLS ---
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterTransactions();
        }

        private void cmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterTransactions();
        }

        private void btnSearchIcon_Click(object sender, EventArgs e)
        {
            FilterTransactions();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using var modal = new AddTransactionForm();
            if (modal.ShowDialog() == DialogResult.OK)
            {
                var tx = new TransactionRecord
                {
                    Amount = double.TryParse(modal.TAmount, out var v) ? v : 0,
                    Description = modal.TDescription,
                    Date = DateTime.Now,
                    Type = modal.TType
                };

                int newId = TransactionRepository.AddTransaction(SessionManager.UserId, tx);
                if (newId > 0)
                {
                    AddRowToList(modal.TDate, modal.TDescription,
                        modal.TCategory, modal.TType, modal.TAmount);
                }
            }
        }

        // --- NEW BUTTON CLICK METHODS (Link these in the Designer) ---
        public void btnAll_Click(object sender, EventArgs e)
        {
            activeTypeFilter = "All";
            FilterTransactions();
        }

        public void btnIncome_Click(object sender, EventArgs e)
        {
            activeTypeFilter = "Income";
            FilterTransactions();
        }

        public void btnExpense_Click(object sender, EventArgs e)
        {
            activeTypeFilter = "Expense";
            FilterTransactions();
        }

        // --- KEEPING THESE TO PREVENT DESIGNER ERRORS ---
        private void button2_Click(object sender, EventArgs e) { }
        private void lblResultsCount_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void headerCat_Click(object sender, EventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void usTransactionNew_Load(object sender, EventArgs e) 
        {
            LoadFromDatabase();
        }
        private void textBoxSearch_TextChanged(object sender, EventArgs e) { FilterTransactions(); }
        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e) { FilterTransactions(); }
    }
}