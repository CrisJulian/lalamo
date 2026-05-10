using System;
using Microsoft.Data.SqlClient;

namespace AOOP_PROJECTT.SupportClasses
{
    public static class DatabaseHelper
    {
        private static readonly string ConnectionString =
            @"Server=(localdb)\MSSQLLocalDB;Database=AOOP_DB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "DB Connection failed: " + ex.Message, "Error");
                return false;
            }
        }
    }
}