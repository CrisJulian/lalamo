namespace AOOP_PROJECTT
{
    partial class usRecurringRow
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
            lblNextDate = new TextBox();
            panelDot = new Panel();
            lblDueStatus = new Label();
            lblName = new Label();
            lblAmount = new Label();
            lblFrequency = new Label();
            lblCategory = new Label();
            btnDelete = new Button();
            SuspendLayout();
            // 
            // lblNextDate
            // 
            lblNextDate.BackColor = Color.FromArgb(24, 28, 38);
            lblNextDate.BorderStyle = BorderStyle.None;
            lblNextDate.Font = new Font("Microsoft Sans Serif", 9F);
            lblNextDate.ForeColor = Color.FromArgb(130, 145, 170);
            lblNextDate.Location = new Point(598, 8);
            lblNextDate.Name = "lblNextDate";
            lblNextDate.Size = new Size(77, 14);
            lblNextDate.TabIndex = 16;
            lblNextDate.Text = "00/00/0000";
            // 
            // panelDot
            // 
            panelDot.BackColor = Color.White;
            panelDot.Location = new Point(14, 19);
            panelDot.Name = "panelDot";
            panelDot.Size = new Size(10, 10);
            panelDot.TabIndex = 19;
            // 
            // lblDueStatus
            // 
            lblDueStatus.AutoSize = true;
            lblDueStatus.ForeColor = Color.White;
            lblDueStatus.Location = new Point(599, 26);
            lblDueStatus.Name = "lblDueStatus";
            lblDueStatus.Size = new Size(38, 15);
            lblDueStatus.TabIndex = 20;
            lblDueStatus.Text = "label1";
            // 
            // lblName
            // 
            lblName.Font = new Font("Microsoft Sans Serif", 10.5F);
            lblName.ForeColor = Color.FromArgb(130, 145, 170);
            lblName.Location = new Point(28, 14);
            lblName.Name = "lblName";
            lblName.Size = new Size(90, 23);
            lblName.TabIndex = 21;
            lblName.Text = "label1";
            // 
            // lblAmount
            // 
            lblAmount.Font = new Font("Microsoft Sans Serif", 10F);
            lblAmount.ForeColor = Color.FromArgb(255, 107, 107);
            lblAmount.Location = new Point(372, 15);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(90, 27);
            lblAmount.TabIndex = 22;
            lblAmount.Text = "₱0.00";
            // 
            // lblFrequency
            // 
            lblFrequency.Font = new Font("Microsoft Sans Serif", 10F);
            lblFrequency.ForeColor = Color.FromArgb(130, 145, 170);
            lblFrequency.Location = new Point(489, 15);
            lblFrequency.Name = "lblFrequency";
            lblFrequency.Size = new Size(90, 23);
            lblFrequency.TabIndex = 23;
            lblFrequency.Text = "---";
            // 
            // lblCategory
            // 
            lblCategory.Font = new Font("Microsoft Sans Serif", 9F);
            lblCategory.ForeColor = Color.FromArgb(130, 145, 170);
            lblCategory.Location = new Point(726, 14);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(90, 23);
            lblCategory.TabIndex = 24;
            lblCategory.Text = "---";
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.BackColor = Color.Red;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 8F);
            btnDelete.ForeColor = SystemColors.ActiveCaptionText;
            btnDelete.Location = new Point(866, 15);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(0, 0, 0, 2);
            btnDelete.Size = new Size(22, 22);
            btnDelete.TabIndex = 25;
            btnDelete.Text = "X";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click_1;
            // 
            // usRecurringRow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 38);
            Controls.Add(btnDelete);
            Controls.Add(lblCategory);
            Controls.Add(lblFrequency);
            Controls.Add(lblAmount);
            Controls.Add(lblName);
            Controls.Add(lblDueStatus);
            Controls.Add(panelDot);
            Controls.Add(lblNextDate);
            Margin = new Padding(0);
            MaximumSize = new Size(0, 50);
            Name = "usRecurringRow";
            Size = new Size(900, 50);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public TextBox lblNextDate;
        private Button btnDelete;
        private Panel panelDot;
        private Label lblDueStatus;
        public Label lblName;
        public Label lblAmount;
        public Label lblFrequency;
        public Label lblCategory;
    }
}