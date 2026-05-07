using CommonCents;
using Microsoft.VisualBasic.Logging;
using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1;

namespace AOOP_PROJECTT
{
    public partial class Form1 : Form
    {
        private usLogin login;

        public Form1()
        {
            InitializeComponent();
            rightsidepanel.Controls.Clear();
            var dashboard = new usDashboard();
            dashboard.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(dashboard);


        }

        public void NavigateTo(UserControl uc)
        {
          
        }

        public void NavigateFullScreen(UserControl uc)
        {
            this.Controls.Clear(); // Remove everything (sidebar, panels, etc.)
            uc.Dock = DockStyle.Fill;
            this.Controls.Add(uc);
        }

        private void Background_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTransactionForm form = new AddTransactionForm();
            form.ShowDialog();
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

     

        private void button3_Click(object sender, EventArgs e)
        {
            rightsidepanel.Controls.Clear();

            var transaction = new usTransaction();
            transaction.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(transaction);
            transaction.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rightsidepanel.Controls.Clear();
            var dashboard = new usAnalytics();
            rightsidepanel.Controls.Add(dashboard);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rightsidepanel.Controls.Clear();
            var dashboard = new usDashboard();
            rightsidepanel.Controls.Add(dashboard);
        }

        private void Background_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panelMainContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            rightsidepanel.Controls.Clear();
            var dashboard = new BudgetsGoalsTab();
            dashboard.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(dashboard);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rightsidepanel.Controls.Clear();
            var dashboard = new DebtsTab();
            dashboard.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(dashboard);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            rightsidepanel.Controls.Clear();
            var dashboard = new usRecurring_Payment();
            rightsidepanel.Controls.Add(new usRecurring_Payment());

            var tabBudgets = new TabPage("Budgets & Goals") { BackColor = Color.FromArgb(22, 27, 38), Padding = new Padding(0) };
        }

        private void button8_Click(object sender, EventArgs e)
        {
            rightsidepanel.Controls.Clear();
            var dashboard = new usProfile();
            rightsidepanel.Controls.Add(dashboard);

            //Navigation(new Profile());


        }

        private void button9_Click(object sender, EventArgs e)
        {
            rightsidepanel.Controls.Clear();
            var dashboard = new UserControl1();
            rightsidepanel.Controls.Add(new UserControl1());
        }

        private void rightsidepanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
