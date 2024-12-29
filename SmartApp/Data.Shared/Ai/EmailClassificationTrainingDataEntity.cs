using Shared.Enums.Ai;

namespace Data.Shared.Ai
{
    public class EmailClassificationTrainingDataEntity: AEntityBase
    {
        public string From { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public SpamClassificationEnum Classification { get; set; }
    }
}
