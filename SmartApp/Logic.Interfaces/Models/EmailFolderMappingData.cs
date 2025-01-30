

namespace Logic.Interfaces.Models
{
    public class EmailFolderMappingData
    {
        public List<EmailTargetFolder> Folders { get; set; } = new List<EmailTargetFolder>();
        public List<FolderMapping> Mappings { get; set; } = new List<FolderMapping>();
    }

    public class EmailTargetFolder
    {
        public int Id { get; set; }
        public string ResourceKey { get; set; } = string.Empty;
    }

    public class FolderMapping
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SettingsGuid { get; set; } = string.Empty;
        public string SourceFolder { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public int TargetFolderId { get; set; }
        public int PredictedTargetFolderId { get; set; }
        public bool IsActive { get; set; }
        public bool ShouldCleanup { get; set; }
    }
}
