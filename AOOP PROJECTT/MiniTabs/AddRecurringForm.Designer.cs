namespace AOOP_PROJECTT
{
    partial class AddRecurringForm
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
            label2 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label3 = new Label();
            comboBox1 = new ComboBox();
            label4 = new Label();
            label5 = new Label();
            comboBox2 = new ComboBox();
            label6 = new Label();
            dateTimePicker1 = new DateTimePicker();
            button2 = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("SimSun", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(17, 23);
            label1.Name = "label1";
            label1.Size = new Size(169, 16);
            label1.TabIndex = 0;
            label1.Text = "Add Recurring Item";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(17, 70);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 1;
            label2.Text = "Name ";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(15, 17, 23);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("SimSun", 10F);
            textBox1.ForeColor = Color.DarkGray;
            textBox1.Location = new Point(17, 97);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(359, 30);
            textBox1.TabIndex = 2;
            textBox1.Text = "\r\n";
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.FromArgb(15, 17, 23);
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Font = new Font("SimSun", 10F);
            textBox2.ForeColor = Color.DarkGray;
            textBox2.Location = new Point(17, 177);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(169, 30);
            textBox2.TabIndex = 4;
            textBox2.Text = "0.00";
            textBox2.Enter += textBox2_Enter;
            textBox2.Leave += textBox2_Leave;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(17, 150);
            label3.Name = "label3";
            label3.Size = new Size(69, 15);
            label3.TabIndex = 3;
            label3.Text = "Amount (₱)";
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.FromArgb(15, 17, 23);
            comboBox1.FlatStyle = FlatStyle.Flat;
            comboBox1.Font = new Font("Segoe UI", 12F);
            comboBox1.ForeColor = Color.DarkGray;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Daily", "Weekly", "Bi-Weekly", "Monthly ", "Yearly" });
            comboBox1.Location = new Point(208, 177);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(168, 29);
            comboBox1.TabIndex = 5;
            comboBox1.Text = "Monthly ";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(208, 150);
            label4.Name = "label4";
            label4.Size = new Size(62, 15);
            label4.TabIndex = 6;
            label4.Text = "Frequency";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.White;
            label5.Location = new Point(208, 235);
            label5.Name = "label5";
            label5.Size = new Size(55, 15);
            label5.TabIndex = 8;
            label5.Text = "Category";
            // 
            // comboBox2
            // 
            comboBox2.BackColor = Color.FromArgb(15, 17, 23);
            comboBox2.FlatStyle = FlatStyle.Flat;
            comboBox2.Font = new Font("Segoe UI", 12F);
            comboBox2.ForeColor = Color.DarkGray;
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "Entertainment", "Utilities", "Subscription", "Insurance", "Health", "Other" });
            comboBox2.Location = new Point(208, 262);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(168, 29);
            comboBox2.TabIndex = 7;
            comboBox2.Text = "Entertainment";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.White;
            label6.Location = new Point(24, 235);
            label6.Name = "label6";
            label6.Size = new Size(58, 15);
            label6.TabIndex = 9;
            label6.Text = "Next Date";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker1.Location = new Point(21, 268);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(165, 23);
            dateTimePicker1.TabIndex = 10;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 165, 30);
            button2.DialogResult = DialogResult.OK;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("SimSun", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(208, 327);
            button2.Name = "button2";
            button2.Size = new Size(168, 35);
            button2.TabIndex = 12;
            button2.Text = "Add";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(28, 32, 40);
            button1.DialogResult = DialogResult.Cancel;
            button1.FlatAppearance.BorderColor = Color.FromArgb(255, 165, 30);
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.FromArgb(255, 165, 30);
            button1.Location = new Point(21, 327);
            button1.Name = "button1";
            button1.Size = new Size(168, 35);
            button1.TabIndex = 13;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = false;
            // 
            // AddRecurringForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(28, 32, 40);
            ClientSize = new Size(404, 391);
            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(dateTimePicker1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(comboBox2);
            Controls.Add(label4);
            Controls.Add(comboBox1);
            Controls.Add(textBox2);
            Controls.Add(label3);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "AddRecurringForm";
            Text = "AddRecurringForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label3;
        private ComboBox comboBox1;
        private Label label4;
        private Label label5;
        private ComboBox comboBox2;
        private Label label6;
        private DateTimePicker dateTimePicker1;
        private Button button2;
        private Button button1;
    }
}