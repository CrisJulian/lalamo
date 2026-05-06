namespace AOOP_PROJECTT
{
    partial class usRecurringRow
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblName = new TextBox();
            lblAmount = new TextBox();
            lblFrequency = new TextBox();
            lblNextDate = new TextBox();
            lblCategory = new TextBox();
            btnDelete = new Button();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.BackColor = Color.FromArgb(24, 28, 38);
            lblName.BorderStyle = BorderStyle.None;
            lblName.Font = new Font("Microsoft Sans Serif", 10F);
            lblName.ForeColor = Color.FromArgb(130, 145, 170);
            lblName.Location = new Point(21, 14);
            lblName.Name = "lblName";
            lblName.Size = new Size(46, 16);
            lblName.TabIndex = 13;
            lblName.Text = "---";
            // 
            // lblAmount
            // 
            lblAmount.BackColor = Color.FromArgb(24, 28, 38);
            lblAmount.BorderStyle = BorderStyle.None;
            lblAmount.Font = new Font("Microsoft Sans Serif", 10F);
            lblAmount.ForeColor = Color.FromArgb(130, 145, 170);
            lblAmount.Location = new Point(379, 14);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(46, 16);
            lblAmount.TabIndex = 14;
            lblAmount.Text = "₱0.00";
            // 
            // lblFrequency
            // 
            lblFrequency.BackColor = Color.FromArgb(24, 28, 38);
            lblFrequency.BorderStyle = BorderStyle.None;
            lblFrequency.Font = new Font("Microsoft Sans Serif", 10F);
            lblFrequency.ForeColor = Color.FromArgb(130, 145, 170);
            lblFrequency.Location = new Point(481, 14);
            lblFrequency.Name = "lblFrequency";
            lblFrequency.Size = new Size(69, 16);
            lblFrequency.TabIndex = 15;
            lblFrequency.Text = "---";
            // 
            // lblNextDate
            // 
            lblNextDate.BackColor = Color.FromArgb(24, 28, 38);
            lblNextDate.BorderStyle = BorderStyle.None;
            lblNextDate.Font = new Font("Microsoft Sans Serif", 10F);
            lblNextDate.ForeColor = Color.FromArgb(130, 145, 170);
            lblNextDate.Location = new Point(598, 14);
            lblNextDate.Name = "lblNextDate";
            lblNextDate.Size = new Size(77, 16);
            lblNextDate.TabIndex = 16;
            lblNextDate.Text = "00/00/0000";
            // 
            // lblCategory
            // 
            lblCategory.BackColor = Color.FromArgb(24, 28, 38);
            lblCategory.BorderStyle = BorderStyle.None;
            lblCategory.Font = new Font("Microsoft Sans Serif", 10F);
            lblCategory.ForeColor = Color.FromArgb(130, 145, 170);
            lblCategory.Location = new Point(710, 14);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(137, 16);
            lblCategory.TabIndex = 17;
            lblCategory.Text = "---";
            lblCategory.TextChanged += lblCategory_TextChanged;
            // 
            // btnDelete
            // 
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 11F);
            btnDelete.ForeColor = Color.Red;
            btnDelete.Location = new Point(829, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(18, 37);
            btnDelete.TabIndex = 18;
            btnDelete.Text = "X";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click_1;
            // 
            // usRecurringRow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 38);
            Controls.Add(btnDelete);
            Controls.Add(lblCategory);
            Controls.Add(lblNextDate);
            Controls.Add(lblFrequency);
            Controls.Add(lblAmount);
            Controls.Add(lblName);
            Margin = new Padding(0);
            MaximumSize = new Size(850, 40);
            Name = "usRecurringRow";
            Size = new Size(850, 40);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public TextBox lblName;
        public TextBox lblAmount;
        public TextBox lblFrequency;
        public TextBox lblNextDate;
        public TextBox lblCategory;
        private Button btnDelete;
        private TextBox textBox4;
    }
}
