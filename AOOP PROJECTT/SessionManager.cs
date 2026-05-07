// SessionManager.cs
// Place this file in your AOOP_PROJECTT project.
// This static class holds the currently logged-in user's info
// and is accessible from anywhere in the app.

namespace AOOP_PROJECTT
{
    public static class SessionManager
    {
        public static int UserId { get; set; } = 0;
        public static string FullName { get; set; } = "";
        public static string Email { get; set; } = "";
        public static string Phone { get; set; } = "";
        public static string Location { get; set; } = "";
        public static string AccountType { get; set; } = "PERSONAL";
        public static DateTime MemberSince { get; set; } = DateTime.Now;

        public static bool IsLoggedIn => UserId > 0;

        public static void Clear()
        {
            UserId = 0;
            FullName = "";
            Email = "";
            Phone = "";
            Location = "";
            AccountType = "PERSONAL";
            MemberSince = DateTime.Now;
        }
    }
}
