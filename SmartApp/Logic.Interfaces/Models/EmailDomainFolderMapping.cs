namespace Logic.Interfaces.Models
{
    public class EmailDomainFolderMapping 
    {
        public bool IsActive { get; set; }
        public string SourceDomain { get; set; } = string.Empty;
        public string TargetFolder { get; set; } = string.Empty;
        public string PredictedTargetFolder { get; set; } = string.Empty;
        public bool AutomatedCleanupEnabled { get; set; }
        public bool ForceDeleteSpamMails { get; set; }
        public List<EmailDomainModel> CurrentEmailData { get; set; } = new List<EmailDomainModel>();
    }
}
