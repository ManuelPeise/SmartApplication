﻿namespace Data.Shared.Tools
{
    public class EmailCleanerSettingsEntity: AEntityBase
    {
        public bool Enabled { get; set; }
        public string Account { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool AllowReadEmails { get; set; }
        public bool AllowMoveEmails { get; set; }
        public bool AllowDeleteEmails { get; set; }
        public bool AllowCreateEmailFolder { get; set; }
        public bool ShareEmailDataToTrainAi { get; set; }
        public bool ScheduleCleanup { get; set; }
        public int ScheduleCleanupAtHour { get; set; }
        public DateTime? LastCleanupTime { get; set; }
        public DateTime? NextCleanupTime { get; set; }
        public ICollection<EmailAddressMappingEntity> EmailAddressMappings { get; set; } = new List<EmailAddressMappingEntity>();
    }
}
