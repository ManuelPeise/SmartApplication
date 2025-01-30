using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Email
{
    public class EmailFolderMappingEntity: AEntityBase
    {
        public int UserId { get; set; }
        public string SettingsGuid { get; set; } = string.Empty;
        public string SourceFolder { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool ShouldCleanup { get; set; }
        public int AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public EmailAddressEntity AddressEntity { get; set; } = new EmailAddressEntity();
        public int FolderId { get; set; }
        [ForeignKey(nameof(FolderId))]
        public EmailTargetFolderEntity FolderEntity { get; set; } = new EmailTargetFolderEntity();
        public int PredictedFolderId { get; set; }
        [ForeignKey(nameof(PredictedFolderId))]
        public EmailTargetFolderEntity? PredictedFolderEntity { get; set; } = new EmailTargetFolderEntity();
    }
}
