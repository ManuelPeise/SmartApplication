namespace Logic.Interfaces.EmailCleanerInterface.Models
{
    public class EmailCleanerUpdateModel
    {
        public string SettingsGuid { get; set; } = string.Empty;
        public bool EmailCleanerEnabled { get; set; }
        public bool UseAiSpamPrediction { get; set; }
        public bool UseAiTargetFolderPrediction { get; set; }
        public bool FolderMappingEnabled { get; set; }
        public bool FolderMappingIsInitialized { get; set; }
    }
}
