namespace AOOP_PROJECTT
{
    public partial class AddTransactionForm : Form
    {

        private const string DescPlaceholder = "e.g. Grocery Store";
        private const string AmountPlaceholder = "0.00";

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TDate { get; set; } = string.Empty;
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TDescription { get; set; } = string.Empty;
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TCategory { get; set; } = string.Empty;
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TType { get; set; } = string.Empty;
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string TAmount { get; set; } = string.Empty;

        public AddTransactionForm()
        {
            InitializeComponent();

            txtDescription.Text = DescPlaceholder;
            txtDescription.ForeColor = Color.Gray;

            txtAmount.Text = AmountPlaceholder;
            txtAmount.ForeColor = Color.Gray;
        }

        // --- DESCRIPTION PLACEHOLDER LOGIC ---
        private void txtDescription_Enter(object sender, EventArgs e)
        {
            if (txtDescription.Text == DescPlaceholder)
            {
                txtDescription.Text = "";
                txtDescription.ForeColor = Color.Gray;
            }
        }

        private void txtDescription_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                txtDescription.Text = DescPlaceholder;
                txtDescription.ForeColor = Color.Gray;
            }
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (txtDescription.Text != DescPlaceholder && !string.IsNullOrWhiteSpace(txtDescription.Text))
                txtDescription.ForeColor = Color.White;
            else
                txtDescription.ForeColor = Color.Gray;
        }

        // --- AMOUNT PLACEHOLDER LOGIC ---
        private void txtAmount_Enter(object sender, EventArgs e)
        {
            if (txtAmount.Text == AmountPlaceholder)
            {
                txtAmount.Text = "";
                txtAmount.ForeColor = Color.Gray;
            }
        }

        private void txtAmount_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                txtAmount.Text = AmountPlaceholder;
                txtAmount.ForeColor = Color.Gray;
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtAmount.Text != AmountPlaceholder && !string.IsNullOrWhiteSpace(txtAmount.Text))
                txtAmount.ForeColor = Color.White;
            else
                txtAmount.ForeColor = Color.Gray;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TDate = dtpDate.Value.ToString("MMM dd");

            string d = txtDescription.Text.Trim();
            TDescription = (d == DescPlaceholder) ? "" : d;

            string a = txtAmount.Text.Trim();
            TAmount = (a == AmountPlaceholder) ? "" : a;

            TCategory = cmbCategory.Text;
            TType = cmbType.Text;

            if (string.IsNullOrWhiteSpace(TDescription) || TAmount == "" || TAmount == "0.00")
            {
                MessageBox.Show("Please enter a valid description and amount.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // --- DESIGNER COMPATIBILITY ---
        private void button1_Click(object sender, EventArgs e) { this.DialogResult = DialogResult.Cancel; this.Close(); }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e) { }
        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
    }
}