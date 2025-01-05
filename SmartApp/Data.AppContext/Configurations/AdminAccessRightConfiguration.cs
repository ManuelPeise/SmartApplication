using Data.Shared.AccessRights;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.AppContext.Configurations
{

    public class AdminAccessRightConfiguration : IEntityTypeConfiguration<UserAccessRightEntity>
    {
        public void Configure(EntityTypeBuilder<UserAccessRightEntity> builder)
        {
            var entities = new List<UserAccessRightEntity>();

            for (int i = 0; i < AccessRights.AvailableAccessRights.Count; i++)
            {
                entities.Add(new UserAccessRightEntity
                {
                    Id = i + 1,
                    UserId = 1,
                    AccessRightId = i + 1,
                    Deny = false,
                    View = true,
                    Edit = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedAt = null,
                    UpdatedBy = null,
                });
            }

            builder.HasData(entities);
        }
    }
}
