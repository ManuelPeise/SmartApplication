using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Email
{
    public class EmailMappingEntity : AEntityBase
    {
        public int UserId { get; set; }
        public string SettingsGuid { get; set; } = string.Empty;
        public DateTime MessageDate { get; set; }
        public string SourceFolder { get; set; } = string.Empty;
        public string? TargetFolder { get; set; }
        public string? PredictedValue { get; set; }
        public bool AutomatedCleanup { get; set; }
        public bool IsProcessed { get; set; }
        public bool ShareWithAi { get; set; }
        public int AddressId { get; set; }
        public EmailAddressEntity AddressEntity { get; set; } = new EmailAddressEntity();
        public int SubjectId { get; set; }
        public EmailSubjectEntity SubjectEntity { get; set; } = new EmailSubjectEntity();
    }
}
