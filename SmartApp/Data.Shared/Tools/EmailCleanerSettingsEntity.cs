namespace Data.Shared.Tools
{
    public class EmailCleanerSettingsEntity: AEntityBase
    {
        public int SettingsId { get; set; }
        public bool EmailCleanerEnabled { get; set; }
        public bool EmailCleanerAiEnabled { get; set; }
        public bool IsAgreed { get; set; }
        public string FolderConfigurationJson { get; set; } = string.Empty;
        public string MessageLogJson { get; set; } = string.Empty;
    }
}
