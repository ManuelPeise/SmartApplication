using Shared.Enums;

namespace Logic.Interfaces.Models
{
    public class EmailAccount
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public EmailProviderTypeEnum ProviderType { get; set; }
        public string ImapServer { get; set; } = string.Empty;
        public int ImapPort { get; set; } = 993;
        public string EmailAddress { get; set; } = string.Empty;
        public string? Password { get; set; }
        public bool ConnectionTestPassed { get; set; }
        
        public bool PasswordDiffers(string password)
        {
            return !password.Equals(Password);
        }
    }
}
