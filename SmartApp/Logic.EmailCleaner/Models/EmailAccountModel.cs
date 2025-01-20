using Shared.Enums;

namespace Logic.EmailCleaner.Models
{
    public class EmailAccountModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public EmailProviderTypeEnum ProviderType { get; set; }
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string? Password { get; set; }
        public bool ConnectionTestPassed { get; set; }
        public MessageLog? MessageLog { get; set; }
        public EmailCleanerSettings Settings { get; set; } = new EmailCleanerSettings();
        public List<EmailMappingModel> EmailAddressMappings { get; set; } = new List<EmailMappingModel>();
    }

    public class EmailCleanerSettings
    {
        public int SettingsId { get; set; }
        public bool EmailCleanerEnabled { get; set; }
        public bool EmailCleanerAiEnabled { get; set; }
        public bool IsAgreed { get; set; }
        public List<FolderSettings> FolderConfiguration { get; set; } = new List<FolderSettings>();
        public MessageLog? MessageLog { get; set; }
    }

    public class FolderSettings
    {
        public Guid FolderId { get; set; }
        public string FolderName { get; set; } = string.Empty;
        public bool IsInbox { get; set; }
    }

    public class MessageLog
    {
        public string User { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;
    }
}
