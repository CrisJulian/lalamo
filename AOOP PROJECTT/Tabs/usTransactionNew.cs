using AOOP_PROJECTT.Repositories;
using AOOP_PROJECTT.SupportClasses;


namespace AOOP_PROJECTT
{
    public partial class usTransactionNew : UserControl
    {
        private string activeTypeFilter = "All";

        public usTransactionNew()
        {
            InitializeComponent();
            txtSearch.PlaceholderText = "Search transactions...";
            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM d, yyyy"); 
            myDateTextBox.ReadOnly = true;
            UIHelper.ApplyRoundedStyle(panelSearch);
            UIHelper.ApplyRoundedStyle(panel2);

            foreach (Button btn in new[] { btnAll, btnIncome, btnExpense })
            {
                btn.Region = Region.FromHrgn(
                    UIHelper.CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 6, 6));
            }

            flowLayoutPanel1.Region = Region.FromHrgn(
                UIHelper.CreateRoundRectRgn(0, 0, flowLayoutPanel1.Width, flowLayoutPanel1.Height, 10, 10));

            flowLayoutPanel1.Resize += (s, e) => flowLayoutPanel1.Region = Region.FromHrgn(
                UIHelper.CreateRoundRectRgn(0, 0, flowLayoutPanel1.Width, flowLayoutPanel1.Height, 10, 10));

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

        private void LoadTransactionsFromDb()
        {
            flowLayoutPanel1.Controls.Clear();
            var transactions = TransactionRepository.GetTransactions(SessionManager.UserId);
            foreach (var tx in transactions)
            {
                usTransactionRow row = new usTransactionRow();
                row.SetTransactionData(
                    tx.TransactionId,
                    tx.Date.ToString("MMM dd"),
                    tx.Description,
                    tx.Category,
                    tx.Type,
                    tx.Amount.ToString()
                );
                flowLayoutPanel1.Controls.Add(row);
            }
            UpdateResultCount();
        }

        public void AddRowToList(string date, string desc, string cat, string type, string amount)
        {
            usTransactionRow row = new usTransactionRow();
            row.SetTransactionData(0, date, desc, cat, type, amount);
            flowLayoutPanel1.Controls.Add(row);
            flowLayoutPanel1.Controls.SetChildIndex(row, 0); // Keeps newest at the top
            UpdateResultCount();
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
            Form1.OpenAddTransaction(ParentForm, LoadTransactionsFromDb);
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
            LoadTransactionsFromDb();
        }
        private void textBoxSearch_TextChanged(object sender, EventArgs e) { FilterTransactions(); }
        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e) { FilterTransactions(); }

        private void myDateTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(50, 60, 80), 1f);
            e.Graphics.DrawLine(pen, 0, panel7.Height - 1,
                                panel7.Width, panel7.Height - 1);
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}