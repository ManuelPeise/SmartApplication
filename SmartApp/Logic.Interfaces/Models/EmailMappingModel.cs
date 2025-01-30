using Data.Shared.Email;

namespace Logic.Interfaces.Models
{
    public class EmailMappingModel
    {
        public int MappingId { get; set; }
        public int AddressEntityId { get; set; }
        public int SubjectEntityId { get; set; }
        public int UserId { get; set; }
        public string? UserDefinedTargetFolder { get; set; }
        public string? PredictedTargetFolder { get; set; }
        public bool UserDefinedAsSpam { get; set; }
        public bool PredictedAsSpam { get; set; }
        public string FromAddress { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;

    }
}
