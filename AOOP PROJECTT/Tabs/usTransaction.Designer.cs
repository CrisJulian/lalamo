namespace AOOP_PROJECTT
{
    partial class usTransaction
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            panelSearch = new Panel();
            lblResultCount = new Label();
            btnIncome = new Button();
            btnExpense = new Button();
            btnAll = new Button();
            cmbFilterType = new ComboBox();
            txtSearch = new TextBox();
            headerDate = new Label();
            panel2 = new Panel();
            headerAmt = new Label();
            headerType = new Label();
            headerCat = new Label();
            headerDesc = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            textBox1 = new TextBox();
            button4 = new Button();
            myDateTextBox = new TextBox();
            panel7 = new Panel();
            panelSearch.SuspendLayout();
            panel2.SuspendLayout();
            panel7.SuspendLayout();
            SuspendLayout();
            // 
            // panelSearch
            // 
            panelSearch.BackColor = Color.FromArgb(24, 28, 38);
            panelSearch.Controls.Add(lblResultCount);
            panelSearch.Controls.Add(btnIncome);
            panelSearch.Controls.Add(btnExpense);
            panelSearch.Controls.Add(btnAll);
            panelSearch.Controls.Add(cmbFilterType);
            panelSearch.Controls.Add(txtSearch);
            panelSearch.Location = new Point(30, 102);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(903, 49);
            panelSearch.TabIndex = 8;
            // 
            // lblResultCount
            // 
            lblResultCount.ForeColor = Color.White;
            lblResultCount.Location = new Point(836, 15);
            lblResultCount.Name = "lblResultCount";
            lblResultCount.Size = new Size(64, 23);
            lblResultCount.TabIndex = 5;
            lblResultCount.Text = "20 results";
            lblResultCount.Click += lblResultsCount_Click;
            // 
            // btnIncome
            // 
            btnIncome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIncome.FlatAppearance.BorderColor = Color.FromArgb(45, 48, 58);
            btnIncome.FlatStyle = FlatStyle.Flat;
            btnIncome.ForeColor = Color.White;
            btnIncome.Location = new Point(705, 12);
            btnIncome.Name = "btnIncome";
            btnIncome.Size = new Size(63, 24);
            btnIncome.TabIndex = 4;
            btnIncome.Text = "Expenses";
            btnIncome.UseVisualStyleBackColor = true;
            btnIncome.Click += btnExpense_Click;
            // 
            // btnExpense
            // 
            btnExpense.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExpense.FlatAppearance.BorderColor = Color.FromArgb(45, 48, 58);
            btnExpense.FlatStyle = FlatStyle.Flat;
            btnExpense.ForeColor = Color.White;
            btnExpense.Location = new Point(762, 12);
            btnExpense.Name = "btnExpense";
            btnExpense.Size = new Size(68, 24);
            btnExpense.TabIndex = 3;
            btnExpense.Text = "Income";
            btnExpense.UseVisualStyleBackColor = true;
            btnExpense.Click += btnIncome_Click;
            // 
            // btnAll
            // 
            btnAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAll.FlatAppearance.BorderColor = Color.FromArgb(45, 48, 58);
            btnAll.FlatStyle = FlatStyle.Flat;
            btnAll.ForeColor = Color.White;
            btnAll.Location = new Point(659, 12);
            btnAll.Name = "btnAll";
            btnAll.Size = new Size(46, 24);
            btnAll.TabIndex = 2;
            btnAll.Text = "All";
            btnAll.UseVisualStyleBackColor = true;
            btnAll.Click += btnAll_Click;
            // 
            // cmbFilterType
            // 
            cmbFilterType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbFilterType.Font = new Font("Segoe UI", 9F);
            cmbFilterType.FormattingEnabled = true;
            cmbFilterType.Items.AddRange(new object[] { "All", "Food", "Transport", "Entertainment", "Utilities", "Healthcare", "Shopping", "Health", "Software", "Income", "Other" });
            cmbFilterType.Location = new Point(535, 13);
            cmbFilterType.Name = "cmbFilterType";
            cmbFilterType.Size = new Size(121, 23);
            cmbFilterType.TabIndex = 1;
            cmbFilterType.Text = "All";
            cmbFilterType.SelectedIndexChanged += cmbFilterType_SelectedIndexChanged;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.BackColor = Color.FromArgb(40, 44, 52);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.ForeColor = Color.White;
            txtSearch.Location = new Point(20, 13);
            txtSearch.Multiline = true;
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(509, 23);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // headerDate
            // 
            headerDate.ForeColor = Color.White;
            headerDate.Location = new Point(34, 9);
            headerDate.Name = "headerDate";
            headerDate.Size = new Size(55, 18);
            headerDate.TabIndex = 0;
            headerDate.Text = "DATE";
            headerDate.Click += label1_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(24, 28, 38);
            panel2.Controls.Add(headerAmt);
            panel2.Controls.Add(headerType);
            panel2.Controls.Add(headerCat);
            panel2.Controls.Add(headerDesc);
            panel2.Controls.Add(headerDate);
            panel2.Location = new Point(30, 165);
            panel2.Name = "panel2";
            panel2.Size = new Size(903, 39);
            panel2.TabIndex = 10;
            // 
            // headerAmt
            // 
            headerAmt.ForeColor = Color.White;
            headerAmt.Location = new Point(670, 9);
            headerAmt.Name = "headerAmt";
            headerAmt.Size = new Size(62, 18);
            headerAmt.TabIndex = 4;
            headerAmt.Text = "AMOUNT";
            // 
            // headerType
            // 
            headerType.ForeColor = Color.White;
            headerType.Location = new Point(555, 9);
            headerType.Name = "headerType";
            headerType.Size = new Size(55, 18);
            headerType.TabIndex = 3;
            headerType.Text = "TYPE";
            // 
            // headerCat
            // 
            headerCat.ForeColor = Color.White;
            headerCat.Location = new Point(414, 9);
            headerCat.Name = "headerCat";
            headerCat.Size = new Size(83, 18);
            headerCat.TabIndex = 2;
            headerCat.Text = "CATEGORY";
            headerCat.Click += headerCat_Click;
            // 
            // headerDesc
            // 
            headerDesc.ForeColor = Color.White;
            headerDesc.Location = new Point(107, 9);
            headerDesc.Name = "headerDesc";
            headerDesc.Size = new Size(107, 18);
            headerDesc.TabIndex = 1;
            headerDesc.Text = "DESCRIPTION";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.FromArgb(24, 28, 38);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Font = new Font("Segoe UI", 10F);
            flowLayoutPanel1.ForeColor = Color.White;
            flowLayoutPanel1.Location = new Point(30, 206);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(903, 418);
            flowLayoutPanel1.TabIndex = 11;
            flowLayoutPanel1.WrapContents = false;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(15, 17, 23);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("SimSun", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(30, 16);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(129, 23);
            textBox1.TabIndex = 7;
            textBox1.Text = "Transaction";
            textBox1.TextChanged += textBox1_TextChanged_1;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(245, 166, 35);
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.ForeColor = Color.Black;
            button4.Location = new Point(762, 10);
            button4.Name = "button4";
            button4.Size = new Size(171, 34);
            button4.TabIndex = 6;
            button4.Text = "+ Add Transaction";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // myDateTextBox
            // 
            myDateTextBox.BackColor = Color.FromArgb(15, 17, 23);
            myDateTextBox.BorderStyle = BorderStyle.None;
            myDateTextBox.Font = new Font("Microsoft Sans Serif", 10F);
            myDateTextBox.ForeColor = Color.FromArgb(130, 145, 170);
            myDateTextBox.Location = new Point(640, 20);
            myDateTextBox.Name = "myDateTextBox";
            myDateTextBox.Size = new Size(112, 16);
            myDateTextBox.TabIndex = 8;
            myDateTextBox.Text = "Date";
            myDateTextBox.TextChanged += myDateTextBox_TextChanged;
            // 
            // panel7
            // 
            panel7.Controls.Add(myDateTextBox);
            panel7.Controls.Add(button4);
            panel7.Controls.Add(textBox1);
            panel7.Location = new Point(0, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(966, 54);
            panel7.TabIndex = 12;
            panel7.Paint += panel7_Paint;
            // 
            // usTransactionNew
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(15, 17, 23);
            Controls.Add(panel7);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel2);
            Controls.Add(panelSearch);
            Name = "usTransactionNew";
            Size = new Size(969, 649);
            Load += usTransactionNew_Load;
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            panel2.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panelSearch;
        private TextBox txtSearch;
        private Button btnIncome;
        private Button btnExpense;
        private Button btnAll;
        private ComboBox cmbFilterType;
        private Panel panel2;
        private FlowLayoutPanel flowLayoutPanel1;
        public Label headerType;
        public Label headerDesc;
        public Label lblResultCount;
        public Label headerDate;
        public Label headerAmt;
        public Label headerCat;
        private TextBox textBox1;
        private Button button4;
        private TextBox myDateTextBox;
        private Panel panel7;
    }
}