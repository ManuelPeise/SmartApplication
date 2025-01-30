namespace Logic.Interfaces.Models
{
    public class EmailFolderMappingUpdate
    {
        public string SettingsGuid { get; set; } = string.Empty;
        public List<FolderMapping> Mappings { get; set; } = new List<FolderMapping>();
    }
}
