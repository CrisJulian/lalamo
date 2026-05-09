// AnalyticsRepository.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace AOOP_PROJECTT
{
    public class AnalyticsBudgetItem
    {
        public string Name { get; set; }
        public double Spent { get; set; }
        public double LimitAmount { get; set; }
        public Color CategoryColor { get; set; }
        public double Percent => LimitAmount > 0 ? Math.Min(100, Spent / LimitAmount * 100) : 0;
    }

    public class AnalyticsChartPoint
    {
        public string Label { get; set; }
        public double Income { get; set; }
        public double Expense { get; set; }
    }

    public class AnalyticsHealthData
    {
        public double TotalBudgetLimit { get; set; }
        public double TotalBudgetSpent { get; set; }
        public double TotalGoalTarget { get; set; }
        public double TotalGoalSaved { get; set; }
        public double TotalDebtOriginal { get; set; }
        public double TotalDebtRemaining { get; set; }

        public double BudgetUsagePct => TotalBudgetLimit > 0 ? Math.Min(100, TotalBudgetSpent / TotalBudgetLimit * 100) : 0;
        public double BudgetKeptPct => TotalBudgetLimit > 0 ? Math.Min(100, (1 - TotalBudgetSpent / TotalBudgetLimit) * 100) : 0;
        public double SavingsRatePct => TotalGoalTarget > 0 ? Math.Min(100, TotalGoalSaved / TotalGoalTarget * 100) : 0;
        public double DebtPaidPct => TotalDebtOriginal > 0 ? Math.Min(100, (1 - TotalDebtRemaining / TotalDebtOriginal) * 100) : 0;

        public int HealthScore
        {
            get
            {
                bool hasBudget = TotalBudgetLimit > 0;
                bool hasGoal = TotalGoalTarget > 0;
                bool hasDebt = TotalDebtOriginal > 0;
                if (!hasBudget && !hasGoal && !hasDebt) return 0;

                double score = 0; int parts = 0;
                if (hasBudget) { score += BudgetKeptPct; parts++; }
                if (hasGoal) { score += SavingsRatePct; parts++; }
                if (hasDebt) { score += DebtPaidPct; parts++; }
                return (int)(score / parts);
            }
        }

        public string HealthLabel => HealthScore >= 80 ? "Excellent"
                                   : HealthScore >= 60 ? "Good"
                                   : HealthScore >= 40 ? "Fair"
                                   : "Poor";
    }

    public static class AnalyticsRepository
    {
        public static List<AnalyticsBudgetItem> GetBudgets(int userId)
        {
            var list = new List<AnalyticsBudgetItem>();
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = @"
                    SELECT Name,
                           ISNULL(Spent, 0),
                           LimitAmount,
                           ISNULL(CategoryColor, '#F5A623')
                    FROM   Budgets
                    WHERE  UserId = @userId
                      AND  Month  = @month
                      AND  Year   = @year";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@month", DateTime.Now.Month);
                cmd.Parameters.AddWithValue("@year", DateTime.Now.Year);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    list.Add(new AnalyticsBudgetItem
                    {
                        Name = r.GetString(0),
                        Spent = Convert.ToDouble(r.GetValue(1)),   // ← changed
                        LimitAmount = Convert.ToDouble(r.GetValue(2)),
                        CategoryColor = ParseColor(r.GetString(3))
                    });
            }
            catch (Exception ex) { ShowError("budgets", ex); }
            return list;
        }

        public static AnalyticsChartPoint[] GetMonthlyChart(int userId)
        {
            var results = new AnalyticsChartPoint[6];
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = @"
                    SELECT MONTH([Date]), YEAR([Date]),
                           SUM(CASE WHEN [Type]='Income'  THEN Amount ELSE 0 END),
                           SUM(CASE WHEN [Type]='Expense' THEN Amount ELSE 0 END)
                    FROM   Transactions
                    WHERE  UserId = @userId
                      AND  [Date] >= DATEADD(MONTH, -5, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))
                    GROUP BY MONTH([Date]), YEAR([Date])
                    ORDER BY YEAR([Date]), MONTH([Date])";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                using var r = cmd.ExecuteReader();
                var raw = new List<(int m, int y, double inc, double exp)>();
                while (r.Read())
                    raw.Add((r.GetInt32(0), r.GetInt32(1), (double)r.GetDecimal(2), (double)r.GetDecimal(3)));

                for (int i = 0; i < 6; i++)
                {
                    var dt = DateTime.Now.AddMonths(-(5 - i));
                    var match = raw.Find(x => x.m == dt.Month && x.y == dt.Year);
                    results[i] = new AnalyticsChartPoint { Label = dt.ToString("MMM"), Income = match.inc, Expense = match.exp };
                }
            }
            catch (Exception ex) { ShowError("monthly chart", ex); }
            return results;
        }

        public static AnalyticsChartPoint[] GetWeeklyChart(int userId)
        {
            var results = new AnalyticsChartPoint[6];
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = @"
                    SELECT DATEPART(ISO_WEEK, [Date]), YEAR([Date]),
                           SUM(CASE WHEN [Type]='Income'  THEN Amount ELSE 0 END),
                           SUM(CASE WHEN [Type]='Expense' THEN Amount ELSE 0 END)
                    FROM   Transactions
                    WHERE  UserId = @userId
                      AND  [Date] >= DATEADD(WEEK, -5, CAST(GETDATE() AS DATE))
                    GROUP BY DATEPART(ISO_WEEK, [Date]), YEAR([Date])
                    ORDER BY YEAR([Date]), DATEPART(ISO_WEEK, [Date])";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                using var r = cmd.ExecuteReader();
                var raw = new List<(int w, int y, double inc, double exp)>();
                while (r.Read())
                    raw.Add((r.GetInt32(0), r.GetInt32(1), (double)r.GetDecimal(2), (double)r.GetDecimal(3)));

                var cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;
                for (int i = 0; i < 6; i++)
                {
                    var dt = DateTime.Now.AddDays(-(5 - i) * 7);
                    int wk = cal.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    var match = raw.Find(x => x.w == wk && x.y == dt.Year);
                    results[i] = new AnalyticsChartPoint { Label = $"W{wk}", Income = match.inc, Expense = match.exp };
                }
            }
            catch (Exception ex) { ShowError("weekly chart", ex); }
            return results;
        }

        public static AnalyticsChartPoint[] GetDailyChart(int userId)
        {
            var results = new AnalyticsChartPoint[7];
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = @"
                    SELECT CAST([Date] AS DATE),
                           SUM(CASE WHEN [Type]='Income'  THEN Amount ELSE 0 END),
                           SUM(CASE WHEN [Type]='Expense' THEN Amount ELSE 0 END)
                    FROM   Transactions
                    WHERE  UserId = @userId
                      AND  [Date] >= DATEADD(DAY, -6, CAST(GETDATE() AS DATE))
                    GROUP BY CAST([Date] AS DATE)
                    ORDER BY CAST([Date] AS DATE)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                using var r = cmd.ExecuteReader();
                var raw = new List<(DateTime d, double inc, double exp)>();
                while (r.Read())
                    raw.Add((r.GetDateTime(0), (double)r.GetDecimal(1), (double)r.GetDecimal(2)));

                for (int i = 0; i < 7; i++)
                {
                    var dt = DateTime.Today.AddDays(-(6 - i));
                    var match = raw.Find(x => x.d.Date == dt);
                    results[i] = new AnalyticsChartPoint { Label = dt.ToString("ddd"), Income = match.inc, Expense = match.exp };
                }
            }
            catch (Exception ex) { ShowError("daily chart", ex); }
            return results;
        }

        public static AnalyticsHealthData GetHealthData(int userId)
        {
            var data = new AnalyticsHealthData();
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                // Budgets
                using (var cmd = new SqlCommand(@"
                    SELECT ISNULL(SUM(LimitAmount), 0), ISNULL(SUM(Spent), 0)
                    FROM Budgets WHERE UserId = @userId AND Month = @month AND Year = @year", conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@month", DateTime.Now.Month);
                    cmd.Parameters.AddWithValue("@year", DateTime.Now.Year);
                    using var r = cmd.ExecuteReader();
                    if (r.Read()) { data.TotalBudgetLimit = (double)r.GetDecimal(0); data.TotalBudgetSpent = (double)r.GetDecimal(1); }
                }

                // Goals
                using (var cmd = new SqlCommand(@"
                    SELECT ISNULL(SUM(Target), 0), ISNULL(SUM(Saved), 0)
                    FROM SavingsGoals WHERE UserId = @userId", conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using var r = cmd.ExecuteReader();
                    if (r.Read()) { data.TotalGoalTarget = (double)r.GetDecimal(0); data.TotalGoalSaved = (double)r.GetDecimal(1); }
                }

                // Debts — Principal = original, AmountPaid = paid so far
                using (var cmd = new SqlCommand(@"
                    SELECT ISNULL(SUM(Principal), 0), ISNULL(SUM(AmountPaid), 0)
                    FROM Debts WHERE UserId = @userId", conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using var r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        data.TotalDebtOriginal = (double)r.GetDecimal(0);
                        data.TotalDebtRemaining = (double)r.GetDecimal(0) - (double)r.GetDecimal(1);
                    }
                }
            }
            catch (Exception ex) { ShowError("health data", ex); }
            return data;
        }

        private static Color ParseColor(string hex)
        {
            try { return ColorTranslator.FromHtml(hex); }
            catch { return Color.FromArgb(245, 166, 35); }
        }

        private static void ShowError(string section, Exception ex) =>
            MessageBox.Show($"Error loading {section}: {ex.Message}", "DB Error");
    }
}