
namespace Infra.Utili.ConfigrationModels
{
    public class AppSettingsConfig
    {
        public string? Secret { get; set; }
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
        public int AddHourExpired { get; set; }
    }
}
