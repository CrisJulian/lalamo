// DebtsTab.cs — UPDATED
// Changes from original:
//   1. Constructor calls LoadFromDatabase() instead of just RefreshAll()
//   2. BtnAddDebt_Click saves to DB then refreshes
//   3. OpenLogPayment saves payment to DB (both DebtPayments log + Debts update)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AOOP_PROJECTT;

namespace CommonCents
{
    public class DebtsTab : UserControl
    {
        private List<Debt> _debts = new List<Debt>();

        private Label lblTotalDebt, lblTotalPaid, lblRemaining;
        private Panel pnlDebtList;

        private static readonly Color BgDark     = Color.FromArgb(15, 17, 23);
        private static readonly Color CardBg     = Color.FromArgb(24, 28, 38);
        private static readonly Color Accent     = Color.FromArgb(245, 166, 35);
        private static readonly Color GreenAmt   = Color.FromArgb(79, 255, 176);
        private static readonly Color RedAmt     = Color.FromArgb(255, 107, 107);
        private static readonly Color SubText    = Color.FromArgb(130, 145, 170);

        public DebtsTab()
        {
            BackColor = BgDark;
            Dock      = DockStyle.Fill;
            Padding   = new Padding(0);
            BuildUI();
            LoadFromDatabase(); // Load real data from DB on startup
        }

        // ── DB LOAD ────────────────────────────────────────────────────
        private void LoadFromDatabase()
        {
            int userId = SessionManager.UserId;
            if (userId == 0) return;

            _debts = DebtRepository.GetDebts(userId);
            RefreshAll();
        }

        // ── UI BUILD ───────────────────────────────────────────────────
        private void BuildUI()
        {
            // ── Top bar ─────────────────────────────────────────────
            var topBar = new Panel { Height = 55, Dock = DockStyle.Top, BackColor = BgDark };
            var lblTitle = new Label
            {
                Text = "Debts",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 13)
            };
            var lblDate = new Label
            {
                Text = DateTime.Now.ToString("ddd, MMM d, yyyy"),
                Font = new Font("Segoe UI", 9f),
                ForeColor = SubText,
                AutoSize = true
            };
            var btnAddTx = MakeYellowButton("+ Add Transaction", 140);
            btnAddTx.Height = 32;
            btnAddTx.Click += (s, e) => Form1.OpenAddTransaction(ParentForm, null);
            btnAddTx.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            topBar.Controls.Add(lblTitle);
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

            // ── Outer scroll ─────────────────────────────────────────
            var outer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = BgDark,
                Padding = new Padding(18)
            };

            // Add in REVERSE order
            Controls.Add(outer);
            Controls.Add(separator);
            Controls.Add(topBar);

