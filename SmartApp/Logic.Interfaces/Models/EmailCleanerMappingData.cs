using Shared.Enums;

namespace Logic.Interfaces.Models
{
    public class EmailCleanerMappingData<TModel>
    {
        public string SettingsGuid { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public EmailProviderTypeEnum ProviderType { get; set; }
        public TModel? MappingData { get; set; }
    }
}
