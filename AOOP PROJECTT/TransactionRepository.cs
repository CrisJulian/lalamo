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
                    SELECT TransactionId, Amount, Description, Date, Type
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
                        Type = reader.GetString(4)
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

        public static int AddTransaction(int userId, TransactionRecord tx)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = @"
                    INSERT INTO Transactions (UserId, Amount, Description, Date, Type)
                    OUTPUT INSERTED.TransactionId
                    VALUES (@userId, @amount, @desc, @date, @type)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@amount", (decimal)tx.Amount);
                cmd.Parameters.AddWithValue("@desc", tx.Description);
                cmd.Parameters.AddWithValue("@date", tx.Date);
                cmd.Parameters.AddWithValue("@type", tx.Type);
                return (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error adding transaction: " + ex.Message, "DB Error");
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
    }
}