            // ── Summary row ──────────────────────────────────────────
            var summaryRow = new TableLayoutPanel
            {
                Height = 130,
                Dock = DockStyle.Top,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = BgDark,
                Margin = new Padding(0, 8, 0, 0)
            };
            summaryRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            summaryRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            summaryRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.4f));

            lblTotalDebt = new Label { ForeColor = RedAmt };
            lblTotalPaid = new Label { ForeColor = GreenAmt };
            lblRemaining = new Label { ForeColor = Accent };

            summaryRow.Controls.Add(MakeSummaryCard("TOTAL DEBT", lblTotalDebt), 0, 0);
            summaryRow.Controls.Add(MakeSummaryCard("TOTAL PAID", lblTotalPaid), 1, 0);
            summaryRow.Controls.Add(MakeSummaryCard("REMAINING", lblRemaining), 2, 0);
            outer.Controls.Add(summaryRow);

            // ── Active debts bar ─────────────────────────────────────
            var activeBar = new Panel { Height = 60, Dock = DockStyle.Top, BackColor = BgDark };
            var lblActive = new Label
            {
                Text = "ACTIVE DEBTS",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = SubText,
                AutoSize = true,
                Location = new Point(0, 35)
            };
            var btnAddDebt = MakeYellowButton("+ Add Debt", 110);
            btnAddDebt.Click += BtnAddDebt_Click;
            activeBar.Controls.Add(lblActive);
            activeBar.Controls.Add(btnAddDebt);
            activeBar.Resize += (s, e) =>
                btnAddDebt.Location = new Point(activeBar.Width - btnAddDebt.Width - 2, 20);
            outer.Controls.Add(activeBar);

            pnlDebtList = new Panel
            {
                Dock = DockStyle.Top,
                BackColor = BgDark,
                Padding = new Padding(0, 4, 0, 0)
            };
            outer.Controls.Add(pnlDebtList);

            outer.Controls.SetChildIndex(pnlDebtList, 0);
            outer.Controls.SetChildIndex(activeBar, 1);
            outer.Controls.SetChildIndex(summaryRow, 2);
        }

        // ── REFRESH ────────────────────────────────────────────────────
        private void RefreshAll()
        {
            UpdateSummary();
            RebuildDebtCards();
        }

        private void UpdateSummary()
        {
            double total = 0, paid = 0;
            foreach (var d in _debts) { total += d.TotalDebt; paid += d.AmountPaid; }

            lblTotalDebt.Text = $"₱{total:N0}";
            lblTotalPaid.Text = $"₱{paid:N0}";
            lblRemaining.Text = $"₱{total - paid:N0}";
        }

        private void RebuildDebtCards()
        {
            pnlDebtList.Controls.Clear();

            var cards = new List<Panel>();
            foreach (var debt in _debts)
                cards.Add(BuildDebtCard(debt));

            for (int i = cards.Count - 1; i >= 0; i--)
            {
                // Add a spacer before each card (except the last one added, which is the first card)
                if (i < cards.Count - 1)
                {
                    var spacer = new Panel
                    {
                        Height = 12,
                        Dock = DockStyle.Top,
                        BackColor = BgDark
                    };
                    pnlDebtList.Controls.Add(spacer);
                }
                pnlDebtList.Controls.Add(cards[i]);
            }

            pnlDebtList.Height = cards.Count * 170 + (cards.Count - 1) * 12 + 10;
        }

        // ── DEBT CARD (unchanged visually) ─────────────────────────────
        private Panel BuildDebtCard(Debt debt)
        {
            var card = new Panel
            {
                Height    = 170,
                Dock      = DockStyle.Top,
                BackColor = CardBg,
                Margin    = new Padding(0, 0, 0, 20),
                Padding   = new Padding(16, 12, 16, 12)
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen  = new Pen(Color.FromArgb(50, 60, 80), 1.5f);
                using var path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 10);
                e.Graphics.DrawPath(pen, path);
            };

            var iconBox = new Panel { Width = 38, Height = 38, Location = new Point(16, 14), BackColor = Color.Transparent };
            iconBox.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(Color.FromArgb(200, 60, 60));
                e.Graphics.FillEllipse(brush, 0, 0, iconBox.Width - 1, iconBox.Height - 1);
                using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString("💳", new Font("Segoe UI Emoji", 14f), Brushes.White,
                    new RectangleF(0, 0, iconBox.Width, iconBox.Height), sf);
            };
            card.Controls.Add(iconBox);

            card.Controls.Add(new Label
            {
                Text      = debt.Name,
                Font      = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize  = true,
                Location  = new Point(62, 14)
            });
            card.Controls.Add(new Label
            {
                Text      = $"{debt.InterestRate}% interest rate  •  Due {debt.DueDate:MMM dd, yyyy}",
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = SubText,
                AutoSize  = true,
                Location  = new Point(63, 36)
            });

            var btnLog = new Button
            {
                Text = "Log Payment",
                Height = 32,
                Width = 108,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(230, 170, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLog.FlatAppearance.BorderColor = Color.FromArgb(230, 170, 0);
            btnLog.FlatAppearance.BorderSize = 1;
            btnLog.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 230, 170, 0);
            btnLog.Click += (s, e) => OpenLogPayment(debt);
            card.Controls.Add(btnLog);
            card.Resize += (s, e) => btnLog.Location = new Point(card.Width - 126, 12);

            var btnDelete = MakeDeleteButton();
            card.Controls.Add(btnDelete);
            card.Resize += (s, e) => btnDelete.Location = new Point(card.Width - btnDelete.Width - 14, 48);
            btnDelete.Location = new Point(card.Width - btnDelete.Width - 14, 48);
            btnDelete.Click += (s, e) =>
            {
                DebtRepository.DeleteDebt(debt.DebtId);
                _debts.Remove(debt);
                RefreshAll();
            };

            int statsY = 62;
            AddStatCol(card, "PRINCIPAL",       $"₱{debt.Principal:N0}",      Color.White, 16,  statsY);
            AddStatCol(card, "AMOUNT PAID",     $"₱{debt.AmountPaid:N0}",     GreenAmt,    170, statsY);
            AddStatCol(card, "REMAINING",       $"₱{debt.Remaining:N0}",      RedAmt,      310, statsY);
            AddStatCol(card, "MONTHLY PAYMENT", $"₱{debt.MonthlyPayment:N0}", Color.White, 450, statsY);

            int progY = 108;
            card.Controls.Add(new Label
            {
                Text      = "Repayment Progress",
                Font      = new Font("Segoe UI", 8f),
                ForeColor = SubText,
                AutoSize  = true,
                Location  = new Point(16, progY)
            });

            var lblPct = new Label
            {
                Text      = $"{(int)debt.ProgressPercent}% paid",
                Font      = new Font("Segoe UI", 8f),
                ForeColor = debt.IsPaidOff ? GreenAmt : Accent,
                AutoSize  = true
            };
            card.Controls.Add(lblPct);
            card.Resize += (s, e) => lblPct.Location = new Point(card.Width - lblPct.Width - 16, progY);

            var track = new Panel
            {
                Height    = 8,
                BackColor = Color.FromArgb(45, 52, 70),
                Location  = new Point(16, progY + 18)
            };
            card.Controls.Add(track);
            card.Resize += (s, e) =>
            {
                track.Width = card.Width - 32;
                RefreshFill(track, debt);
            };
            RefreshFill(track, debt);

            if (debt.PaidThisMonth)
                card.Controls.Add(new Label
                {
                    Text      = "✓ Monthly Paid",
                    Font      = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                    ForeColor = GreenAmt,
                    AutoSize  = true,
                    Location  = new Point(14, progY + 36)
                });

            return card;
        }

        private void RefreshFill(Panel track, Debt debt)
        {
            track.Controls.Clear();
            int fillW = (int)(track.Width * debt.ProgressPercent / 100.0);
            if (fillW < 1) return;
            track.Controls.Add(new Panel
            {
                Location  = new Point(0, 0),
                Height    = track.Height,
                Width     = fillW,
                BackColor = Accent
            });
        }

        private void AddStatCol(Panel card, string header, string value, Color valColor, int x, int y)
        {
            card.Controls.Add(new Label
            {
                Text      = header,
                Font      = new Font("Segoe UI", 7f, FontStyle.Bold),
                ForeColor = SubText,
                AutoSize  = true,
                Location  = new Point(x, y)
            });
            card.Controls.Add(new Label
            {
                Text      = value,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = valColor,
                AutoSize  = true,
                Location  = new Point(x, y + 16)
            });
        }

        private Panel MakeSummaryCard(string title, Label valueLabel)
        {
            var card = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = CardBg,
                Margin    = new Padding(0, 0, 8, 0),
                Padding   = new Padding(16, 12, 16, 12)
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen  = new Pen(Color.FromArgb(50, 60, 80), 1f);
                using var path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 8);
                e.Graphics.DrawPath(pen, path);
            };
            card.Controls.Add(new Label
            {
                Text      = title,
                Font      = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = SubText,
                AutoSize  = true,
                Location  = new Point(16, 14)
            });
            valueLabel.Font     = new Font("Segoe UI", 20f, FontStyle.Bold);
            valueLabel.AutoSize = true;
            valueLabel.Location = new Point(14, 36);
            valueLabel.Text     = "₱0";
            card.Controls.Add(valueLabel);
            return card;
        }

        // ── ACTIONS — now save to DB ────────────────────────────────────
        private void BtnAddDebt_Click(object sender, EventArgs e)
        {
            using var form = new AddDebtForm();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                var debt = new Debt(
                    form.DebtName, form.Principal,
                    form.InterestRate, form.MonthlyPayment, form.DueDate);

                // Save to DB and get the new DebtId back
                int newId = DebtRepository.AddDebt(SessionManager.UserId, debt);
                if (newId > 0)
                {
                    debt.DebtId = newId;
                    _debts.Add(debt);
                    RefreshAll();
                }
            }
        }

        private void OpenLogPayment(Debt debt)
        {
            using var form = new LogPaymentForm(debt);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                bool isMonthly = Math.Abs(form.PaymentAmount - debt.MonthlyPayment) < 0.01
                                 && !debt.PaidThisMonth;
                if (isMonthly)
                    debt.PayMonthly();
                else
                    debt.LogPayment(form.PaymentAmount);

                // 1. Log the individual payment in DebtPayments history
                DebtRepository.LogPayment(debt.DebtId, form.PaymentAmount);

                // 2. Update the debt's running total in the Debts table
                DebtRepository.UpdateDebtPayment(debt.DebtId, debt.AmountPaid, debt.PaidThisMonth);

                RefreshAll();
            }
        }

        // ── UTILITIES ─────────────────────────────────────────────────
        private Button MakeYellowButton(string text, int width)
        {
            var b = new Button
            {
                Text      = text,
                BackColor = Accent,
                ForeColor = Color.FromArgb(20, 20, 20),
                FlatStyle = FlatStyle.Flat,
                Height    = 32,
                Width     = width,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            b.FlatAppearance.BorderSize         = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 200, 30);
            return b;
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d    = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X,         bounds.Y,          d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y,          d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0,   90);
            path.AddArc(bounds.X,         bounds.Bottom - d, d, d, 90,  90);
            path.CloseFigure();
            return path;
        }

        private Panel MakeDeleteButton()
        {
            var circle = new Panel
            {
                Width = 28,
                Height = 28,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            var lbl = new Label
            {
                Text = "X",
                Font = new Font("Segoe UI", 6.5f, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = false,
                Size = new Size(28, 28),
                Location = new Point(0, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Enabled = false
            };

            circle.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(Color.FromArgb(200, 60, 60));
                e.Graphics.FillEllipse(brush, 1, 1, circle.Width - 3, circle.Height - 3);
            };

            circle.Controls.Add(lbl);
            return circle;
        }
    }
}
