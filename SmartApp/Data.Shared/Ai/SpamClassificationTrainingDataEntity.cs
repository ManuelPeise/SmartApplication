namespace Data.Shared.Ai
{
    public class SpamClassificationTrainingDataEntity: AEntityBase
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public bool IsSpam { get; set; }
    }
}
