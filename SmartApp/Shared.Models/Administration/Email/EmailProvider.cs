using Shared.Enums;

namespace Shared.Models.Administration.Email
{
    public class EmailProvider
    {
        public EmailProviderTypeEnum ProviderType { get; set; }
        public string IMapServerAddress { get; set; } = string.Empty;
        public int? ImapPort { get; set; }
        public string Logo { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
