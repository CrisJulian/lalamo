// AnalyticsTab.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WinFormsApp1;
using CommonCents; 

namespace AOOP_PROJECTT
{
    public class AnalyticsTab : UserControl
    {
        private static readonly Color BgDark = Color.FromArgb(22, 27, 38);
        private static readonly Color CardBg = Color.FromArgb(30, 36, 52);
        private static readonly Color Accent = Color.FromArgb(230, 170, 0);
        private static readonly Color SubText = Color.FromArgb(130, 145, 170);
        private static readonly Color GreenAmt = Color.FromArgb(80, 220, 120);
        private static readonly Color RedAmt = Color.FromArgb(255, 80, 100);
        private static readonly Color BtnActive = Color.FromArgb(50, 60, 85);

        // controls updated on refresh
        private Panel donutPanel, legendPanel, barChartPanel, gaugePanel;
        private Label lblHealthTitle, lblScoreHighlight, lblHealthLabel;
        private Label lblSavingsRate, lblDebtRatio, lblBudgetKept;
        private Label lblSavingsCaption, lblDebtCaption, lblBudgetCaption;
        private Label[] _insightLabels;

        private ChartPeriod _period = ChartPeriod.Monthly;
        private Button _btnMonthly, _btnWeekly, _btnDaily;

        public AnalyticsTab()
        {
            BackColor = BgDark;
            Dock = DockStyle.Fill;
            BuildUI();
            VisibleChanged += (s, e) => { if (Visible) RefreshAll(); };
        }

        // BUILD UI
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
            topBar.Controls.Add(new Label
            {
                Text = "Analytics",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(0, 8)
            });
            var lblDate = new Label
            {
                Text = DateTime.Now.ToString("ddd, MMM d, yyyy"),
                Font = new Font("Segoe UI", 9f),
                ForeColor = SubText,
                AutoSize = true
            };
            var btnAddTx = MakeYellowButton("+ Add Transaction", 148);
            btnAddTx.Click += BtnAddTransaction_Click!;
            topBar.Controls.Add(lblDate);
            topBar.Controls.Add(btnAddTx);
            topBar.Resize += (s, e) =>
            {
                btnAddTx.Location = new Point(topBar.Width - btnAddTx.Width - 2, 6);
                lblDate.Location = new Point(btnAddTx.Left - lblDate.Width - 16, 14);
            };
            scroll.Controls.Add(topBar);

            // row 1: donut + bar chart
            var row1 = new TableLayoutPanel
            {
                Height = 310,
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = BgDark,
                Margin = new Padding(0, 12, 0, 0)
            };
            row1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            row1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            row1.Controls.Add(BuildDonutCard(), 0, 0);
            row1.Controls.Add(BuildBarChartCard(), 1, 0);
            scroll.Controls.Add(row1);

            // row 2: financial health
            var healthCard = BuildHealthCard();
            healthCard.Margin = new Padding(0, 12, 0, 0);
            scroll.Controls.Add(healthCard);

            scroll.Controls.SetChildIndex(healthCard, 0);
            scroll.Controls.SetChildIndex(row1, 1);
            scroll.Controls.SetChildIndex(topBar, 2);
        }

        //  DONUT CARD
        private Panel BuildDonutCard()
        {
            var card = MakeCard();
            card.Padding = new Padding(16);
            card.Controls.Add(MakeSectionLabel("SPENDING BY CATEGORY"));

            donutPanel = new Panel
            {
                Width = 155,
                Height = 155,
                BackColor = Color.Transparent,
                Location = new Point(16, 36)
            };
            donutPanel.Paint += PaintDonut!;
            card.Controls.Add(donutPanel);

            legendPanel = new Panel
            {
                Location = new Point(182, 36),
                Width = 170,
                Height = 220,
                BackColor = Color.Transparent
            };
            card.Controls.Add(legendPanel);
            return card;
        }

