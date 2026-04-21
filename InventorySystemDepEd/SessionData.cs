namespace InventorySystemDepEd
{
    public static class SessionData
    {
        public static int UserID { get; set; }                    // numeric ID
        public static string UserEmail { get; set; } = string.Empty;
        public static string UserFullName { get; set; } = string.Empty;
        public static string UserRole { get; set; } = "Guest";
        public static int CurrentUserId { get; set; }
        public static bool IsLoggedIn { get; set; } = false;
        public static int CurrentOfficeId { get; set; }

        public static void Clear()
        {
            UserID = 0;
            UserEmail = string.Empty;
            UserFullName = string.Empty;
            UserRole = "Guest";
            IsLoggedIn = false;
        }
    }
}