using Shared.Enums;

namespace Shared.Models.Settings.EmailAccountSettings
{
    public class EmailAccountSettingsModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public EmailProviderTypeEnum ProviderType { get; set; }
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 993;
        public string EmailAddress { get; set; } = string.Empty;
        public string? Password { get; set; }
        public MessageLog? MessageLog { get; set; }

    }

    public class MessageLog
    {
        public string User { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;
    }
}
