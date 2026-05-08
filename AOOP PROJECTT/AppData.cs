// AppData.cs
// Central data store — all tabs read and write here.
using System;
using System.Collections.Generic;

namespace CommonCents
{
    public static class AppData
    {
        public static List<Debt>        Debts   { get; } = new List<Debt>();
        public static List<Budget>      Budgets { get; } = new List<Budget>();
        public static List<SavingsGoal> Goals   { get; } = new List<SavingsGoal>();

        // Transaction record (used by income/expense chart)
        public static List<Transaction> Transactions { get; } = new List<Transaction>();

        //  FINANCIAL HEALTH METRICS
        //  Formula: (total spent / total limit) * 100 = usage %
        //  Budget Kept = 100 - usage  (lower spending = better)
        //  Savings Rate = total saved / total target * 100
        //  Debt Paid   = total paid  / total debt   * 100


        /// Total budget limit across all categories
        public static double TotalBudgetLimit
        {
            get
            {
                double t = 0;
                foreach (var b in Budgets) t += b.Limit;
                return t;
            }
        }

        /// Total amount spent across all budget categories
        public static double TotalBudgetSpent
        {
            get
            {
                double t = 0;
                foreach (var b in Budgets) t += b.Spent;
                return t;
            }
        }

        /// (total spent / total limit) * 100  — how much of the budget was used
        public static double BudgetUsagePct =>
            TotalBudgetLimit > 0
                ? Math.Min(100, TotalBudgetSpent / TotalBudgetLimit * 100)
                : 0;

        /// 100 - BudgetUsagePct  — how much of the budget was kept/saved
        public static double BudgetKeptPct => Math.Max(0, 100 - BudgetUsagePct);

        /// (total saved / total target) * 100
        public static double SavingsRatePct
        {
            get
            {
                double totalTarget = 0, totalSaved = 0;
                foreach (var g in Goals) { totalTarget += g.Target; totalSaved += g.Saved; }
                return totalTarget > 0
                    ? Math.Min(100, totalSaved / totalTarget * 100)
                    : 0;
            }
        }

        /// (total paid / total debt) * 100
        public static double DebtPaidPct
        {
            get
            {
                double totalDebt = 0, totalPaid = 0;
                foreach (var d in Debts) { totalDebt += d.TotalDebt; totalPaid += d.AmountPaid; }
                return totalDebt > 0
                    ? Math.Min(100, totalPaid / totalDebt * 100)
                    : 0;
            }
        }

        /// Overall health score: average of the three metrics
        public static int HealthScore =>
            (int)((BudgetKeptPct + SavingsRatePct + DebtPaidPct) / 3.0);

        public static string HealthLabel
        {
            get
            {
                int s = HealthScore;
                if (s >= 80) return "Excellent";
                if (s >= 60) return "Good";
                if (s >= 40) return "Fair";
                return "Needs Work";
            }
        }


        //  CHART DATA HELPERS
        //  Returns income/expense totals grouped by period

        /// Groups transactions into the last N periods and returns
        /// (label, totalIncome, totalExpense) for each.
        public static (string Label, double Income, double Expense)[]
            GetChartData(ChartPeriod period)
        {
            int slots;
            Func<DateTime, string> labelFn;
            Func<DateTime, DateTime, bool> inSlot;
            Func<int, DateTime> slotStart;

            var now = DateTime.Today;

            switch (period)
            {
                case ChartPeriod.Weekly:
                    // last 6 weeks
                    slots = 6;
                    slotStart = i => now.AddDays(-7 * (slots - 1 - i));
                    labelFn  = d => "Wk " + GetIso8601WeekOfYear(d);
                    inSlot   = (d, s) => d >= s && d < s.AddDays(7);
                    break;

                case ChartPeriod.Daily:
                    // last 7 days
                    slots = 7;
                    slotStart = i => now.AddDays(-(slots - 1 - i));
                    labelFn  = d => d.ToString("ddd");
                    inSlot   = (d, s) => d.Date == s.Date;
                    break;

                default: // Monthly
                    slots = 6;
                    slotStart = i => new DateTime(now.Year, now.Month, 1).AddMonths(-(slots - 1 - i));
                    labelFn  = d => d.ToString("MMM");
                    inSlot   = (d, s) => d.Year == s.Year && d.Month == s.Month;
                    break;
            }

            var result = new (string Label, double Income, double Expense)[slots];
            for (int i = 0; i < slots; i++)
            {
                var start = slotStart(i);
                double inc = 0, exp = 0;
                foreach (var tx in Transactions)
                    if (inSlot(tx.Date.Date, start))
                    {
                        if (tx.IsIncome) inc += tx.Amount;
                        else             exp += tx.Amount;
                    }
                result[i] = (labelFn(start), inc, exp);
            }
            return result;
        }

        // ISO week number helper
        private static int GetIso8601WeekOfYear(DateTime date)
        {
            var cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;
            return cal.GetWeekOfYear(date,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }
    }

    // simple transaction record
    public class Transaction
    {
        public DateTime Date      { get; set; }
        public double   Amount    { get; set; }
        public bool     IsIncome  { get; set; }
        public string   Category  { get; set; }

        public Transaction(DateTime date, double amount, bool isIncome, string category = "")
        {
            Date     = date;
            Amount   = amount;
            IsIncome = isIncome;
            Category = category;
        }
    }

    public enum ChartPeriod { Monthly, Weekly, Daily }
}
