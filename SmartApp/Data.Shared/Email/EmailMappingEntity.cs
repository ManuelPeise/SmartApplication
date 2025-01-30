using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Email
{
    public class EmailMappingEntity : AEntityBase
    {
        public int UserId { get; set; }
        public string? UserDefinedTargetFolder { get; set; }
        public string? PredictedTargetFolder { get; set; }
        public bool UserDefinedAsSpam { get; set; }
        public bool PredictedAsSpam { get; set; }
        public int AddressId { get; set; }
        public EmailAddressEntity AddressEntity { get; set; } = new EmailAddressEntity();
        public int SubjectId { get; set; }
        public EmailSubjectEntity SubjectEntity { get; set; } = new EmailSubjectEntity();
    }
}
