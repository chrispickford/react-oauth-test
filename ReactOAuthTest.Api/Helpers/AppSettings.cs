namespace ReactOAuthTest.Api.Helpers
{
    public class AppSettings
    {
        public string JwtAudience { get; set; }
        public int JwtExpireMins { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtKey { get; set; }
    }
}