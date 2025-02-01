using Shared.Enums;

namespace Data.Shared.Email
{
    public class EmailAccountEntity:AEntityBase
    {
        public int UserId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string ImapServer { get; set; } = string.Empty;
        public int ImapPort { get; set; } = 993;
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public EmailProviderTypeEnum ProviderType { get; set; }
        public bool ConnectionTestPassed { get; set; }
    }
}
