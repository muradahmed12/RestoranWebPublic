namespace RestoranWeb
{
    public class Global
    {
        public static string LoginSession { get; } = "RW-Session";
        public static string LoginCookie { get; } = "RW-AUTH";
        public const string AdminRole  = "Admin";
        public const string ManagerRole  = "Manager";
    }
}
