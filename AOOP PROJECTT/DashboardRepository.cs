// DashboardRepository.cs
// Place in your AOOP_PROJECTT project alongside DatabaseHelper.cs
// Queries all data needed to populate the Dashboard.

using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace AOOP_PROJECTT
{
    // Simple data containers for the dashboard
    public class DashboardSummary
    {
        public double TotalBalance      { get; set; }
        public double MonthlyExpenses   { get; set; }
        public double MonthlyIncome     { get; set; }
        public double LastMonthExpenses { get; set; }

        // % of income spent this month
        public double IncomeSpentPercent =>
            MonthlyIncome > 0 ? Math.Min(100, (MonthlyExpenses / MonthlyIncome) * 100) : 0;

        // % change vs last month (expenses)
        public double ExpenseChangePercent =>
            LastMonthExpenses > 0
                ? ((MonthlyExpenses - LastMonthExpenses) / LastMonthExpenses) * 100
                : 0;
    }

    public class DashboardBudgetItem
    {
        public string Name          { get; set; }
        public double Spent         { get; set; }
        public double Limit         { get; set; }
        public string CategoryColor { get; set; }
        public double Percent       => Limit > 0 ? Math.Min(100, (Spent / Limit) * 100) : 0;
    }

    public class DashboardGoalItem
    {
        public string Name    { get; set; }
        public double Saved   { get; set; }
        public double Target  { get; set; }
        public string Color   { get; set; }
        public double Percent => Target > 0 ? Math.Min(100, (Saved / Target) * 100) : 0;
    }

    public static class DashboardRepository
    {
        // ================================================================
        // SUMMARY: Total Balance + Monthly Expenses + Income
        // ================================================================
        public static DashboardSummary GetSummary(int userId)
        {
            var summary = new DashboardSummary();

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    SELECT
                        -- Total balance (all time income - all time expenses)
                        SUM(CASE WHEN [Type] = 'Income'  THEN Amount ELSE -Amount END) AS TotalBalance,

                        -- This month's expenses
                        SUM(CASE WHEN [Type] = 'Expense'
                                  AND MONTH([Date]) = MONTH(GETDATE())
                                  AND YEAR([Date])  = YEAR(GETDATE())
                             THEN Amount ELSE 0 END) AS MonthlyExpenses,

                        -- This month's income
                        SUM(CASE WHEN [Type] = 'Income'
                                  AND MONTH([Date]) = MONTH(GETDATE())
                                  AND YEAR([Date])  = YEAR(GETDATE())
                             THEN Amount ELSE 0 END) AS MonthlyIncome,

                        -- Last month's expenses (for % change)
                        SUM(CASE WHEN [Type] = 'Expense'
                                  AND MONTH([Date]) = MONTH(DATEADD(MONTH, -1, GETDATE()))
                                  AND YEAR([Date])  = YEAR(DATEADD(MONTH, -1, GETDATE()))
                             THEN Amount ELSE 0 END) AS LastMonthExpenses
                    FROM Transactions
                    WHERE UserId = @userId";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    summary.TotalBalance      = reader.IsDBNull(0) ? 0 : (double)reader.GetDecimal(0);
                    summary.MonthlyExpenses   = reader.IsDBNull(1) ? 0 : (double)reader.GetDecimal(1);
                    summary.MonthlyIncome     = reader.IsDBNull(2) ? 0 : (double)reader.GetDecimal(2);
                    summary.LastMonthExpenses = reader.IsDBNull(3) ? 0 : (double)reader.GetDecimal(3);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error loading dashboard summary: " + ex.Message, "DB Error");
            }

            return summary;
        }

        // ================================================================
        // BUDGET STATUS: Top 4 budgets for current month
        // ================================================================
        public static List<DashboardBudgetItem> GetBudgetStatus(int userId)
        {
            var list = new List<DashboardBudgetItem>();

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    SELECT TOP 4
                        Name,
                        ISNULL(Spent, 0)              AS Spent,
                        LimitAmount,
                        ISNULL(CategoryColor, '#F5A623') AS CategoryColor
                    FROM Budgets
                    WHERE UserId = @userId
                      AND Month  = @month
                      AND Year   = @year
                    ORDER BY Spent DESC";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@month",  DateTime.Now.Month);
                cmd.Parameters.AddWithValue("@year",   DateTime.Now.Year);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DashboardBudgetItem
                    {
                        Name          = reader.GetString(0),
                        Spent         = (double)reader.GetDecimal(1),
                        Limit         = (double)reader.GetDecimal(2),
                        CategoryColor = reader.GetString(3)
                    });
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error loading budget status: " + ex.Message, "DB Error");
            }

            return list;
        }

        // ================================================================
        // SAVINGS GOALS: Top 3 goals
        // ================================================================
        public static List<DashboardGoalItem> GetSavingsGoals(int userId)
        {
            var list = new List<DashboardGoalItem>();

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    SELECT TOP 3
                        Name,
                        ISNULL(Saved, 0)            AS Saved,
                        Target,
                        ISNULL(GoalColor, '#2ECC71') AS GoalColor
                    FROM SavingsGoals
                    WHERE UserId = @userId
                    ORDER BY CreatedAt ASC";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DashboardGoalItem
                    {
                        Name   = reader.GetString(0),
                        Saved  = (double)reader.GetDecimal(1),
                        Target = (double)reader.GetDecimal(2),
                        Color  = reader.GetString(3)
                    });
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error loading savings goals: " + ex.Message, "DB Error");
            }

            return list;
        }
    }
}
