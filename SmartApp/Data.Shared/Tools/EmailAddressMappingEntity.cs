using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Tools
{
    public class EmailAddressMappingEntity:AEntityBase
    {
        public bool IsActive { get; set; }
        public int AccountId { get; set; }
        public string EmailFolder { get; set; } = string.Empty;
        public bool IsSpam { get; set; }
        public string? PredictedValue { get; set; }
        public string TargetFolder { get; set; } = string.Empty;
        public int Action { get; set; } 
        public int EmailDataId { get; set; }
        [ForeignKey(nameof(EmailDataId))]
        public EmailDataEntity EmailData { get; set; } = new EmailDataEntity();
    }
}
