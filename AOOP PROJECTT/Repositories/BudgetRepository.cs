using Microsoft.Data.SqlClient;
using AOOP_PROJECTT.SupportClasses;

namespace AOOP_PROJECTT.Repositories
{
    public static class BudgetRepository
    {
        // ================================================================
        // BUDGETS
        // ================================================================
        public static List<Budget> GetBudgets(int userId)
        {
            var list = new List<Budget>();

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    SELECT b.BudgetId, b.Name, b.LimitAmount, 
                           ISNULL(b.Spent, 0)        AS Spent,
                           ISNULL(b.CategoryColor, '#F5A623') AS CategoryColor
                    FROM Budgets b
                    WHERE b.UserId = @userId
                      AND b.Month  = @month
                      AND b.Year   = @year
                    ORDER BY b.CreatedAt";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@month",  DateTime.Now.Month);
                cmd.Parameters.AddWithValue("@year",   DateTime.Now.Year);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var budget = new Budget(
                        reader.GetString(1),
                        (double)reader.GetDecimal(2),
                        ColorTranslator.FromHtml(reader.GetString(4))
                    );
                    budget.BudgetId = reader.GetInt32(0);
                    budget.Spent    = (double)reader.GetDecimal(3);
                    list.Add(budget);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error loading budgets: " + ex.Message, "DB Error");
            }

            return list;
        }

        public static int AddBudget(int userId, Budget budget)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    INSERT INTO Budgets 
                        (UserId, Name, LimitAmount, Spent, CategoryColor, Month, Year, CreatedAt)
                    OUTPUT INSERTED.BudgetId
                    VALUES 
                        (@userId, @name, @limit, 0, @color, @month, @year, GETDATE())";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@name",   budget.Name);
                cmd.Parameters.AddWithValue("@limit",  (decimal)budget.Limit);
                cmd.Parameters.AddWithValue("@color",  ColorTranslator.ToHtml(budget.CategoryColor));
                cmd.Parameters.AddWithValue("@month",  DateTime.Now.Month);
                cmd.Parameters.AddWithValue("@year",   DateTime.Now.Year);

                return (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error adding budget: " + ex.Message, "DB Error");
                return -1;
            }
        }

        public static bool UpdateBudgetSpent(int budgetId, double spent)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = "UPDATE Budgets SET Spent = @spent WHERE BudgetId = @id";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@spent", (decimal)spent);
                cmd.Parameters.AddWithValue("@id",    budgetId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error updating budget: " + ex.Message, "DB Error");
                return false;
            }
        }

        public static bool DeleteBudget(int budgetId)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = "DELETE FROM Budgets WHERE BudgetId = @id";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", budgetId);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error deleting budget: " + ex.Message, "DB Error");
                return false;
            }
        }

        // ================================================================
        // SAVINGS GOALS
        // ================================================================

        public static List<SavingsGoal> GetGoals(int userId)
        {
            var list = new List<SavingsGoal>();

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    SELECT GoalId, Name, Target, 
                           ISNULL(Saved, 0) AS Saved,
                           ISNULL(GoalColor, '#2ECC71') AS GoalColor
                    FROM SavingsGoals
                    WHERE UserId = @userId
                    ORDER BY CreatedAt";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var goal = new SavingsGoal(
                        reader.GetString(1),
                        (double)reader.GetDecimal(2),
                        ColorTranslator.FromHtml(reader.GetString(4))
                    );
                    goal.GoalId = reader.GetInt32(0);
                    goal.Saved  = (double)reader.GetDecimal(3);
                    list.Add(goal);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error loading goals: " + ex.Message, "DB Error");
            }

            return list;
        }

        public static int AddGoal(int userId, SavingsGoal goal)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    INSERT INTO SavingsGoals 
                        (UserId, Name, Target, Saved, GoalColor, CreatedAt)
                    OUTPUT INSERTED.GoalId
                    VALUES 
                        (@userId, @name, @target, @saved, @color, GETDATE())";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@name",   goal.Name);
                cmd.Parameters.AddWithValue("@target", (decimal)goal.Target);
                cmd.Parameters.AddWithValue("@saved",  (decimal)goal.Saved);
                cmd.Parameters.AddWithValue("@color",  ColorTranslator.ToHtml(goal.GoalColor));

                return (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error adding goal: " + ex.Message, "DB Error");
                return -1;
            }
        }

        public static bool UpdateGoalSaved(int goalId, double saved)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = "UPDATE SavingsGoals SET Saved = @saved WHERE GoalId = @id";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@saved", (decimal)saved);
                cmd.Parameters.AddWithValue("@id",    goalId);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error updating goal: " + ex.Message, "DB Error");
                return false;
            }
        }

        public static bool DeleteGoal(int goalId)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = "DELETE FROM SavingsGoals WHERE GoalId = @id";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", goalId);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error deleting goal: " + ex.Message, "DB Error");
                return false;
            }
        }
    }
}
