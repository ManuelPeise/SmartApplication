using Data.ContextAccessor.Interfaces;
using Shared.Enums;

namespace Data.ContextAccessor.ModuleSettings
{
    public class ModuleSettings : IModuleSettings
    {
        public string ModuleName { get; set; } = string.Empty;
        public ModuleTypeEnum ModuleType { get; set; }
    }

    public static class GenericSettigsModules
    {
        public const string EmailAccountInterfaceModuleName = "EmailAccountInterface";
        public const ModuleTypeEnum EmailAccountInterfaceModuleType = ModuleTypeEnum.EmailAccountInterface;

        public const string EmailCleanerInterfaceModuleName = "EmailCleanerInterface";
        public const ModuleTypeEnum EmailCleanerInterfaceModuleType = ModuleTypeEnum.EmailCleanerInterface;
    }
}
