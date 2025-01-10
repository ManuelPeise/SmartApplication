using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Tools
{
    public class EmailAccountEntity : AEntityBase
    {
        public int UserId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public EmailProviderTypeEnum ProviderType { get; set; }
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 993;
        public string EmailAddress { get; set; } = string.Empty;
        public string EncodedPassword { get; set; } = string.Empty;
        public string? MessageLogJson { get; set; }
        public int? EmailCleanerSettingsId { get; set; }
        [ForeignKey(nameof(EmailCleanerSettingsId))]
        public EmailCleanerSettingsEntity? EmailCleanerSettings { get; set; }

    }
}
