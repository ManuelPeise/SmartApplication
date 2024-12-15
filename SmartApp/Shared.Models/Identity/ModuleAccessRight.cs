using Shared.Enums;

namespace Shared.Models.Identity
{
    public class ModuleAccessRight
    {
        public string? ModuleName { get; set; }
        public ModuleEnum ModuleType { get; set; }
        public bool Deny { get; set; }
        public bool HasReadAccess { get; set; }
        public bool HasWriteAccess { get; set; }
    }
}
