using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Email
{
    public class EmailCleanupConfigurationEntity : AEntityBase
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public EmailAccountEntity Account { get; set; } = new EmailAccountEntity();
        public int AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public EmailAddressEntity Address { get; set; } = new EmailAddressEntity();
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public EmailSubjectEntity Subject { get; set; } = new EmailSubjectEntity();
        public int TargetFolderId { get; set; } = 1;
        [ForeignKey(nameof(TargetFolderId))]
        public EmailTargetFolderEntity TargetFolder { get; set; } = new EmailTargetFolderEntity();
        public int? PredictedTargetFolderId { get; set; } = null;
        [ForeignKey(nameof(PredictedTargetFolderId))]
        public EmailTargetFolderEntity? PredictedTargetFolder { get; set; }
        public bool IsSharedWithAi { get; set; }
        public bool IsSpam { get; set; }
        public bool IsPredictedAsSpam { get; set; }
        public bool Backup { get; set; }
        public bool Delete { get; set; }
    }
}
