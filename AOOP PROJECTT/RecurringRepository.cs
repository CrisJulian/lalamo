using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using CommonCents;

namespace AOOP_PROJECTT
{
    public class RecurringRepository
    {
        public void EnsureTableExists()
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                // Drop old table if it has wrong columns
                string checkColumns = @"
            IF EXISTS (
                SELECT * FROM sysobjects WHERE name='RecurringPayments' AND xtype='U'
            )
            AND NOT EXISTS (
                SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME='RecurringPayments' AND COLUMN_NAME='Id'
            )
            DROP TABLE RecurringPayments";

                using (var cmd = new SqlCommand(checkColumns, conn))
                    cmd.ExecuteNonQuery();

                // Now create if it doesn't exist
                string createTable = @"
            IF NOT EXISTS (
                SELECT * FROM sysobjects WHERE name='RecurringPayments' AND xtype='U'
            )
            CREATE TABLE RecurringPayments (
                Id        INT IDENTITY(1,1) PRIMARY KEY,
                UserId    INT            NOT NULL,
                Name      NVARCHAR(100)  NOT NULL,
                Amount    DECIMAL(18,2)  NOT NULL,
                Frequency NVARCHAR(50)   NOT NULL,
                NextDate  DATE           NOT NULL,
                Category  NVARCHAR(100)  NOT NULL
            )";

                using (var cmd = new SqlCommand(createTable, conn))
                    cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error creating table: " + ex.Message, "DB Error");
            }
        }

        public List<RecurringPayment> GetAll(int userId)
        {
            var list = new List<RecurringPayment>();

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    SELECT Id, Name, Amount, Frequency, NextDate, Category
                    FROM RecurringPayments
                    WHERE UserId = @userId
                    ORDER BY Id";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new RecurringPayment
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Amount = reader.GetDecimal(2),
                        Frequency = reader.GetString(3),
                        NextDate = reader.GetDateTime(4),
                        Category = reader.GetString(5)
                    });
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error loading recurring payments: " + ex.Message, "DB Error");
            }

            return list;
        }

        public int Add(int userId, RecurringPayment payment)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    INSERT INTO RecurringPayments 
                        (UserId, Name, Amount, Frequency, NextDate, Category)
                    OUTPUT INSERTED.Id
                    VALUES 
                        (@userId, @name, @amount, @frequency, @nextDate, @category)";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@name", payment.Name);
                cmd.Parameters.AddWithValue("@amount", payment.Amount);
                cmd.Parameters.AddWithValue("@frequency", payment.Frequency);
                cmd.Parameters.AddWithValue("@nextDate", payment.NextDate);
                cmd.Parameters.AddWithValue("@category", payment.Category);

                return (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error adding recurring payment: " + ex.Message, "DB Error");
                return -1;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = "DELETE FROM RecurringPayments WHERE Id = @id";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error deleting recurring payment: " + ex.Message, "DB Error");
                return false;
            }
        }
    }
}