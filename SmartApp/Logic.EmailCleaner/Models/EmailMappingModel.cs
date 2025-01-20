using Shared.Enums;

namespace Logic.EmailCleaner.Models
{
    public class EmailMappingModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int AccountId { get; set; }
        public string EmailFolder { get; set; } = string.Empty;
        public string TargetFolder { get; set; } = string.Empty;
        public string SourceAddress { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string? PredictedValue { get; set; }
        public bool IsSpam { get; set; }
        public int Action { get; set; }

    }
}
