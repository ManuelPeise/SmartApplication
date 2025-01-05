using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Data.Shared.AccessRights;

namespace Data.AppContext.Configurations
{
    public class AccessRightConfiguration : IEntityTypeConfiguration<AccessRightEntity>
    {
        public void Configure(EntityTypeBuilder<AccessRightEntity> builder)
        {
            var entities = new List<AccessRightEntity>();

            for(int i = 0; i < AccessRights.AvailableAccessRights.Count; i++)
            {
                entities.Add(new AccessRightEntity
                {
                    Id = i + 1,
                    Name = AccessRights.AvailableAccessRights[i],
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
