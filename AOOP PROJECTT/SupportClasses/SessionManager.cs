namespace AOOP_PROJECTT.SupportClasses
{
    public static class SessionManager
    {
        public static int UserId { get; set; } = 0;
        public static string FullName { get; set; } = "";
        public static string Username { get; set; } = ""; // 👈 add this
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
            Username = ""; // 👈 add this
            Email = "";
            Phone = "";
            Location = "";
            AccountType = "PERSONAL";
            MemberSince = DateTime.Now;
        }
    }
}