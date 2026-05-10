using Microsoft.Data.SqlClient;
using AOOP_PROJECTT.SupportClasses;

namespace AOOP_PROJECTT.Repositories
{
    public static class DebtRepository
    {
        // ================================================================
        // DEBTS
        // ================================================================

        /// Load all debts for the logged-in user.
        public static List<Debt> GetDebts(int userId)
        {
            var list = new List<Debt>();

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    SELECT DebtId, Name, Principal, InterestRate,
                           MonthlyPayment, ISNULL(AmountPaid, 0) AS AmountPaid,
                           DueDate, ISNULL(PaidThisMonth, 0)     AS PaidThisMonth
                    FROM Debts
                    WHERE UserId = @userId
                    ORDER BY DueDate ASC";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var debt = new Debt(
                        reader.GetString(1),
                        (double)reader.GetDecimal(2),
                        (double)reader.GetDecimal(3),
                        (double)reader.GetDecimal(4),
                        reader.GetDateTime(6)
                    );
                    debt.DebtId        = reader.GetInt32(0);
                    debt.AmountPaid    = (double)reader.GetDecimal(5);
                    debt.PaidThisMonth = reader.GetBoolean(7);
                    list.Add(debt);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error loading debts: " + ex.Message, "DB Error");
            }

            return list;
        }

        /// Insert a new debt into the database.
        public static int AddDebt(int userId, Debt debt)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    INSERT INTO Debts 
                        (UserId, Name, Principal, InterestRate, 
                         MonthlyPayment, AmountPaid, DueDate, PaidThisMonth)
                    OUTPUT INSERTED.DebtId
                    VALUES 
                        (@userId, @name, @principal, @interestRate,
                         @monthlyPayment, 0, @dueDate, 0)";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId",         userId);
                cmd.Parameters.AddWithValue("@name",           debt.Name);
                cmd.Parameters.AddWithValue("@principal",      (decimal)debt.Principal);
                cmd.Parameters.AddWithValue("@interestRate",   (decimal)debt.InterestRate);
                cmd.Parameters.AddWithValue("@monthlyPayment", (decimal)debt.MonthlyPayment);
                cmd.Parameters.AddWithValue("@dueDate",        debt.DueDate);

                return (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error adding debt: " + ex.Message, "DB Error");
                return -1;
            }
        }

        /// Update AmountPaid and PaidThisMonth after a payment.
        public static bool UpdateDebtPayment(int debtId, double amountPaid, bool paidThisMonth)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    UPDATE Debts 
                    SET AmountPaid    = @amountPaid,
                        PaidThisMonth = @paidThisMonth
                    WHERE DebtId = @id";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@amountPaid",    (decimal)amountPaid);
                cmd.Parameters.AddWithValue("@paidThisMonth", paidThisMonth);
                cmd.Parameters.AddWithValue("@id",            debtId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error updating debt payment: " + ex.Message, "DB Error");
                return false;
            }
        }

        /// Delete a debt (also deletes its payment history via cascade or manual).
        public static bool DeleteDebt(int debtId)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                // Delete payment log first to avoid FK constraint error
                string deletePay = "DELETE FROM DebtPayments WHERE DebtId = @id";
                using (var cmd = new SqlCommand(deletePay, conn))
                {
                    cmd.Parameters.AddWithValue("@id", debtId);
                    cmd.ExecuteNonQuery();
                }

                string deleteDebt = "DELETE FROM Debts WHERE DebtId = @id";
                using (var cmd = new SqlCommand(deleteDebt, conn))
                {
                    cmd.Parameters.AddWithValue("@id", debtId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error deleting debt: " + ex.Message, "DB Error");
                return false;
            }
        }

        // ================================================================
        // DEBT PAYMENT LOG
        // ================================================================

        /// Log an individual payment into DebtPayments history table.
        public static bool LogPayment(int debtId, double amount, string notes = "")
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    INSERT INTO DebtPayments (DebtId, Amount, PaidOn, Notes)
                    VALUES (@debtId, @amount, GETDATE(), @notes)";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@debtId", debtId);
                cmd.Parameters.AddWithValue("@amount", (decimal)amount);
                cmd.Parameters.AddWithValue("@notes",  notes ?? "");

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error logging payment: " + ex.Message, "DB Error");
                return false;
            }
        }
    }
}
