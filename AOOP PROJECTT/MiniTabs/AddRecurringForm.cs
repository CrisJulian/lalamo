namespace AOOP_PROJECTT
{
    public partial class AddRecurringForm : Form

    {
        public string FinalName => textBox1.Text;
        public string FinalAmount => textBox2.Text;
        public string FinalFrequency => comboBox1.Text;
        public string FinalCategory => comboBox2.Text;
        public DateTime FinalDate => dateTimePicker1.Value;

        public AddRecurringForm()
        {
            InitializeComponent();
            textBox1.Text = "e.g. Netflix";
            textBox1.ForeColor = Color.Gray;

            textBox2.Text = "0.00";
            textBox2.ForeColor = Color.Gray;
            this.ActiveControl = label1;
        }




        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "e.g. Netflix")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.White; 
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "e.g. Netflix";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "0.00")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.White; 
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "0.00";
                textBox2.ForeColor = Color.Gray; 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "e.g. Netflix" || string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter a valid name for the recurring item.");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
