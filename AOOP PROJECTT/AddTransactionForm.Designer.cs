namespace AOOP_PROJECTT
{
    partial class AddTransactionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label3 = new Label();
            txtDescription = new TextBox();
            txtAmount = new TextBox();
            label4 = new Label();
            cmbType = new ComboBox();
            label5 = new Label();
            label6 = new Label();
            cmbCategory = new ComboBox();
            dtpDate = new DateTimePicker();
            label7 = new Label();
            btnCancel = new Button();
            btnSave = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("SimSun", 12F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(18, 29);
            label1.Name = "label1";
            label1.Size = new Size(142, 16);
            label1.TabIndex = 0;
            label1.Text = "Add Transaction";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Gray;
            label3.Location = new Point(17, 75);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 2;
            label3.Text = "Description";
            // 
            // txtDescription
            // 
            txtDescription.BackColor = Color.FromArgb(15, 17, 23);
            txtDescription.BorderStyle = BorderStyle.None;
            txtDescription.Location = new Point(18, 100);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(359, 30);
            txtDescription.TabIndex = 3;
            txtDescription.TextChanged += txtDescription_TextChanged;
            txtDescription.Enter += txtDescription_Enter;
            txtDescription.Leave += txtDescription_Leave;
            // 
            // txtAmount
            // 
            txtAmount.BackColor = Color.FromArgb(15, 17, 23);
            txtAmount.BorderStyle = BorderStyle.None;
            txtAmount.Location = new Point(18, 179);
            txtAmount.Multiline = true;
            txtAmount.Name = "txtAmount";
            txtAmount.Size = new Size(164, 30);
            txtAmount.TabIndex = 5;
            txtAmount.TextChanged += txtAmount_TextChanged;
            txtAmount.Enter += txtAmount_Enter;
            txtAmount.Leave += txtAmount_Leave;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Gray;
            label4.Location = new Point(17, 154);
            label4.Name = "label4";
            label4.Size = new Size(69, 15);
            label4.TabIndex = 4;
            label4.Text = "Amount (₱)";
            // 
            // cmbType
            // 
            cmbType.BackColor = Color.FromArgb(15, 17, 23);
            cmbType.Font = new Font("Segoe UI", 12F);
            cmbType.ForeColor = SystemColors.ActiveBorder;
            cmbType.FormattingEnabled = true;
            cmbType.Items.AddRange(new object[] { "Expense", "Income" });
            cmbType.Location = new Point(17, 265);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(165, 29);
            cmbType.TabIndex = 6;
            cmbType.Text = "Expense";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Gray;
            label5.Location = new Point(17, 242);
            label5.Name = "label5";
            label5.Size = new Size(32, 15);
            label5.TabIndex = 7;
            label5.Text = "Type";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.Gray;
            label6.Location = new Point(212, 242);
            label6.Name = "label6";
            label6.Size = new Size(55, 15);
            label6.TabIndex = 9;
            label6.Text = "Category";
            // 
            // cmbCategory
            // 
            cmbCategory.BackColor = Color.FromArgb(15, 17, 23);
            cmbCategory.Font = new Font("Segoe UI", 12F);
            cmbCategory.ForeColor = SystemColors.ActiveBorder;
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Items.AddRange(new object[] { "Food", "Transport", "Entertainment", "Utilities", "Healthcare", "Shopping", "Health", "Software", "Income", "Other" });
            cmbCategory.Location = new Point(212, 265);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(165, 29);
            cmbCategory.TabIndex = 8;
            cmbCategory.Text = "Food";
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            // 
            // dtpDate
            // 
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Location = new Point(212, 179);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(165, 23);
            dtpDate.TabIndex = 10;
            dtpDate.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.Gray;
            label7.Location = new Point(212, 154);
            label7.Name = "label7";
            label7.Size = new Size(31, 15);
            label7.TabIndex = 11;
            label7.Text = "Date";
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.FromArgb(255, 165, 30);
            btnCancel.Location = new Point(18, 329);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(164, 32);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += button1_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(255, 165, 30);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("SimSun", 11F, FontStyle.Bold);
            btnSave.Location = new Point(212, 329);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(165, 32);
            btnSave.TabIndex = 13;
            btnSave.Text = "Add Transaction";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // AddTransactionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(28, 32, 40);
            ClientSize = new Size(404, 391);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(label7);
            Controls.Add(dtpDate);
            Controls.Add(label6);
            Controls.Add(cmbCategory);
            Controls.Add(label5);
            Controls.Add(cmbType);
            Controls.Add(txtAmount);
            Controls.Add(label4);
            Controls.Add(txtDescription);
            Controls.Add(label3);
            Controls.Add(label1);
            Name = "AddTransactionForm";
            Text = "AddTransactionForm";
            Click += btnSave_Click;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label3;
        private TextBox txtDescription;
        private TextBox txtAmount;
        private Label label4;
        private ComboBox cmbType;
        private Label label5;
        private Label label6;
        private ComboBox cmbCategory;
        private DateTimePicker dtpDate;
        private Label label7;
        private Button btnCancel;
        private Button btnSave;
    }
}