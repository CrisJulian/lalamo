namespace AOOP_PROJECTT
{
    partial class usRecurring_Payment
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
            panel1 = new Panel();
            button1 = new Button();
            myDateTextBox = new TextBox();
            textBox1 = new TextBox();
            button2 = new Button();
            lblMonthlyTotal = new TextBox();
            textBox2 = new TextBox();
            panel3 = new Panel();
            lblFrequency = new Label();
            lblCategory = new Label();
            lblNextDate = new Label();
            lblName = new Label();
            lblAmount = new Label();
            pnlRecurringList = new FlowLayoutPanel();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(myDateTextBox);
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(0, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(962, 54);
            panel1.TabIndex = 0;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(245, 166, 35);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 10F);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(765, 10);
            button1.Name = "button1";
            button1.Size = new Size(171, 34);
            button1.TabIndex = 3;
            button1.Text = "+ Add Transaction";
            button1.UseVisualStyleBackColor = false;
            // 
            // myDateTextBox
            // 
            myDateTextBox.BackColor = Color.FromArgb(15, 17, 23);
            myDateTextBox.BorderStyle = BorderStyle.None;
            myDateTextBox.Font = new Font("Microsoft Sans Serif", 10F);
            myDateTextBox.ForeColor = Color.FromArgb(130, 145, 170);
            myDateTextBox.Location = new Point(639, 20);
            myDateTextBox.Name = "myDateTextBox";
            myDateTextBox.Size = new Size(297, 16);
            myDateTextBox.TabIndex = 5;
            myDateTextBox.Text = "Date";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(15, 17, 23);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("SimSun", 15F);
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(27, 16);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(129, 23);
            textBox1.TabIndex = 4;
            textBox1.Text = "Recurring";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(245, 166, 35);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 10F);
            button2.ForeColor = Color.Black;
            button2.Location = new Point(782, 100);
            button2.Name = "button2";
            button2.Size = new Size(154, 34);
            button2.TabIndex = 3;
            button2.Text = "+ Add Recurring";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // lblMonthlyTotal
            // 
            lblMonthlyTotal.BackColor = Color.FromArgb(15, 17, 23);
            lblMonthlyTotal.BorderStyle = BorderStyle.None;
            lblMonthlyTotal.Font = new Font("Segoe UI", 18.75F, FontStyle.Bold);
            lblMonthlyTotal.ForeColor = Color.FromArgb(255, 107, 107);
            lblMonthlyTotal.Location = new Point(27, 111);
            lblMonthlyTotal.Margin = new Padding(0);
            lblMonthlyTotal.Name = "lblMonthlyTotal";
            lblMonthlyTotal.Size = new Size(297, 34);
            lblMonthlyTotal.TabIndex = 4;
            lblMonthlyTotal.Text = "₱0.00";
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.FromArgb(15, 17, 23);
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Font = new Font("Microsoft Sans Serif", 10F);
            textBox2.ForeColor = Color.FromArgb(130, 145, 170);
            textBox2.Location = new Point(27, 84);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(297, 16);
            textBox2.TabIndex = 6;
            textBox2.Text = "Monthly Recurring Total";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(24, 28, 38);
            panel3.Controls.Add(lblFrequency);
            panel3.Controls.Add(lblCategory);
            panel3.Controls.Add(lblNextDate);
            panel3.Controls.Add(lblName);
            panel3.Controls.Add(lblAmount);
            panel3.Location = new Point(27, 174);
            panel3.Name = "panel3";
            panel3.Size = new Size(903, 44);
            panel3.TabIndex = 0;
            // 
            // lblFrequency
            // 
            lblFrequency.AutoSize = true;
            lblFrequency.Font = new Font("SimSun", 12F);
            lblFrequency.ForeColor = Color.FromArgb(130, 145, 170);
            lblFrequency.Location = new Point(480, 14);
            lblFrequency.Margin = new Padding(0);
            lblFrequency.Name = "lblFrequency";
            lblFrequency.Size = new Size(79, 16);
            lblFrequency.TabIndex = 8;
            lblFrequency.Text = "Frequency";
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Font = new Font("SimSun", 12F);
            lblCategory.ForeColor = Color.FromArgb(130, 145, 170);
            lblCategory.Location = new Point(709, 14);
            lblCategory.Margin = new Padding(0);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(71, 16);
            lblCategory.TabIndex = 7;
            lblCategory.Text = "Category";
            // 
            // lblNextDate
            // 
            lblNextDate.AutoSize = true;
            lblNextDate.Font = new Font("SimSun", 12F);
            lblNextDate.ForeColor = Color.FromArgb(130, 145, 170);
            lblNextDate.Location = new Point(597, 14);
            lblNextDate.Margin = new Padding(0);
            lblNextDate.Name = "lblNextDate";
            lblNextDate.Size = new Size(79, 16);
            lblNextDate.TabIndex = 6;
            lblNextDate.Text = "Next Date";
            lblNextDate.Click += label3_Click;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("SimSun", 12F);
            lblName.ForeColor = Color.FromArgb(130, 145, 170);
            lblName.Location = new Point(20, 14);
            lblName.Margin = new Padding(0);
            lblName.Name = "lblName";
            lblName.Size = new Size(47, 16);
            lblName.TabIndex = 4;
            lblName.Text = "Name ";
            // 
            // lblAmount
            // 
            lblAmount.AutoSize = true;
            lblAmount.Font = new Font("SimSun", 12F);
            lblAmount.ForeColor = Color.FromArgb(130, 145, 170);
            lblAmount.Location = new Point(378, 14);
            lblAmount.Margin = new Padding(0);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(55, 16);
            lblAmount.TabIndex = 5;
            lblAmount.Text = "Amount";
            // 
            // pnlRecurringList
            // 
            pnlRecurringList.AutoScroll = true;
            pnlRecurringList.BackColor = Color.FromArgb(24, 28, 38);
            pnlRecurringList.FlowDirection = FlowDirection.TopDown;
            pnlRecurringList.Location = new Point(27, 224);
            pnlRecurringList.Margin = new Padding(0);
            pnlRecurringList.Name = "pnlRecurringList";
            pnlRecurringList.Size = new Size(903, 396);
            pnlRecurringList.TabIndex = 7;
            pnlRecurringList.WrapContents = false;
            // 
            // usRecurring_Payment
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 17, 23);
            Controls.Add(pnlRecurringList);
            Controls.Add(panel3);
            Controls.Add(textBox2);
            Controls.Add(button2);
            Controls.Add(lblMonthlyTotal);
            Controls.Add(panel1);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Name = "usRecurring_Payment";
            Size = new Size(963, 649);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button button1;
        private TextBox myDateTextBox;
        private TextBox textBox1;
        private Button button2;
        private TextBox lblMonthlyTotal;
        private TextBox textBox2;
        private Panel panel3;
        private FlowLayoutPanel pnlRecurringList;
        private Label lblCategory;
        private Label lblNextDate;
        private Label lblAmount;
        private Label lblName;
        private Label lblFrequency;
    }
}
