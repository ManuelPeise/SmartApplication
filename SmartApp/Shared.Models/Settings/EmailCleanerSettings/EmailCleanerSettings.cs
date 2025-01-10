namespace Shared.Models.Settings.EmailCleanerSettings
{
    public class EmailCleanerSettings
    {
        public int SettingsId { get; set; }
        public int UserId { get; set; }
        public bool Enabled { get; set; }
        public bool AllowReadEmails { get; set; }
        public bool AllowMoveEmails { get; set; }
        public bool AllowDeleteEmails { get; set; }
        public bool AllowCreateEmailFolder { get; set; }
        public bool ShareEmailDataToTrainAi { get; set; }
        public bool ScheduleCleanup { get; set; }
        public int ScheduleCleanupAtHour { get; set; }
        public bool HasMappings { get; set; }
        public DateTime? LastCleanupTime { get; set; }
        public List<EmailAddressMapping> Mappings { get; set; } = new List<EmailAddressMapping>();
    }
}
