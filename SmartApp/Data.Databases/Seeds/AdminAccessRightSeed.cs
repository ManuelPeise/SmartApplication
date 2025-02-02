using Data.Shared.AccessRights;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Databases.Seeds
{
    public class AdminAccessRightSeed : IEntityTypeConfiguration<UserAccessRightEntity>
    {
        public void Configure(EntityTypeBuilder<UserAccessRightEntity> builder)
        {
            var entities = new List<UserAccessRightEntity>();
            var keys = AccessRights.AvailableAccessRights.Keys.ToList();

            foreach (var key in keys)
            {
                entities.Add(new UserAccessRightEntity
                {
                    Id = keys.IndexOf(key) + 1,
                    UserId = 1,
                    AccessRightId = keys.IndexOf(key) + 1,
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
