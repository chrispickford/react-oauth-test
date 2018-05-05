namespace ReactOAuthTest.Api.Helpers
{
    public class AppSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireMins { get; set; }
    }
}