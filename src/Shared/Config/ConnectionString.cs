namespace Shared.Config
{
    public class ConnectionString
    {
        public static string Label { get; set; } = "ConnectionStrings";
        public string DbConnection { get; set; } = string.Empty;
    }
}
