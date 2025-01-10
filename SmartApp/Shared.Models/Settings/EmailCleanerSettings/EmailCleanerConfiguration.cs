namespace Shared.Models.Settings.EmailCleanerSettings
{
    public class EmailCleanerConfiguration
    {
        public int UserId { get; set; }
        public List<EmailCleanerAccount> Accounts { get; set; } = new List<EmailCleanerAccount>();
        public List<EmailCleanerSettings> Settings { get; set; } = new List<EmailCleanerSettings>(); 
    }
}