        private void PaintDonut(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(4, 4, donutPanel.Width - 8, donutPanel.Height - 8);

            double total = 0;
            foreach (var b in AppData.Budgets) total += b.Spent;

            if (AppData.Budgets.Count == 0 || total == 0)
            {
                using var pen = new Pen(Color.FromArgb(50, 60, 80), 22f);
                g.DrawEllipse(pen, rect);
            }
            else
            {
                float start = -90f;
                foreach (var b in AppData.Budgets)
                {
                    float sweep = (float)(b.Spent / total * 360);
                    if (sweep < 0.5f) continue;
                    using var brush = new SolidBrush(b.CategoryColor);
                    g.FillPie(brush, rect, start, sweep);
                    start += sweep;
                }
            }

            // punch center hole
            int hole = rect.Width / 3;
            int cx = rect.X + rect.Width / 2 - hole / 2;
            int cy = rect.Y + rect.Height / 2 - hole / 2;
            using var holeBrush = new SolidBrush(CardBg);
            g.FillEllipse(holeBrush, cx, cy, hole, hole);
        }

        private void RefreshDonutLegend()
        {
            legendPanel.Controls.Clear();
            double total = 0;
            foreach (var b in AppData.Budgets) total += b.Spent;
            if (total == 0) total = 1;

            int ly = 0;
            foreach (var b in AppData.Budgets)
            {
                int pct = (int)(b.Spent / total * 100);
                var col = b.CategoryColor;
                var dot = new Panel
                {
                    Width = 10,
                    Height = 10,
                    BackColor = Color.Transparent,
                    Location = new Point(0, ly + 3)
                };
                dot.Paint += (ds, de) =>
                {
                    de.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using var brush = new SolidBrush(col);
                    de.Graphics.FillEllipse(brush, 0, 0, 9, 9);
                };
                legendPanel.Controls.Add(dot);
                legendPanel.Controls.Add(new Label
                {
                    Text = b.Name,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(14, ly)
                });
                legendPanel.Controls.Add(new Label
                {
                    Text = $"{pct}%",
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = SubText,
                    AutoSize = true,
                    Location = new Point(118, ly)
                });
                ly += 22;
            }
        }

        // BAR CHART CARD
        private Panel BuildBarChartCard()
        {
            var card = MakeCard();
            card.Padding = new Padding(16);
            card.Margin = new Padding(10, 0, 0, 0);
            card.Controls.Add(MakeSectionLabel("INCOME VS EXPENSES"));

            _btnMonthly = MakePeriodButton("Monthly");
            _btnWeekly = MakePeriodButton("Weekly");
            _btnDaily = MakePeriodButton("Daily");
            _btnMonthly.Click += (s, e) => SetPeriod(ChartPeriod.Monthly);
            _btnWeekly.Click += (s, e) => SetPeriod(ChartPeriod.Weekly);
            _btnDaily.Click += (s, e) => SetPeriod(ChartPeriod.Daily);

            var btnRow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(0, 10)
            };
            btnRow.Controls.Add(_btnMonthly);
            btnRow.Controls.Add(_btnWeekly);
            btnRow.Controls.Add(_btnDaily);
            card.Controls.Add(btnRow);
            card.Resize += (s, e) =>
                btnRow.Location = new Point(card.Width - btnRow.Width - 16, 10);

            barChartPanel = new Panel { BackColor = Color.Transparent, Location = new Point(16, 42) };
            barChartPanel.Paint += PaintBarChart!;
            card.Resize += (s, e) =>
                barChartPanel.Size = new Size(card.Width - 32, card.Height - 90);
            card.Controls.Add(barChartPanel);

            AddLegendDot(card, GreenAmt, "Income", 16, card.Height - 26);
            AddLegendDot(card, RedAmt, "Expenses", 90, card.Height - 26);
            card.Resize += (s, e) =>
            {
                foreach (Control c in card.Controls)
                    if (c.Tag is string t && t == "legend") c.Top = card.Height - 26;
            };

