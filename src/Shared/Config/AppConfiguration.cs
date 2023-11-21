namespace Hyperspan.Shared.Config
{
    public class AppConfiguration
    {
        public static string Label { get; set; } = "AppConfiguration";
        public string SiteUrl { get; set; } = string.Empty;
        public string JwtSecurityKey { get; set; } = string.Empty;
        public string JwtIssuer { get; set; } = string.Empty;
        public string JwtAudience { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
        public int JwtExpiry { get; set; }
    }
}
