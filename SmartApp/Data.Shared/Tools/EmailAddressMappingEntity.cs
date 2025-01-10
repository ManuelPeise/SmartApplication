using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Tools
{
    public class EmailAddressMappingEntity: AEntityBase
    {
        public int UserId { get; set; }
        public string? SourceAddress { get; set; }
        public string? Domain { get; set; }
        public bool ShouldCleanup { get; set; }
        public bool IsSpam { get; set; }
        public string? PredictedAs { get; set; }
        public int EmailCleanerSettingsId { get; set; }
        [ForeignKey(nameof(EmailCleanerSettingsId))]
        public EmailCleanerSettingsEntity EmailCleanerSettings { get; set; } = new EmailCleanerSettingsEntity();
    }
}
