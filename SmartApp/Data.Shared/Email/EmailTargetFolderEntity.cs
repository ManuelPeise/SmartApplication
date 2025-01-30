namespace Data.Shared.Email
{
    public class EmailTargetFolderEntity: AEntityBase
    {
        public string ResourceKey { get; set; } = string.Empty;
        public string TargetFolderName { get; set; } = string.Empty;
    }
}
