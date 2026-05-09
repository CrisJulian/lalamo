using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace AOOP_PROJECTT
{
    public static class TransactionRepository
    {
        public static List<TransactionRecord> GetTransactions(int userId)
        {
            var list = new List<TransactionRecord>();
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = @"
            SELECT TransactionId, Amount, Description, Date, Type, Category
            FROM Transactions
            WHERE UserId = @userId
            ORDER BY Date DESC";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new TransactionRecord
                    {
                        TransactionId = reader.GetInt32(0),
                        Amount = (double)reader.GetDecimal(1),
                        Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Date = reader.GetDateTime(3),
                        Type = reader.GetString(4),
                        Category = reader.IsDBNull(5) ? "" : reader.GetString(5) // 👈 added
                    });
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error loading transactions: " + ex.Message, "DB Error");
            }
            return list;
        }

        public static void UpdateBudgetSpent(int userId, string budgetName, double amount)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = @"
            UPDATE Budgets
            SET Spent = ISNULL(Spent, 0) + @amount
            WHERE UserId = @userId
              AND Name = @name
              AND Month = @month
              AND Year = @year";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@amount", (decimal)amount);
                cmd.Parameters.AddWithValue("@name", budgetName);
                cmd.Parameters.AddWithValue("@month", DateTime.Now.Month);
                cmd.Parameters.AddWithValue("@year", DateTime.Now.Year);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error updating budget spent: " + ex.Message, "DB Error");
            }
        }

        public static void DeleteTransaction(int transactionId)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = "DELETE FROM Transactions WHERE TransactionId = @id";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", transactionId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting transaction: " + ex.Message, "DB Error");
            }
        }

        public static int AddTransaction(int userId, TransactionRecord tx)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = @"
            INSERT INTO Transactions (UserId, Amount, Description, Date, Type, Category)
            OUTPUT INSERTED.TransactionId
            VALUES (@userId, @amount, @desc, @date, @type, @category)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@amount", (decimal)tx.Amount);
                cmd.Parameters.AddWithValue("@desc", tx.Description);
                cmd.Parameters.AddWithValue("@date", tx.Date);
                cmd.Parameters.AddWithValue("@type", tx.Type);
                cmd.Parameters.AddWithValue("@category", tx.Category ?? (object)DBNull.Value);
                return (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding transaction: " + ex.Message, "DB Error");
                return -1;
            }
        }
    }



    public class TransactionRecord
    {
        public int TransactionId { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; } = "";
        public DateTime Date { get; set; }
        public string Type { get; set; } = "Expense";
        public string Category { get; set; } = "";
    }
}

