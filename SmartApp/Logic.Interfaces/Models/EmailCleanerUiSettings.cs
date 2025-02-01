using Shared.Enums;

namespace Logic.Interfaces.Models
{
    public class EmailCleanerUiSettings
    {
        public int AccountId { get; set; }
        public int SettingsId { get; set; }
        public int UserId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public bool EmailCleanerEnabled { get; set; }
        public bool UseScheduledEmailDataImport { get; set; }
        public bool ConnectionTestPassed { get; set; }
        public EmailProviderTypeEnum ProviderType { get; set; }
        public string? UpdatedAt { get; set; } 
        public string? UpdatedBy { get; set; }
    }
}
