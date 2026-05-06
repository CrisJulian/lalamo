// Debt.cs
namespace CommonCents
{
    public class Debt
    {
        public string Name { get; set; }
        public double Principal { get; set; }          // Original loan amount 
        public double InterestRate { get; set; }        // Annual interest rate in %
        public double AmountPaid { get; set; }
        public double MonthlyPayment { get; set; }
        public DateTime DueDate { get; set; }
        public bool PaidThisMonth { get; set; }

        // Principal + total interest applied at loan creation
        public double TotalDebt => Principal * (1 + InterestRate / 100);

        public double Remaining => TotalDebt - AmountPaid;

        public double ProgressPercent => TotalDebt > 0
            ? Math.Min(100, (AmountPaid / TotalDebt) * 100)
            : 0;

        public bool IsPaidOff => Remaining <= 0;

        public Debt(string name, double principal, double interestRate,
                    double monthlyPayment, DateTime dueDate)
        {
            Name = name;
            Principal = principal;
            InterestRate = interestRate;
            MonthlyPayment = monthlyPayment;
            DueDate = dueDate;
            AmountPaid = 0;
            PaidThisMonth = false;
        }

        /// Log a manual payment amount.
        public void LogPayment(double amount)
        {
            AmountPaid = Math.Min(AmountPaid + amount, TotalDebt);
        }

        /// Mark monthly payment as paid.
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
