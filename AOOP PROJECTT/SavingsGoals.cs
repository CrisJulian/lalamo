// Saving Goals Model
namespace CommonCents
{
    public class SavingsGoal
    {
        public string Name { get; set; }
        public double Target { get; set; }
        public double Saved { get; set; }
        public System.Drawing.Color GoalColor { get; set; }

        public double Remaining => Math.Max(0, Target - Saved);
        public double ProgressPercent => Target > 0 ? Math.Min(100, (Saved / Target) * 100) : 0;
        public bool IsComplete => Saved >= Target;

        public SavingsGoal(string name, double target, System.Drawing.Color color)
        {
            Name = name;
            Target = target;
            Saved = 0;
            GoalColor = color;
        }

        public void AddContribution(double amount)
        {
            Saved = Math.Min(Saved + amount, Target);
        }
    }
}