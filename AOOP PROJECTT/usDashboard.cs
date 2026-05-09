// usDashboard.cs — UPDATED
// Wires all dashboard panels to real DB data via DashboardRepository.
// Designer file (usDashboard.Designer.cs) is NOT changed.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AOOP_PROJECTT
{
    public partial class usDashboard : UserControl
    {
        public usDashboard()
        {
            InitializeComponent();

            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM d, yyyy");
            myDateTextBox.ReadOnly = true;

            // Load all dashboard data from DB
            LoadDashboard();
        }

        // ── MAIN LOAD ──────────────────────────────────────────────────
        private void LoadDashboard()
        {
            int userId = SessionManager.UserId;
            if (userId == 0) return;

            AddRoundedBorders();
            LoadSummary(userId);
            LoadBudgetStatus(userId);
            LoadSavingsGoals(userId);
        }

        // ── SUMMARY: Balance + Monthly Expenses ────────────────────────
        private void LoadSummary(int userId)
        {
            var summary = DashboardRepository.GetSummary(userId);

            // Total Balance (label2)
            label2.Text = $"₱{summary.TotalBalance:N2}";

            // Balance change indicator (label3)
            // Shows green if positive balance, red if negative
            if (summary.TotalBalance >= 0)
            {
                label3.ForeColor = Color.FromArgb(79, 255, 176);
                label3.Text = $"+₱{summary.MonthlyIncome - summary.MonthlyExpenses:N0} this month";
            }
            else
            {
                label3.ForeColor = Color.FromArgb(255, 107, 107);
                label3.Text = $"₱{summary.TotalBalance:N0} this month";
            }

            // Monthly Expenses (label5)
            label5.Text = $"₱{summary.MonthlyExpenses:N2}";

            // Expense % change vs last month (label6)
            double expChange = summary.ExpenseChangePercent;
            if (expChange >= 0)
            {
                label6.ForeColor = Color.FromArgb(255, 107, 107); // red = spending more
                label6.Text = $"+{expChange:N0}% vs last month";
            }
            else
            {
                label6.ForeColor = Color.FromArgb(79, 255, 176); // green = spending less
                label6.Text = $"{expChange:N0}% vs last month";
            }

            // % of income spent progress bar (replaces textBox8)
            // Remove textBox8 and draw a proper progress bar instead
            textBox8.Visible = false;
            DrawIncomeSpentBar(summary.IncomeSpentPercent);

            // Update label7 to show the actual percentage
            label7.Text = $"{(int)summary.IncomeSpentPercent}% of income spent";
        }

        // ── INCOME SPENT PROGRESS BAR (inside panel2) ──────────────────

        private void AddRoundedBorders()
        {
            UIHelper.ApplyRoundedStyle(panel2);
            UIHelper.ApplyRoundedStyle(panel3);
            UIHelper.ApplyRoundedStyle(panel4);
            UIHelper.ApplyRoundedStyle(panel5);
        }

        private void DrawIncomeSpentBar(double percent)
        {
            // Remove any existing progress bar we added before
            string trackTag = "incomeTrack";
            foreach (Control c in panel2.Controls)
                if (c.Tag?.ToString() == trackTag) { panel2.Controls.Remove(c); break; }

            var track = new Panel
            {
                Height = 8,
                Width = panel2.Width - 38,
                Location = new Point(19, 120),
                BackColor = Color.FromArgb(45, 52, 70),
                Tag = trackTag
            };

            int fillW = (int)(track.Width * percent / 100.0);
            if (fillW > 0)
            {
                track.Controls.Add(new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(fillW, track.Height),
                    BackColor = Color.FromArgb(230, 170, 0)
                });
            }

            panel2.Controls.Add(track);
            track.BringToFront();
        }

        // ── BUDGET STATUS (panel4) ──────────────────────────────────────
        private void LoadBudgetStatus(int userId)
        {
            var budgets = DashboardRepository.GetBudgetStatus(userId);

            // Clear anything already in panel4 except label8
            var toRemove = new List<Control>();
            foreach (Control c in panel4.Controls)
                if (c != label8) toRemove.Add(c);
            foreach (var c in toRemove) panel4.Controls.Remove(c);

            if (budgets.Count == 0)
            {
                panel4.Controls.Add(new Label
                {
                    Text = "No budgets set for this month.",
                    ForeColor = Color.FromArgb(130, 145, 170),
                    Font = new Font("Segoe UI", 9f),
                    AutoSize = true,
                    Location = new Point(24, 60)
                });
                return;
            }

            int startY = 55;
            int rowH = 50;

            foreach (var budget in budgets)
            {
                Color fillColor;
                try { fillColor = ColorTranslator.FromHtml(budget.CategoryColor); }
                catch { fillColor = Color.FromArgb(230, 170, 0); }

                // Budget name + amounts
                var lblName = new Label
                {
                    Text = budget.Name,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9f),
                    AutoSize = true,
                    Location = new Point(24, (startY - 5))
                };
                var lblAmounts = new Label
                {
                    Text = $"₱{budget.Spent:N0} / ₱{budget.Limit:N0}",
                    ForeColor = Color.FromArgb(130, 145, 170),
                    Font = new Font("Segoe UI", 8f),
                    AutoSize = true
                };
                var lblPct = new Label
                {
                    Text = $"{(int)budget.Percent}%",
                    ForeColor = Color.FromArgb(130, 145, 170),
                    Font = new Font("Segoe UI", 8f),
                    AutoSize = true
                };

                // Progress track
                var track = new Panel
                {
                    Height = 6,
                    Width = panel4.Width - 48,
                    Location = new Point(24, startY + 18),
                    BackColor = Color.FromArgb(45, 52, 70)
                };
                int fillW = (int)(track.Width * budget.Percent / 100.0);
                if (fillW > 0)
                    track.Controls.Add(new Panel
                    {
                        Location = new Point(0, 0),
                        Size = new Size(fillW, track.Height),
                        BackColor = fillColor
                    });

                // Position right-aligned labels
                lblAmounts.Location = new Point(24, startY + 28);
                lblPct.Location = new Point(panel4.Width - 60, startY - 3);

                panel4.Controls.Add(lblName);
                panel4.Controls.Add(lblAmounts);
                panel4.Controls.Add(lblPct);
                panel4.Controls.Add(track);

                startY += rowH;
            }
        }

        // ── SAVINGS GOALS (panel5) ──────────────────────────────────────
        private void LoadSavingsGoals(int userId)
        {
            var goals = DashboardRepository.GetSavingsGoals(userId);

            // Clear anything already in panel5 except label9
            var toRemove = new List<Control>();
            foreach (Control c in panel5.Controls)
                if (c != label9) toRemove.Add(c);
            foreach (var c in toRemove) panel5.Controls.Remove(c);

            if (goals.Count == 0)
            {
                panel5.Controls.Add(new Label
                {
                    Text = "No savings goals set yet.",
                    ForeColor = Color.FromArgb(130, 145, 170),
                    Font = new Font("Segoe UI", 9f),
                    AutoSize = true,
                    Location = new Point(14, 60)
                });
                return;
            }

            int startY = 55;
            int rowH = 50;

            foreach (var goal in goals)
            {
                Color fillColor;
                try { fillColor = ColorTranslator.FromHtml(goal.Color); }
                catch { fillColor = Color.FromArgb(46, 204, 113); }

                // Goal name + percent
                var lblName = new Label
                {
                    Text = goal.Name,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9f),
                    AutoSize = true,
                    Location = new Point(14, startY - 5)
                };
                var lblPct = new Label
                {
                    Text = $"{(int)goal.Percent}%",
                    ForeColor = fillColor,
                    Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                    AutoSize = true
                };

                // Saved / Target
                var lblProgress = new Label
                {
                    Text = $"₱{goal.Saved:N0} / ₱{goal.Target:N0}",
                    ForeColor = Color.FromArgb(130, 145, 170),
                    Font = new Font("Segoe UI", 8f),
                    AutoSize = true,
                    Location = new Point(14, startY + 28)
                };

                // Progress track
                var track = new Panel
                {
                    Height = 6,
                    Width = panel5.Width - 28,
                    Location = new Point(14, startY + 18),
                    BackColor = Color.FromArgb(45, 52, 70)
                };
                int fillW = (int)(track.Width * goal.Percent / 100.0);
                if (fillW > 0)
                    track.Controls.Add(new Panel
                    {
                        Location = new Point(0, 0),
                        Size = new Size(fillW, track.Height),
                        BackColor = fillColor
                    });

                lblPct.Location = new Point(panel5.Width - 55, startY - 3);

                panel5.Controls.Add(lblName);
                panel5.Controls.Add(lblPct);
                panel5.Controls.Add(lblProgress);
                panel5.Controls.Add(track);

                startY += rowH;
            }
        }

        // ── EXISTING HANDLERS (unchanged) ──────────────────────────────
        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(50, 60, 80), 1f);
            e.Graphics.DrawLine(pen, 0, panel7.Height - 1,
                                panel7.Width, panel7.Height - 1);
        }

        private void myDateTextBox_TextChanged(object sender, EventArgs e)
        {
            myDateTextBox.Text = DateTime.Now.ToString("ddd, MMM d, yyyy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.OpenAddTransaction(ParentForm, LoadDashboard);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void textBox8_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void progressBar1_Click(object sender, EventArgs e) { }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