            HighlightPeriodButton();
            return card;
        }

        private void SetPeriod(ChartPeriod period)
        {
            _period = period;
            HighlightPeriodButton();
            barChartPanel.Invalidate();
        }

        private void HighlightPeriodButton()
        {
            _btnMonthly.BackColor = _period == ChartPeriod.Monthly ? BtnActive : Color.FromArgb(36, 43, 58);
            _btnWeekly.BackColor = _period == ChartPeriod.Weekly ? BtnActive : Color.FromArgb(36, 43, 58);
            _btnDaily.BackColor = _period == ChartPeriod.Daily ? BtnActive : Color.FromArgb(36, 43, 58);
        }

        private void PaintBarChart(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var panel = (Panel)sender;
            int w = panel.Width, h = panel.Height - 30;
            if (w < 20 || h < 20) return;

            var data = AppData.GetChartData(_period);
            double maxVal = 1;
            foreach (var (_, inc, exp) in data)
                maxVal = Math.Max(maxVal, Math.Max(inc, exp));
            maxVal *= 1.2;

            using var gridPen = new Pen(Color.FromArgb(40, 50, 68), 1f);
            for (int i = 0; i <= 4; i++)
            {
                int y = h - (int)(h * i / 4.0);
                g.DrawLine(gridPen, 44, y, w, y);
                double v = maxVal * i / 4;
                string lbl = v >= 1000 ? $"₱{(int)(v / 1000)}k" : $"₱{(int)v}";
                using var sf = new StringFormat { Alignment = StringAlignment.Far };
                g.DrawString(lbl, new Font("Segoe UI", 7f), new SolidBrush(SubText),
                    new RectangleF(0, y - 8, 42, 16), sf);
            }

            int n = data.Length;
            float slotW = (w - 44f) / n;
            float barW = slotW * 0.28f;
            float gap = slotW * 0.05f;

            for (int i = 0; i < n; i++)
            {
                var (label, income, expense) = data[i];
                float slotX = 44 + i * slotW + slotW * 0.08f;
                float incH = (float)(h * income / maxVal);
                float expH = (float)(h * expense / maxVal);

                if (incH > 1) { using var b = new SolidBrush(GreenAmt); g.FillRectangle(b, slotX, h - incH, barW, incH); }
                if (expH > 1) { using var b = new SolidBrush(RedAmt); g.FillRectangle(b, slotX + barW + gap, h - expH, barW, expH); }

                using var sfC = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString(label, new Font("Segoe UI", 7f), new SolidBrush(SubText),
                    new RectangleF(slotX - gap, h + 4, slotW * 0.9f, 20), sfC);
            }
        }

        private void AddLegendDot(Panel card, Color col, string text, int x, int y)
        {
            card.Controls.Add(new Panel { Width = 10, Height = 10, BackColor = col, Location = new Point(x, y), Tag = "legend" });
            card.Controls.Add(new Label { Text = text, Font = new Font("Segoe UI", 8f), ForeColor = Color.White, AutoSize = true, Location = new Point(x + 14, y - 1), Tag = "legend" });
        }

        //  FINANCIAL HEALTH CARD
        //  Layout: left = gauge + "Fair" + 3 stats underneath
        //          right = title + score + description + bullets

        private Panel BuildHealthCard()
        {
            var card = MakeCard();
            card.Height = 320;
            card.Padding = new Padding(16);
            card.Controls.Add(MakeSectionLabel("FINANCIAL HEALTH RATING"));

            // LEFT COLUMN

            // arc gauge (200x200)
            gaugePanel = new Panel
            {
                Width = 200,
                Height = 200,
                BackColor = Color.Transparent,
                Location = new Point(10, 30),
                Tag = 0
            };
            gaugePanel.Paint += (s, e) =>
                PaintGauge(e.Graphics, gaugePanel, (int)(gaugePanel.Tag ?? 0));
            card.Controls.Add(gaugePanel);

            // "Fair" / "Good" label centered under the gauge
            lblHealthLabel = new Label
            {
                Text = "—",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Accent,
                AutoSize = true,
                Location = new Point(80, 234)   // centered under 200px gauge
            };
            card.Controls.Add(lblHealthLabel);

            // THREE STAT PAIRS under the gauge
            // Each pair: big % value + small caption below it

            lblSavingsRate = MakeBigStat("0%");
            lblDebtRatio = MakeBigStat("0%");
            lblBudgetKept = MakeBigStat("0%");
            lblSavingsCaption = MakeSmallCaption("SAVINGS RATE");
            lblDebtCaption = MakeSmallCaption("DEBT PAID");
            lblBudgetCaption = MakeSmallCaption("BUDGET KEPT");

            // x positions: 14, 80, 148  (evenly spread under 200px gauge)
            PlaceStatPair(card, lblSavingsRate, lblSavingsCaption, 14, 264);
            PlaceStatPair(card, lblDebtRatio, lblDebtCaption, 80, 264);
            PlaceStatPair(card, lblBudgetKept, lblBudgetCaption, 152, 264);

            // RIGHT COLUMN
            int rx = 230;

            lblHealthTitle = new Label
            {
                Text = "Add budgets, goals && debts to see your score.",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(400, 44),
                Location = new Point(rx, 40)
            };
            card.Controls.Add(lblHealthTitle);

            // "Your score of XX/100"
            card.Controls.Add(new Label
            {
                Text = "Your score of",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(190, 200, 220),
                AutoSize = true,
                Location = new Point(rx, 90)
            });

            lblScoreHighlight = new Label
            {
                Text = "0/100",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = GreenAmt,
                AutoSize = true,
                Location = new Point(rx + 82, 90)
            };
            card.Controls.Add(lblScoreHighlight);
            card.Controls.SetChildIndex(lblScoreHighlight, 0);

            card.Controls.Add(new Label
            {
                Text = "is based on your budget usage, savings progress,\n" +
                       "and debt repayment across all your tracked items.",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(190, 200, 220),
                AutoSize = false,
                Size = new Size(400, 44),
                Location = new Point(rx, 108)
            });

            // bullet insights
            BuildInsightBullets(card, rx, 158);

            return card;
        }

        // draws the three-quarter arc gauge
        private void PaintGauge(Graphics g, Panel panel, int score)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(12, 12, panel.Width - 24, panel.Height - 24);

            // dark background track
            using var trackPen = new Pen(Color.FromArgb(45, 52, 70), 16f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            g.DrawArc(trackPen, rect, 135, 270);

            // colored arc — color changes based on score range
            float sweep = (float)(score / 100.0 * 270);
            Color arcColor = score >= 80 ? GreenAmt
                           : score >= 60 ? Accent
                           : score >= 40 ? Color.FromArgb(240, 140, 60)
                           : RedAmt;

            if (sweep > 0.5f)
            {
                using var scorePen = new Pen(arcColor, 16f)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round
                };
                g.DrawArc(scorePen, rect, 135, sweep);
            }

            // score number centered in gauge
            using var bigFont = new Font("Segoe UI", 32f, FontStyle.Bold);
            using var smallFont = new Font("Segoe UI", 9f);
            using var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(score.ToString(), bigFont, new SolidBrush(arcColor),
                new RectangleF(0, -12, panel.Width, panel.Height), sf);
            g.DrawString("/ 100", smallFont, new SolidBrush(SubText),
                new RectangleF(0, 28, panel.Width, panel.Height), sf);
        }

        // places a value label + caption label as a stat pair
        private void PlaceStatPair(Panel card, Label value, Label caption, int x, int y)
        {
            value.Location = new Point(x, y);
            caption.Location = new Point(x, y + 22);
            card.Controls.Add(value);
            card.Controls.Add(caption);
        }

        // insight bullet points
        private void BuildInsightBullets(Panel card, int rx, int startY)
        {
            _insightLabels = new Label[4];
            var dotColors = new[] { RedAmt, GreenAmt, GreenAmt, Accent };
            int iy = startY;

            for (int i = 0; i < 4; i++)
            {
                var col = dotColors[i];
                var dot = new Panel
                {
                    Width = 8,
                    Height = 8,
                    BackColor = Color.Transparent,
                    Location = new Point(rx, iy + 4)
                };
                dot.Paint += (ds, de) =>
                {
                    de.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using var b = new SolidBrush(col);
                    de.Graphics.FillEllipse(b, 0, 0, 7, 7);
                };
                card.Controls.Add(dot);

                _insightLabels[i] = new Label
                {
                    Text = "—",
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.FromArgb(190, 200, 220),
                    AutoSize = true,
                    Location = new Point(rx + 14, iy)
                };
                card.Controls.Add(_insightLabels[i]);
                iy += 22;
            }
        }

        //  REFRESH  — pulls live values from AppData
        private void RefreshAll()
        {
            int score = AppData.HealthScore;

            // gauge
            gaugePanel.Tag = score;
            gaugePanel.Invalidate();

            // health label color matches arc color
            Color labelColor = score >= 80 ? GreenAmt
                             : score >= 60 ? Accent
                             : score >= 40 ? Color.FromArgb(240, 140, 60)
                             : RedAmt;
            lblHealthLabel.ForeColor = labelColor;
            lblHealthLabel.Text = AppData.HealthLabel;

            // title
            lblHealthTitle.Text = score >= 60
                ? "Your financial health is on track."
                : score >= 40
                    ? "Your financial health needs improvement."
                    : "Your financial health needs urgent attention.";

            lblScoreHighlight.Text = $"{score}/100";

            // stat values with 1 decimal place
            lblBudgetKept.Text = $"{AppData.BudgetKeptPct:F1}%";
            lblSavingsRate.Text = $"{AppData.SavingsRatePct:F1}%";
            lblDebtRatio.Text = $"{AppData.DebtPaidPct:F1}%";

            // insight bullets
            if (_insightLabels != null)
            {
                double usage = AppData.BudgetUsagePct;
                double debt = AppData.DebtPaidPct;
                double savings = AppData.SavingsRatePct;

                _insightLabels[0].Text = usage >= 90
                    ? $"Budget nearly exhausted ({usage:F0}% used)"
                    : $"Budget usage at {usage:F0}% of total limit";

                _insightLabels[1].Text = debt >= 50
                    ? $"Debt {debt:F0}% repaid — great progress!"
                    : $"Debt repayment at {debt:F0}% — keep going";

                _insightLabels[2].Text = savings >= 50
                    ? $"Savings at {savings:F0}% of your goals"
                    : $"Savings at {savings:F0}% — try to save more";

                _insightLabels[3].Text = score >= 70
                    ? "Overall financial health is solid"
                    : "Focus on reducing spending to improve score";
            }

            // donut + bar chart
            RefreshDonutLegend();
            donutPanel.Invalidate();
            barChartPanel.Invalidate();
        }

        //Add Transaction 
        private void BtnAddTransaction_Click(object sender, EventArgs e)
        {
            using var dlg = new AddTransactionForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AppData.Transactions.Add(new Transaction(
                    DateTime.Now,
                    double.TryParse(dlg.TAmount, out var v) ? v : 0,
                    dlg.TType == "Income",
                    dlg.TCategory));
            }
        }

        //  HELPERS

        // big bold percentage label for the stat row
        private Label MakeBigStat(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 13f, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true
        };

        // small all-caps caption below each stat
        private Label MakeSmallCaption(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 7f, FontStyle.Bold),
            ForeColor = SubText,
            AutoSize = true
        };

        private Panel MakeCard()
        {
            var card = new Panel { Dock = DockStyle.Fill, BackColor = CardBg };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(50, 60, 80), 1.5f);
                using var path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 10);
                e.Graphics.DrawPath(pen, path);
            };
            return card;
        }

        private Label MakeSectionLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
            ForeColor = SubText,
            AutoSize = true,
            Location = new Point(16, 14)
        };

        private Button MakePeriodButton(string text)
        {
            var b = new Button
            {
                Text = text,
                BackColor = Color.FromArgb(36, 43, 58),
                ForeColor = Color.FromArgb(190, 200, 220),
                FlatStyle = FlatStyle.Flat,
                Height = 24,
                Width = 64,
                Font = new Font("Segoe UI", 8f),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 4, 0)
            };
            b.FlatAppearance.BorderColor = Color.FromArgb(55, 65, 85);
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.MouseOverBackColor = BtnActive;
            return b;
        }

        private Button MakeYellowButton(string text, int width)
        {
            var b = new Button
            {
                Text = text,
                BackColor = Accent,
                ForeColor = Color.FromArgb(20, 20, 20),
                FlatStyle = FlatStyle.Flat,
                Height = 32,
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
                e.Graphics.DrawString(btn.Text, btn.Font, new SolidBrush(btn.ForeColor),
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