// BudgetsGoalsTab.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CommonCents
{
    public class BudgetsGoalsTab : UserControl
    {
        private List<Budget> _budgets = new List<Budget>();
        private List<SavingsGoal> _goals = new List<SavingsGoal>();

        private Panel pnlBudgetGrid;
        private Panel pnlGoalGrid;

        private static readonly Color BgDark   = Color.FromArgb(22, 27, 38);
        private static readonly Color CardBg   = Color.FromArgb(30, 36, 52);
        private static readonly Color Accent   = Color.FromArgb(230, 170, 0);
        private static readonly Color SubText  = Color.FromArgb(130, 145, 170);
        private static readonly Color GreenAmt = Color.FromArgb(80, 220, 120);

        public BudgetsGoalsTab()
        {
            BackColor = BgDark;
            Dock = DockStyle.Fill;
            BuildUI();
        }

        private void BuildUI()
        {
            var scroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = BgDark,
                Padding = new Padding(18)
            };
            Controls.Add(scroll);

            // top bar
            var topBar = new Panel { Height = 46, Dock = DockStyle.Top, BackColor = BgDark };
            var lblTitle = new Label
            {
                Text = "Budgets && Goals",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            var lblDate = new Label
            {
                Text = DateTime.Now.ToString("ddd, MMM dd, yyyy"),
                Font = new Font("Segoe UI", 9f),
                ForeColor = SubText,
                AutoSize = true
            };
            var btnAddTx = MakeYellowButton("+ Add Transaction", 148);
            btnAddTx.Click += (s, e) => MessageBox.Show("Transactions tab coming soon!", "Info");
            topBar.Controls.Add(lblTitle);
            topBar.Controls.Add(lblDate);
            topBar.Controls.Add(btnAddTx);
            topBar.Resize += (s, e) =>
            {
                btnAddTx.Location = new Point(topBar.Width - btnAddTx.Width - 2, 6);
                lblDate.Location  = new Point(btnAddTx.Left - lblDate.Width - 16, 14);
            };
            scroll.Controls.Add(topBar);

            // budget section
            var budgetSection = BuildSection(
                "BUDGET CATEGORIES — " + DateTime.Now.ToString("MMMM yyyy").ToUpper(),
                "+ Add Budget", BtnAddBudget_Click, out pnlBudgetGrid);
            scroll.Controls.Add(budgetSection);

            // goals section
            var goalsSection = BuildSection(
                "SAVINGS GOALS",
                "+ Add Goal", BtnAddGoal_Click, out pnlGoalGrid);
            goalsSection.Margin = new Padding(0, 14, 0, 0);
            scroll.Controls.Add(goalsSection);

            scroll.Controls.SetChildIndex(goalsSection,  0);
            scroll.Controls.SetChildIndex(budgetSection, 1);
            scroll.Controls.SetChildIndex(topBar,        2);

            RefreshAll();
        }

        private Panel BuildSection(string title, string btnLabel, EventHandler btnClick, out Panel grid)
        {
            var section = new Panel
            {
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(26, 31, 44),
                Padding = new Padding(16),
                Height = 60
            };

            var header = new Panel { Height = 38, Dock = DockStyle.Top, BackColor = Color.Transparent };
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = SubText,
                AutoSize = true,
                Location = new Point(0, 12)
            };
            var btn = MakeYellowButton(btnLabel, btnLabel.Length * 8 + 20);
            btn.Click += btnClick;
            header.Controls.Add(lblTitle);
            header.Controls.Add(btn);
            header.Resize += (s, e) => btn.Location = new Point(header.Width - btn.Width - 2, 4);
            section.Controls.Add(header);

           
            var innerGrid = new Panel
            {
                Dock = DockStyle.None,
                BackColor = Color.Transparent,
                Height = 0,
                Location = new Point(0, 44),
                Padding = new Padding(0, 6, 0, 6)
            };
            section.Controls.Add(innerGrid);

            section.Resize += (s, e) =>
                innerGrid.Width = section.Width - section.Padding.Left - section.Padding.Right;

            innerGrid.SizeChanged += (s, e) =>
                section.Height = header.Height + innerGrid.Height
                                 + section.Padding.Top + section.Padding.Bottom + 8;

            grid = innerGrid;
            return section;
        }

        private void RefreshAll()
        {
            RebuildGrid(pnlBudgetGrid, _budgets, BuildBudgetCard);
            RebuildGrid(pnlGoalGrid,   _goals,   BuildGoalCard);
        }

        private void RebuildGrid<T>(Panel grid, List<T> items, Func<T, Panel> cardBuilder)
        {
            grid.Controls.Clear();
            if (items.Count == 0) { grid.Height = 0; return; }

            int cols  = 2;
            int cardH = 140;
            int gap   = 12;
            int rows  = (int)Math.Ceiling(items.Count / (double)cols);

            grid.Height = rows * (cardH + gap) + gap;

            var table = new TableLayoutPanel
            {
                ColumnCount     = 2,
                RowCount        = rows,
                Dock            = DockStyle.Fill,
                BackColor       = Color.Transparent,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            for (int r = 0; r < rows; r++)
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, cardH + gap));

            for (int i = 0; i < items.Count; i++)
            {
                var card = cardBuilder(items[i]);
                card.Dock = DockStyle.Fill;
                card.Margin = new Padding(
                    i % 2 == 0 ? 0       : gap / 2,
                    gap / 2,
                    i % 2 == 0 ? gap / 2 : 0,
                    gap / 2);
                table.Controls.Add(card, i % cols, i / cols);
            }
            grid.Controls.Add(table);
        }

        // Budget card 
        private Panel BuildBudgetCard(Budget budget)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(16, 12, 16, 12),
                Cursor = Cursors.Hand
            };
            card.Paint += (s, e) => DrawRoundedBorder(e.Graphics, card);
            card.Click += (s, e) => OpenAddSpending(budget);

            var dot = MakeDot(budget.CategoryColor);
            dot.Location = new Point(16, 14);
            card.Controls.Add(dot);

            card.Controls.Add(new Label
            {
                Text = budget.Name,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(34, 13)
            });

            // Percentage bar for the added budget
            var lblPercent = new Label
            {
                Text = $"{(int)budget.ProgressPercent}%",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = PctColor(budget.ProgressPercent), 
                AutoSize = true
            };

            card.Controls.Add(lblPercent);

            // positioned in the top right corner
            card.Resize += (s, e) => lblPercent.Location = new Point(card.Width - lblPercent.Width - 16, 14);
            lblPercent.Location = new Point(card.Width - lblPercent.Width - 16, 14);

            var track = MakeTrack(card, 58);
            card.Resize += (s, e) =>
            {
                track.Width = card.Width - 32;
                RefreshTrack(track, budget.ProgressPercent, budget.CategoryColor);
            };
            RefreshTrack(track, budget.ProgressPercent, budget.CategoryColor);

            var lblSpent = new Label
            {
                Text = $"₱{budget.Spent:N0}",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(16, 76)
            };
            var lblLimit = new Label
            {
                Text = $"of ₱{budget.Limit:N0} limit",
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = SubText,
                AutoSize = true,
                Location = new Point(16, 96)
            };
            var lblRem = new Label
            {
                Text = $"₱{budget.Remaining:N0}",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = budget.Remaining < budget.Limit * 0.1
                    ? Color.FromArgb(255, 80, 80) : GreenAmt,
                AutoSize = true
            };
            var lblRemLbl = new Label
            {
                Text = "remaining",
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = SubText,
                AutoSize = true
            };
            card.Controls.Add(lblSpent);
            card.Controls.Add(lblLimit);
            card.Controls.Add(lblRem);
            card.Controls.Add(lblRemLbl);
            card.Resize += (s, e) =>
            {
                lblRem.Location    = new Point(card.Width - lblRem.Width - 16, 76);
                lblRemLbl.Location = new Point(card.Width - lblRemLbl.Width - 16, 96);
            };

            new ToolTip().SetToolTip(card, "Click to add spending");
            return card;
        }

        // Goal card
        private Panel BuildGoalCard(SavingsGoal goal)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(16, 12, 16, 12),
                Cursor = Cursors.Hand
            };
            card.Paint += (s, e) => DrawRoundedBorder(e.Graphics, card);
            card.Click += (s, e) => OpenAddContribution(goal);

            var dot = MakeDot(goal.GoalColor);
            dot.Location = new Point(16, 14);
            card.Controls.Add(dot);

            card.Controls.Add(new Label
            {
                Text = goal.Name,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(34, 13)
            });

            var lblPct = new Label
            {
                Text = $"{(int)goal.ProgressPercent}%",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = goal.GoalColor,
                AutoSize = true
            };
            card.Controls.Add(lblPct);
            card.Resize += (s, e) => lblPct.Location = new Point(card.Width - lblPct.Width - 14, 14);
            lblPct.Location = new Point(200, 14);

            var lblSaved = new Label
            {
                Text = $"₱{goal.Saved:N0}",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(14, 34) 
            };
            card.Controls.Add(lblSaved);

            var lblTarget = new Label
            {
                Text = $"of ₱{goal.Target:N0} target",
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = SubText,
                AutoSize = true
            };

            lblTarget.Location = new Point(16, lblSaved.Bottom - 2);
            card.Controls.Add(lblTarget);

            var track = MakeTrack(card, 78);
            card.Resize += (s, e) =>
            {
                track.Width = card.Width - 32;
                RefreshTrack(track, goal.ProgressPercent, goal.GoalColor);
            };
            RefreshTrack(track, goal.ProgressPercent, goal.GoalColor);

            card.Controls.Add(new Label
            {
                Text = goal.IsComplete ? "✓ Goal reached!" : $"₱{goal.Remaining:N0} to go",
                Font = new Font("Segoe UI", 8f),
                ForeColor = goal.IsComplete ? GreenAmt : SubText,
                AutoSize = true,
                Location = new Point(16, 96)
            });

            new ToolTip().SetToolTip(card, "Click to add contribution");
            return card;
        }

        // Actions
        private void BtnAddBudget_Click(object sender, EventArgs e)
        {
            using var form = new AddBudgetForm();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                _budgets.Add(new Budget(form.BudgetName, form.Limit, form.ChosenColor));
                RefreshAll();
            }
        }

        private void BtnAddGoal_Click(object sender, EventArgs e)
        {
            using var form = new AddGoalForm();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                var goal = new SavingsGoal(form.GoalName, form.Target, form.ChosenColor);
                if (form.InitialSaved > 0) goal.AddContribution(form.InitialSaved);
                _goals.Add(goal);
                RefreshAll();
            }
        }

        private void OpenAddSpending(Budget budget)
        {
            using var form = new AddSpendingForm(budget);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                budget.AddSpending(form.Amount);
                RefreshAll();
            }
        }

        private void OpenAddContribution(SavingsGoal goal)
        {
            using var form = new AddContributionForm(goal);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                goal.AddContribution(form.Amount);
                RefreshAll();
            }
        }

        // Helpers
        private Panel MakeTrack(Panel parent, int y)
        {
            var track = new Panel
            {
                Height = 7,
                BackColor = Color.FromArgb(45, 52, 70),
                Location = new Point(16, y),
                Width = Math.Max(1, parent.Width - 32)
            };
            parent.Controls.Add(track);
            return track;
        }

        private void RefreshTrack(Panel track, double pct, Color fillColor)
        {
            track.Controls.Clear();
            int w = (int)(track.Width * pct / 100.0);
            if (w < 2) return;
            track.Controls.Add(new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(w, track.Height),
                BackColor = fillColor
            });
        }

        private Panel MakeDot(Color c)
        {
            var dot = new Panel { Width = 12, Height = 12, BackColor = Color.Transparent };
            dot.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(c);
                e.Graphics.FillEllipse(brush, 0, 0, dot.Width - 1, dot.Height - 1);
            };
            return dot;
        }

        private Panel MakeBadge(string text, Color bg)
        {
            var circle = new Panel { Width = 42, Height = 42, BackColor = Color.Transparent };
            var lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 25, 35),
                AutoSize = false,
                Size = new Size(42, 42),
                Location = new Point(0, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Enabled = false
            };
            circle.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(bg);
                e.Graphics.FillEllipse(brush, 0, 0, circle.Width - 1, circle.Height - 1);
            };
            circle.Controls.Add(lbl);
            return circle;
        }

        private Color PctColor(double pct)
        {
            if (pct >= 90) return Color.FromArgb(230, 170, 0);
            if (pct >= 75) return Color.FromArgb(240, 140, 60);
            return Color.FromArgb(80, 200, 120);
        }

        private void DrawRoundedBorder(Graphics g, Panel card)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(50, 60, 80), 1.5f);
            using var path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 10);
            g.DrawPath(pen, path);
        }

        private Button MakeYellowButton(string text, int width)
        {
            var b = new Button
            {
                Text = text,
                BackColor = Color.FromArgb(230, 170, 0),
                ForeColor = Color.FromArgb(20, 20, 20),
                FlatStyle = FlatStyle.Flat,
                Height = 30,
                Width = width,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 200, 30);
            b.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var btn = (Button)s;
                using var path = RoundedRect(new Rectangle(0, 0, btn.Width - 1, btn.Height - 1), 6);
                using var brush = new SolidBrush(btn.BackColor);
                e.Graphics.FillPath(brush, path);
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(btn.Text, btn.Font,
                    new SolidBrush(btn.ForeColor),
                    new RectangleF(0, 0, btn.Width, btn.Height), sf);
            };
            return b;
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
