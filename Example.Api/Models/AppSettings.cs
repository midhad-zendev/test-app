namespace Example.Api.Models
{
    public class AppSettingsModel
    {
        public AppSettingsModel()
        {
            // Set default value.
        }

        public ConnectionStrings ConnectionStrings { get; set; }
        public AppSettingProperties AppSettings { get; set; }
    }
   
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class AppSettingProperties
    {
        public TokenAuthentication TokenAuthenticationConfig { get; set; }
    }

    public class TokenAuthentication
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresAfterHours { get; set; }

    }
}
