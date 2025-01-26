using Shared.Enums;

namespace Logic.Interfaces.Models
{
    public class EmailCleanerInterfaceConfiguration: EmailCleanerInterfaceConfigurationUiModel
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string? Password { get; set; }
    }

    public class EmailCleanerInterfaceConfigurationUiModel
    {
        public string SettingsGuid { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public EmailProviderTypeEnum ProviderType { get; set; }
        public int UnmappedDomains { get; set; }
        public bool ConnectionTestPassed { get; set; }
        public bool EmailCleanerEnabled { get; set; }
        public bool UseAiSpamPrediction { get; set; }
        public bool UseAiTargetFolderPrediction { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
        public List<EmailDomainFolderMapping> DomainFolderMapping { get; set; } = new List<EmailDomainFolderMapping>();
    }
}
