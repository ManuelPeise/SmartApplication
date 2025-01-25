using Shared.Enums;

namespace Logic.Interfaces.EmailAccountInterface.Models
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
        public EmailAccountAiSettings EmailAccountAiSettings { get; set; } = new EmailAccountAiSettings();
    }

    public class EmailAccountAiSettings
    {
        public bool UseAiSpamPrediction { get; set; }
        public bool UseAiTargetFolderPrediction { get; set; }
    }
}
