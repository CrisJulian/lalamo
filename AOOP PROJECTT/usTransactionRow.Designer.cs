namespace AOOP_PROJECTT
{
    partial class usTransactionRow
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
            lblDate = new Label();
            lblDescription = new Label();
            lblCategory = new Label();
            lblType = new Label();
            lblAmount = new Label();
            SuspendLayout();
            // 
            // lblDate
            // 
            lblDate.ForeColor = Color.White;
            lblDate.ImageAlign = ContentAlignment.MiddleRight;
            lblDate.Location = new Point(30, 14);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(80, 25);
            lblDate.TabIndex = 0;
            lblDate.Text = "Apr 22";
            lblDate.Click += lblDate_Click;
            // 
            // lblDescription
            // 
            lblDescription.ForeColor = Color.White;
            lblDescription.ImageAlign = ContentAlignment.MiddleRight;
            lblDescription.Location = new Point(105, 14);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(250, 25);
            lblDescription.TabIndex = 1;
            lblDescription.Text = "Puregold Supermarket";
            lblDescription.Click += lblDescription_Click;
            // 
            // lblCategory
            // 
            lblCategory.ForeColor = Color.White;
            lblCategory.ImageAlign = ContentAlignment.MiddleRight;
            lblCategory.Location = new Point(476, 9);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(115, 25);
            lblCategory.TabIndex = 2;
            lblCategory.Text = "FOOD";
            lblCategory.TextAlign = ContentAlignment.MiddleCenter;
            lblCategory.Click += lblCategory_Click;
            // 
            // lblType
            // 
            lblType.ForeColor = Color.White;
            lblType.ImageAlign = ContentAlignment.MiddleRight;
            lblType.Location = new Point(640, 9);
            lblType.Name = "lblType";
            lblType.Size = new Size(80, 25);
            lblType.TabIndex = 3;
            lblType.Text = "EXPENSE";
            lblType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAmount
            // 
            lblAmount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAmount.ForeColor = Color.White;
            lblAmount.ImageAlign = ContentAlignment.MiddleRight;
            lblAmount.Location = new Point(789, 14);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(98, 25);
            lblAmount.TabIndex = 4;
            lblAmount.Text = "₱1,850.00";
            lblAmount.Click += lblAmount_Click;
            // 
            // usTransactionRow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 38);
            Controls.Add(lblAmount);
            Controls.Add(lblType);
            Controls.Add(lblCategory);
            Controls.Add(lblDescription);
            Controls.Add(lblDate);
            Name = "usTransactionRow";
            Size = new Size(878, 44);
            Load += usTransactionRow_Load;
            ResumeLayout(false);
        }

        #endregion

        public Label lblDate;
        public Label lblDescription;
        public Label lblCategory;
        public Label lblType;
        public Label lblAmount;
    }
}
