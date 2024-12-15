using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Identity.Entities
{
    public class UserModuleEntity: AEntityBase
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserIdentity User { get; set; }
        public int ModuleId { get; set; }
        [ForeignKey(nameof(ModuleId))]
        public ModuleEntity Module { get; set; }
        public bool Deny { get; set; }
        public bool HasReadAccess { get; set; }
        public bool HasWriteAccess { get; set; }
    }
}
