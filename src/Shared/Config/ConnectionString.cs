namespace Hyperspan.Shared.Config
{
    public class ConnectionString
    {
        public static string Label { get; set; } = "ConnectionStrings";
        public string PgDatabase { get; set; } = string.Empty;
        public string RedisCache { get; set; } = string.Empty;
    }
}
