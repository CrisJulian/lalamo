// Debt.cs — UPDATED
// Added DebtId so the repository can update/delete by ID.

using System;

namespace CommonCents
{
    public class Debt
    {
        public int      DebtId         { get; set; } = 0;   // DB primary key
        public string   Name           { get; set; }
        public double   Principal      { get; set; }
        public double   InterestRate   { get; set; }
        public double   AmountPaid     { get; set; }
        public double   MonthlyPayment { get; set; }
        public DateTime DueDate        { get; set; }
        public bool     PaidThisMonth  { get; set; }

        public double TotalDebt       => Principal * (1 + InterestRate / 100);
        public double Remaining       => TotalDebt - AmountPaid;
        public double ProgressPercent => TotalDebt > 0
            ? Math.Min(100, (AmountPaid / TotalDebt) * 100) : 0;
        public bool   IsPaidOff       => Remaining <= 0;

        public Debt(string name, double principal, double interestRate,
                    double monthlyPayment, DateTime dueDate)
        {
            Name           = name;
            Principal      = principal;
            InterestRate   = interestRate;
            MonthlyPayment = monthlyPayment;
            DueDate        = dueDate;
            AmountPaid     = 0;
            PaidThisMonth  = false;
        }

        public void LogPayment(double amount)
        {
            AmountPaid = Math.Min(AmountPaid + amount, TotalDebt);
        }

        public void PayMonthly()
        {
            if (!PaidThisMonth)
            {
                LogPayment(MonthlyPayment);
                PaidThisMonth = true;
            }
        }
    }
}
