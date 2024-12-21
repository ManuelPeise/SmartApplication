using Shared.Enums;

namespace Shared.Models.Administration.Email
{
    public class EmailProviderSettings
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public EmailProvider Provider { get; set; } = new EmailProvider();
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public EmailProviderConfigurationStatusEnum Status { get; set; }
        public bool ConnectionTestPassed { get; set; }
        public EmailProviderConnectionInfo? ConnectionInfo { get; set; }
        public string UpdatePasswordIfDiffers(string prevPassword, EncriptPasswordAction encrypt)
        {
            if(prevPassword != Password)
            {
                return encrypt(Password);
            }

            return prevPassword;
        }
    }

    public delegate string EncriptPasswordAction(string passWord);
}
