﻿using Data.Shared.AccessRights;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Databases.Seeds
{
    public class AccessRightSeed : IEntityTypeConfiguration<AccessRightEntity>
    {
        public void Configure(EntityTypeBuilder<AccessRightEntity> builder)
        {
            var entities = new List<AccessRightEntity>();
            var keys = AccessRights.AvailableAccessRights.Keys.ToList();

            foreach (var group in AccessRights.AvailableAccessRights.Keys)
            {
                var entry = AccessRights.AvailableAccessRights[group];

                entities.Add(new AccessRightEntity
                {
                    Id = keys.IndexOf(group) + 1,
                    Group = AccessRights.AvailableAccessRights[group],
                    Name = group,
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
