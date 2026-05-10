namespace AOOP_PROJECTT.SupportClasses
{
    public class Budget
    {
        public int    BudgetId      { get; set; } = 0; 
        public string Name          { get; set; }
        public double Limit         { get; set; }
        public double Spent         { get; set; }
        public Color  CategoryColor { get; set; }

        public double Remaining       => Math.Max(0, Limit - Spent);
        public double ProgressPercent => Limit > 0 ? Math.Min(100, (Spent / Limit) * 100) : 0;

        public Budget(string name, double limit, Color color)
        {
            Name          = name;
            Limit         = limit;
            Spent         = 0;
            CategoryColor = color;
        }

        public void AddSpending(double amount)
        {
            Spent = Math.Min(Spent + amount, Limit);
        }
    }
}
