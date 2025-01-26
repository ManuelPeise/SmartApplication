using Shared.Enums;

namespace Logic.Interfaces.Models
{
    public class EmailAccountSettings
    {
        public string SettingsGuid { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public EmailProviderTypeEnum ProviderType { get; set; }
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string? Password { get; set; }
        public bool ConnectionTestPassed { get; set; }
        
    }
}
