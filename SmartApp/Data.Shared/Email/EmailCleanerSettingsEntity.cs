﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Email
{
    public class EmailCleanerSettingsEntity: AEntityBase
    {
        public int UserId { get; set; }
        public bool EmailCleanerEnabled { get; set; }
        public bool UseScheduledEmailDataImport { get; set; }
        public bool SpamPredictionEnabled { get; set; }
        public bool FolderPredictionEnabled { get; set; }
        public bool ShareDataWithAi { get; set; }
        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public EmailAccountEntity Account { get; set; } = new EmailAccountEntity();
    }
}
