using Shared.Enums.Ai;

namespace Data.Shared.Ai
{
    public class EmailClassificationTrainingDataEntity: AEntityBase
    {
        public int UserId { get; set; }
        public string From { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public SpamClassificationEnum Classification { get; set; }
    }
}
