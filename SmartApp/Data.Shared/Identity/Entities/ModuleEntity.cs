using Shared.Enums;

namespace Data.Shared.Identity.Entities
{
    public class ModuleEntity: AEntityBase
    {
        public string? ModuleName { get; set; }
        public ModuleEnum ModuleType { get; set; }
        public List<UserIdentity> Users { get; set; } = [];
    }
}
