using Shared.Enums;

namespace Logic.Interfaces.EmailAccountInterface.Models
{
    public class EmailAccountSettingsUiModel
    {
        public Guid SettingsGuid { get; set; }
        public int UserId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public EmailProviderTypeEnum ProviderType { get; set; }
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public bool ConnectionTestPassed { get; set; }
    }

    public class EmailAccountSettings : EmailAccountSettingsUiModel
    {
        
        public string? Password { get; set; }
    }
}
