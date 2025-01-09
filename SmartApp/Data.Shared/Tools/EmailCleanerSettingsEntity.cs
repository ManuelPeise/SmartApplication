namespace Data.Shared.Tools
{
    public class EmailCleanerSettingsEntity: AEntityBase
    {
        public bool Enabled { get; set; }
        public bool AllowReadEmails { get; set; }
        public bool AllowMoveEmails { get; set; }
        public bool AllowDeleteEmails { get; set; }
        public bool AllowCreateEmailFolder { get; set; }
        public bool AllowUseEmailDataForSpamDetectionAiTraining { get; set; }
        public int ScheduledCleanupAtHour { get; set; }
        public DateTime LastCleanupTime { get; set; }
        public DateTime NextCleanupTime { get; set; }
        public ICollection<EmailAddressMapping> EmailAddressMappings { get; set; } = new List<EmailAddressMapping>();
    }
}
