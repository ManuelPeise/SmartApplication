using Data.Shared.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Enums;

namespace Data.Identity.Seeds
{
    public class AdminUserModulesSeed : IEntityTypeConfiguration<UserModuleEntity>
    {
        public void Configure(EntityTypeBuilder<UserModuleEntity> builder)
        {
            var timeStamp = DateTime.Now;
            var defaultAdminUserId = 1;

            builder.HasData(new List<UserModuleEntity>
            {
                new UserModuleEntity {
                    Id = 1,
                    UserId = defaultAdminUserId,
                    ModuleId = GetModuleId(ModuleEnum.Administration),
                    HasReadAccess = true,
                    HasWriteAccess = true,
                    CreatedBy = "System",
                    CreatedAt = timeStamp,
                    UpdatedBy = "System",
                    UpdatedAt = timeStamp,
                }
            });
        }

        private int GetModuleId(ModuleEnum module)
        {
            return (int)module +1;
        }
    }
}
