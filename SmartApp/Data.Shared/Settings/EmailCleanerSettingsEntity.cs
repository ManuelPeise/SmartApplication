namespace Data.Shared.Settings
{
    public class EmailCleanerSettingsEntity: AEntityBase
    {
        public int UserId { get; set; }
        public string ImapServer { get; set; } = string.Empty;
        public int ImapPort { get; set; } = 993;
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool ConnectionTestPassed { get; set; } = false;
        public bool ConnectionEstablished { get; set; } = false;
        public bool IsInitialized { get; set; } = false;
        public string SettingsJson { get; set; } = string.Empty;
    }
}
