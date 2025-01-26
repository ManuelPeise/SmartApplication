using Data.ContextAccessor.Interfaces;
using Shared.Enums;

namespace Data.ContextAccessor.ModuleSettings
{
    public class ModuleSettings : IModuleSettings
    {
        public string ModuleName { get; set; } = string.Empty;
        public ModuleTypeEnum ModuleType { get; set; }
    }

    public static class EmailAccountInterfaceSettings
    {
        public const string ModuleName = "EmailAccountInterface";
        public const ModuleTypeEnum ModuleType = ModuleTypeEnum.EmailAccountInterface;
    }

    public static class EmailCleanerInterfaceSettings
    {
        public const string ModuleName = "EmailCleanerInterface";
        public const ModuleTypeEnum ModuleType = ModuleTypeEnum.EmailCleanerInterface;
    }
}
