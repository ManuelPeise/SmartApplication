using Shared.Enums;

namespace Data.ContextAccessor.Interfaces
{
    public interface IModuleSettings
    {
        public string ModuleName { get; set; }
        public ModuleTypeEnum ModuleType { get; set; }
    }
}
