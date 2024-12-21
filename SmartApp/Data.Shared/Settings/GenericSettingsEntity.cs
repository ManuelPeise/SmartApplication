using Shared.Enums;

namespace Data.Shared.Settings
{
    public class GenericSettingsEntity : AEntityBase
    {
        public SettingsTypeEnum SettingsType { get; set; }
        public int UserId { get; set; }
        public string? SettingsJson { get; set; }
    }
}
