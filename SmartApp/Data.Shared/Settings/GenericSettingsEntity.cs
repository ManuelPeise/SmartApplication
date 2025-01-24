using Shared.Enums;

namespace Data.Shared.Settings
{
    public class GenericSettingsEntity:AEntityBase
    {
        public int UserId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public ModuleTypeEnum ModuleType { get; set; }
        public string? SettingsJson { get; set; }
    }
}
