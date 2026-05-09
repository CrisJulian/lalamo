// AnalyticsTab.cs
using AOOP_PROJECTT;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CommonCents
{
    public class AnalyticsTab : UserControl
    {
        private static readonly Color BgDark = Color.FromArgb(15, 17, 23);
        private static readonly Color CardBg = Color.FromArgb(24, 28, 38);
        private static readonly Color Accent = Color.FromArgb(230, 170, 0);
        private static readonly Color SubText = Color.FromArgb(130, 145, 170);
        private static readonly Color GreenAmt = Color.FromArgb(80, 220, 120);
        private static readonly Color RedAmt = Color.FromArgb(255, 80, 100);
        private static readonly Color BtnActive = Color.FromArgb(50, 60, 85);

        private List<AnalyticsBudgetItem> _budgets = new();

        // panels
        private Panel donutPanel, legendPanel, barChartPanel, gaugePanel;

        // health labels
        private Label lblHealthTitle, lblScoreHighlight, lblHealthLabel;
        private Label lblSavingsRate, lblDebtRatio, lblBudgetKept;
        private Label lblSavingsCaption, lblDebtCaption, lblBudgetCaption;
        private Label[] _insightLabels;

        // chart period
        private ChartPeriod _period = ChartPeriod.Monthly;
        private Button _btnMonthly, _btnWeekly, _btnDaily;


        public AnalyticsTab()
        {
            BackColor = BgDark;
            Dock = DockStyle.Fill;
            BuildUI();

            // 👇 Replace the old VisibleChanged with this
            VisibleChanged += (s, e) =>
            {
                if (!Visible) return;
                // Delay refresh until after layout is complete
                this.BeginInvoke((Action)(() =>
                {
                    RefreshAll();
                    donutPanel.Invalidate();
                    barChartPanel.Invalidate();
                    gaugePanel.Invalidate();
                }));
            };

            this.HandleCreated += (s, e) =>
            {
                this.BeginInvoke((Action)(() => RefreshAll()));
            };
        }


        // ═══════════════════════════════════════════════════════════
        //  BUILD UI
        // ═══════════════════════════════════════════════════════════
        private void BuildUI()
        {
            // ── Top bar ─────────────────────────────────────────────
            var topBar = new Panel { Height = 54, Dock = DockStyle.Top, BackColor = BgDark };
            topBar.Controls.Add(new Label
            {
                Text = "Analytics",
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 13)
            });
            var lblDate = new Label
            {
                Text = DateTime.Now.ToString("ddd, MMM d, yyyy"),
                Font = new Font("Segoe UI", 9f),
                ForeColor = SubText,
                AutoSize = true
            };
            var btnAddTx = MakeYellowButton("+ Add Transaction", 160);
            btnAddTx.Click += (s, e) => Form1.OpenAddTransaction(ParentForm, RefreshAll);
            topBar.Controls.Add(lblDate);
            topBar.Controls.Add(btnAddTx);
            topBar.Resize += (s, e) =>
            {
                btnAddTx.Location = new Point(topBar.Width - btnAddTx.Width - 20, 13);
                lblDate.Location = new Point(btnAddTx.Left - lblDate.Width - 30, 20);
            };

            // ── Separator ────────────────────────────────────────────
            var separator = new Panel
            {
                Height = 1,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(50, 60, 80)
            };

            // ── Scroll ───────────────────────────────────────────────
            var scroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = BgDark,
                Padding = new Padding(24, 8, 24, 24) // 👈 increase top from 12 to 20
            };

            // Add in REVERSE order for DockStyle.Top to work correctly
            Controls.Add(scroll);
            Controls.Add(separator);
            Controls.Add(topBar);

            // ── Row 1: donut + bar chart ─────────────────────────────
            var row1 = new TableLayoutPanel
            {
                Height = 250,
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = BgDark,
                Margin = new Padding(0, 12, 0, 0)
            };
            row1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42f));
            row1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58f));
            row1.Padding = new Padding(0, 0, 0, 0);
            row1.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            row1.Controls.Add(BuildDonutCard(), 0, 0);
            row1.Controls.Add(BuildBarChartCard(), 1, 0);

            // ── Spacer ───────────────────────────────────────────────
            var spacer = new Panel
            {
                Height = 16, // 👈 adjust for more/less space
                Dock = DockStyle.Top,
                BackColor = BgDark
            };

            // ── Financial health card ────────────────────────────────
            var healthCard = BuildHealthCard();

            // Add in REVERSE order
            scroll.Controls.Add(healthCard); // index 0 = bottom
            scroll.Controls.Add(spacer);     // index 1 = middle
            scroll.Controls.Add(row1);       // index 2 = top
        }

        // ═══════════════════════════════════════════════════════════
        //  DONUT CARD
        // ═══════════════════════════════════════════════════════════
        private Panel BuildDonutCard()
        {
            var card = MakeCard();
            card.Padding = new Padding(16);
            card.Controls.Add(MakeSectionLabel("SPENDING BY CATEGORY"));

            // Use CardBg (not Transparent) so WinForms paints correctly
            donutPanel = new Panel
            {
                Width = 175,
                Height = 170,
                BackColor = CardBg,
                Location = new Point(30, 48)
            };
            donutPanel.Paint += PaintDonut;
            card.Controls.Add(donutPanel);

            legendPanel = new Panel
            {
                BackColor = CardBg,
                Location = new Point(220, 48),
                Size = new Size(160, 220)
            };
            card.Controls.Add(legendPanel);

            // Reposition on resize
            card.Resize += (s, e) =>
            {
                int cy = Math.Max(48, (card.Height - donutPanel.Height) / 2);
                donutPanel.Location = new Point(12, cy);

                int lx = donutPanel.Right + 16;
                legendPanel.Location = new Point(lx, 48);
                legendPanel.Size = new Size(Math.Max(10, card.Width - lx - 12),
                                                card.Height - 58);
            };

            return card;
        }

        private void PaintDonut(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Fill background explicitly (fixes transparency rendering)
            g.Clear(CardBg);

            int pad = 14;
            var rect = new Rectangle(pad, pad,
                             donutPanel.Width - pad * 2,
                             donutPanel.Height - pad * 2);
            float ring = 30f;

            double total = 0;
            foreach (var b in _budgets) total += b.Spent;

            // ── Empty state ──────────────────────────────────────────
            if (total == 0)
            {
                using var ep = new Pen(Color.FromArgb(45, 55, 72), ring);
                g.DrawEllipse(ep, rect);
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString("No spending\nthis month",
                    new Font("Segoe UI", 8f), new SolidBrush(SubText),
                    new RectangleF(0, 0, donutPanel.Width, donutPanel.Height), sf);
                return;
            }

            // ── Segments ─────────────────────────────────────────────
            float start = -90f;
            float gap = 3f;

            foreach (var b in _budgets)
            {
                if (b.Spent <= 0) continue;
                float sweep = (float)(b.Spent / total * 360.0) - gap;
                if (sweep < 1f) { start += sweep + gap; continue; }

                using var pen = new Pen(b.CategoryColor, ring)
                {
                    StartCap = LineCap.Flat,
                    EndCap = LineCap.Flat
                };
                g.DrawArc(pen, rect, start, sweep);
                start += sweep + gap;
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  LEGEND
        // ═══════════════════════════════════════════════════════════
        private void RefreshDonutLegend()
        {
            legendPanel.Controls.Clear();

            double total = 0;
            foreach (var b in _budgets) total += b.Spent;
            if (total == 0) total = 1;

            int iy = 8;
            foreach (var b in _budgets)
            {
                if (b.Spent <= 0) continue;
                int pct = (int)Math.Round(b.Spent / total * 100);
                Color dotColor = b.CategoryColor;

                // dot
                var dot = new Panel
                {
                    Width = 10,
                    Height = 10,
                    BackColor = dotColor,
                    Location = new Point(0, iy + 5)
                };
                legendPanel.Controls.Add(dot);

                // name
                legendPanel.Controls.Add(new Label
                {
                    Text = b.Name,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(18, iy + 2)
                });

                // pct — right side
                var lblPct = new Label
                {
                    Text = $"{pct}%",
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = SubText,
                    AutoSize = true,
                    Location = new Point(130, iy + 2)
                };
                legendPanel.Controls.Add(lblPct);

                iy += 28;
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  BAR CHART CARD
        // ═══════════════════════════════════════════════════════════
        private Panel BuildBarChartCard()
        {
            var card = MakeCard();
            card.Padding = new Padding(16);
            card.Margin = new Padding(16, 0, 0, 0);
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

            barChartPanel = new Panel
            {
                BackColor = Color.Transparent,
                Location = new Point(16, 55),
                Size = new Size(400, 200)
            };
            barChartPanel.Paint += PaintBarChart;
            card.Controls.Add(barChartPanel);

            AddLegendDot(card, GreenAmt, "Income", 16, 10);
            AddLegendDot(card, RedAmt, "Expenses", 90, 10);

            // 👇 single resize event handling everything
            card.Resize += (s, e) =>
            {
                btnRow.Location = new Point(card.Width - btnRow.Width - 16, 10);
                barChartPanel.Size = new Size(card.Width - 32, card.Height - 80);
                foreach (Control c in card.Controls)
                    if (c.Tag is string t && t == "legend")
                        c.Top = card.Height - 26;
            };

            HighlightPeriodButton();
            return card;
        }

        private void SetPeriod(ChartPeriod p)
        {
            _period = p;
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
            int userId = SessionManager.UserId;
            AnalyticsChartPoint[] data = _period switch
            {
                ChartPeriod.Weekly => AnalyticsRepository.GetWeeklyChart(userId),
                ChartPeriod.Daily => AnalyticsRepository.GetDailyChart(userId),
                _ => AnalyticsRepository.GetMonthlyChart(userId)
            };

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var panel = (Panel)sender;
            int w = panel.Width, h = panel.Height - 40;
            if (w < 20 || h < 20) return;

            double maxVal = 1;
            foreach (var pt in data) maxVal = Math.Max(maxVal, Math.Max(pt.Income, pt.Expense));
            maxVal *= 1.35;

            using var gridPen = new Pen(Color.FromArgb(40, 50, 68), 1f);
            for (int i = 0; i <= 4; i++)
            {
                int y = (int)(h * (1 - i / 4.0));
                double v = maxVal * i / 3;
                string lbl = v >= 1000 ? $"₱{(int)(v / 1000)}k" : $"₱{(int)v}";
                g.DrawLine(gridPen, 44, y, w, y);
                using var sf = new StringFormat { Alignment = StringAlignment.Far };
                g.DrawString(lbl, new Font("Segoe UI", 7f),
                    new SolidBrush(SubText), new RectangleF(0, y - 8, 42, 16), sf);
            }

            int n = data.Length;
            float slotW = (w - 44f) / n;
            float barW = slotW * 0.28f;
            float gap2 = slotW * 0.05f;

            for (int i = 0; i < n; i++)
            {
                float slotX = 44 + i * slotW + slotW * 0.08f;
                float incH = (float)(h * data[i].Income / maxVal);
                float expH = (float)(h * data[i].Expense / maxVal);

                if (incH > 1) { using var b = new SolidBrush(GreenAmt); g.FillRectangle(b, slotX, h - incH, barW, incH); }
                if (expH > 1) { using var b = new SolidBrush(RedAmt); g.FillRectangle(b, slotX + barW + gap2, h - expH, barW, expH); }

                using var sfC = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString(data[i].Label, new Font("Segoe UI", 7f),
                    new SolidBrush(SubText),
                    new RectangleF(slotX - gap2, h + 4, slotW * 0.9f, 20), sfC);
            }
        }

        private void AddLegendDot(Panel card, Color col, string text, int x, int y)
        {
            card.Controls.Add(new Panel { Width = 10, Height = 10, BackColor = col, Location = new Point(x, y), Tag = "legend" });
            card.Controls.Add(new Label { Text = text, Font = new Font("Segoe UI", 8f), ForeColor = Color.White, AutoSize = true, Location = new Point(x + 14, y - 1), Tag = "legend" });
        }

        // ═══════════════════════════════════════════════════════════
        //  FINANCIAL HEALTH CARD
        // ═══════════════════════════════════════════════════════════
        private Panel BuildHealthCard()
        {
            var card = MakeCard();
            card.Height = 320;
            card.Padding = new Padding(0);
            card.Controls.Add(MakeSectionLabel("FINANCIAL HEALTH RATING"));

            gaugePanel = new Panel
            {
                Width = 170,
                Height = 170,
                BackColor = Color.Transparent,
                Location = new Point(10, 30),
                Tag = 0
            };
            gaugePanel.Paint += (s, e) =>
                PaintGauge(e.Graphics, gaugePanel, (int)(gaugePanel.Tag ?? 0));
            card.Controls.Add(gaugePanel);
            gaugePanel.SendToBack();

            lblHealthLabel = new Label
            {
                Text = "—",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Accent,
                AutoSize = true,
                Location = new Point(80, 195)
            };
            card.Controls.Add(lblHealthLabel);

            // ── Stats now sit to the RIGHT of the gauge ──
            lblSavingsRate = MakeBigStat("0%");
            lblDebtRatio = MakeBigStat("0%");
            lblBudgetKept = MakeBigStat("0%");
            lblSavingsCaption = MakeSmallCaption("SAVINGS RATE");
            lblDebtCaption = MakeSmallCaption("DEBT PAID");
            lblBudgetCaption = MakeSmallCaption("BUDGET KEPT");

            // X starts just after gauge (10 + 170 = 180), spaced in three columns
            PlaceStatPair(card, lblSavingsRate, lblSavingsCaption, 220, 85);
            PlaceStatPair(card, lblDebtRatio, lblDebtCaption, 310, 85);
            PlaceStatPair(card, lblBudgetKept, lblBudgetCaption, 395, 85);

            // ── Right-side text block pushed further right ──
            int rx = 580;  // 👈 was 230, now 410

            lblHealthTitle = new Label
            {
                Text = "Add budgets, goals & debts to see your score.",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(320, 44),
                Location = new Point(rx, 40)
            };
            card.Controls.Add(lblHealthTitle);

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
                Size = new Size(320, 44),
                Location = new Point(rx, 108)
            });

            BuildInsightBullets(card, rx, 158);
            return card;
        }

        private void PaintGauge(Graphics g, Panel panel, int score)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(12, 12, panel.Width - 24, panel.Height - 24);

            using var trackPen = new Pen(Color.FromArgb(45, 52, 70), 16f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            g.DrawArc(trackPen, rect, 135, 270);

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

        private void PlaceStatPair(Panel card, Label value, Label caption, int x, int y)
        {
            value.Location = new Point(x, y);
            caption.Location = new Point(x, y + 30);
            caption.Size = new Size(95, 20);
            card.Controls.Add(value);
            card.Controls.Add(caption);
        }

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

        // ═══════════════════════════════════════════════════════════
        //  REFRESH — all data from database, zero AppData usage
        // ═══════════════════════════════════════════════════════════
        private void RefreshAll()
        {
            int userId = SessionManager.UserId;

            // ── Donut ────────────────────────────────────────────────
            _budgets = AnalyticsRepository.GetBudgets(userId);
            donutPanel.Invalidate();
            RefreshDonutLegend();


            // ── Bar chart ────────────────────────────────────────────
            barChartPanel.Invalidate();

            // ── Financial Health ─────────────────────────────────────
            var health = AnalyticsRepository.GetHealthData(userId);
            int score = health.HealthScore;

            gaugePanel.Tag = score;
            gaugePanel.Invalidate();

            Color labelColor = score >= 80 ? GreenAmt
                             : score >= 60 ? Accent
                             : score >= 40 ? Color.FromArgb(240, 140, 60)
                             : RedAmt;

            lblHealthLabel.ForeColor = labelColor;
            lblHealthLabel.Text = health.HealthLabel;

            lblHealthTitle.Text = score >= 60
                ? "Your financial health is on track."
                : score >= 40
                    ? "Your financial health needs improvement."
                    : "Your financial health needs urgent attention.";

            lblScoreHighlight.Text = $"{score}/100";
            lblBudgetKept.Text = $"{health.BudgetKeptPct:F1}%";
            lblSavingsRate.Text = $"{health.SavingsRatePct:F1}%";
            lblDebtRatio.Text = $"{health.DebtPaidPct:F1}%";

            if (_insightLabels != null)
            {
                double usage = health.BudgetUsagePct;
                double debt = health.DebtPaidPct;
                double savings = health.SavingsRatePct;

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
        }

        // ═══════════════════════════════════════════════════════════
        //  HELPERS
        // ═══════════════════════════════════════════════════════════
        private Label MakeBigStat(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 13f, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true
        };

        private Label MakeSmallCaption(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 7f, FontStyle.Bold),
            ForeColor = SubText,
            AutoSize = false,
            Size = new Size(90, 16),
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
                BackColor = Color.FromArgb(245, 166, 35),
                ForeColor = Color.FromArgb(20, 20, 20),
                FlatStyle = FlatStyle.Flat,
                Height = 32,
                Width = width,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
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

        private void InitializeComponent()
        {
            ResumeLayout(false);

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