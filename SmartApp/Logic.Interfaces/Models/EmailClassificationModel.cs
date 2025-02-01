using Shared.Enums;

namespace Logic.Interfaces.Models
{
    public class EmailClassificationModel
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Domain => EmailAddress.Split('@')[1];
        public string Subject { get; set; } = string.Empty;
        public int TargetFolderId { get; set; }
        public int? PredictedTargetFolderId { get; set; }
        public SpamValueEnum SpamId { get; set; }
        public SpamValueEnum? PredictedSpamId { get; set; }
    }
}
