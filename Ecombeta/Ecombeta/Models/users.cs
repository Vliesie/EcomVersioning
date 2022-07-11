namespace Ecombeta.Models
{
    public class Users
    {
        public static bool LoggedIn { get; set; }
        public static string Username { get; set; }
        public string Password { get; set; }
        public static string Store { get; set; }
        public static int CId { get; set; }
        public static string CEmail { get; set; }

        public int id { get; set; }
    }
}