namespace Shared.Models.Settings
{
    public class AppSettingsModel
    {
        public string ImapServer { get; set; } = string.Empty;
        public int ImapPort { get; set; }
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SystemEmail { get; set; } = string.Empty;
        public string SystemEmailPassword { get; set; } = string.Empty;
        public string ContactMail { get; set; } = string.Empty;
        public MaintananceUser MaintananceUser { get; set; }

    }

    public class MaintananceUser
    {
        public string Name { get; set; } = "SystemAdmin";
        public string Role { get; set; } = "MaintananceUser";
    }
}