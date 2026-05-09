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
            var db = new usDashboard(); // 👈 rename from dashboard to db
            db.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(db);
            SetActiveButton(dashboard); // 👈 now correctly refers to the Button
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            foreach (Control c in LeftSidePanel.Controls)
            {
                if (c is Button b)
                {
                    b.FlatAppearance.MouseOverBackColor = Color.FromArgb(19, 22, 31);
                }
            }

            label1.Text = SessionManager.FullName;
            label2.Text = SessionManager.AccountType;
        }

        private Button _activeButton;



        // Testing

        private void SetActiveButton(Button btn)
        {
            // Reset previous active button
            if (_activeButton != null)
            {
                _activeButton.BackColor = Color.Transparent;
                _activeButton.ForeColor = Color.White;
                _activeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 255, 255, 255); // subtle white hover
            }

            // Set new active button — semi-transparent yellow
            _activeButton = btn;
            _activeButton.BackColor = Color.FromArgb(40, 245, 166, 35); // 👈 low opacity yellow
            _activeButton.ForeColor = Color.FromArgb(245, 166, 35);      // 👈 yellow text
            _activeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 245, 166, 35); // 👈 keep yellow on hover
        }

        public static void OpenAddTransaction(Form parentForm, Action onSuccess = null)
        {
            using (AddTransactionForm modal = new AddTransactionForm())
            {
                modal.StartPosition = FormStartPosition.CenterParent;
                if (modal.ShowDialog(parentForm) == DialogResult.OK)
                {
                    var tx = new TransactionRecord
                    {
                        Amount = double.Parse(modal.TAmount),
                        Description = modal.TDescription,
                        Category = modal.TCategory,
                        Type = modal.TType,
                        Date = DateTime.Now
                    };
                    int newId = TransactionRepository.AddTransaction(SessionManager.UserId, tx);

                    if (modal.TType == "Expense")
                        TransactionRepository.UpdateBudgetSpent(
                            SessionManager.UserId, modal.TCategory, double.Parse(modal.TAmount));

                    onSuccess?.Invoke();
                }
            }
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
            SetActiveButton(button3);
            rightsidepanel.Controls.Clear();
            var transaction = new usTransactionNew();
            transaction.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(transaction);
            transaction.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetActiveButton(button4);
            rightsidepanel.Controls.Clear();
            var analytics = new AnalyticsTab();
            analytics.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(analytics);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetActiveButton(dashboard);
            rightsidepanel.Controls.Clear();
            var db = new usDashboard();
            db.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(db);
        }

        private void Background_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panelMainContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetActiveButton(button5);
            rightsidepanel.Controls.Clear();
            var dashboard = new BudgetsGoalsTab();
            dashboard.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(dashboard);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetActiveButton(button6);
            rightsidepanel.Controls.Clear();
            var dashboard = new DebtsTab();
            dashboard.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(dashboard);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SetActiveButton(button7);
            rightsidepanel.Controls.Clear();
            var recurring = new usRecurring_Payment();
            recurring.Dock = DockStyle.Fill;
            rightsidepanel.Controls.Add(recurring);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SetActiveButton(button8);
            rightsidepanel.Controls.Clear();
            var dashboard = new usProfile();
            rightsidepanel.Controls.Add(dashboard);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SetActiveButton(button9);
            rightsidepanel.Controls.Clear();
            var settings = new UserControl1();
            settings.Dock = DockStyle.Fill;
            settings.LogoutRequested += (s, ev) =>
            {
                SessionManager.Clear();
                Application.Restart();
            };
            rightsidepanel.Controls.Add(settings);
        }

        private void rightsidepanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LeftSidePanel_Paint(object sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(70, 80, 100), 1f);
            e.Graphics.DrawLine(pen, LeftSidePanel.Width - 1, 0,
                                LeftSidePanel.Width - 1, LeftSidePanel.Height);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(50, 60, 80), 1f);
            e.Graphics.DrawLine(pen, 0, panel6.Height - 1,
                                panel6.Width, panel6.Height - 1);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
