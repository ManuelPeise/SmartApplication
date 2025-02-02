using Data.Shared.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Databases.Seeds
{
    internal class FolderName
    {
        public string ResourceKey { get; set; } = string.Empty;
        public string TargetFolderName { get; set; } = string.Empty;
       
    }

    public class EmailTargetFolderSeed : IEntityTypeConfiguration<EmailTargetFolderEntity>
    {
       
        
        // Health
        private readonly List<FolderName> _folderNames = new List<FolderName>
        {
            new FolderName{ ResourceKey="labelFolderUnknown", TargetFolderName="Unknown",},
            new FolderName{ ResourceKey="labelFolderFoodOrder", TargetFolderName="Food" },
            new FolderName{ ResourceKey="labelFolderTravel", TargetFolderName="Travel" },
            new FolderName{ ResourceKey="labelFolderTax", TargetFolderName="Tax" },
            new FolderName{ ResourceKey="labelFolderAccounts", TargetFolderName="Accounts" },
            new FolderName{ ResourceKey="labelFolderHealth", TargetFolderName="Health" },
            new FolderName{ ResourceKey="labelFolderRentAndReside", TargetFolderName="RentAndReside" },
            new FolderName{ ResourceKey="labelFolderArchiv", TargetFolderName="Archiv" },
            new FolderName{ ResourceKey="labelFolderSpam", TargetFolderName="Spam" },
            new FolderName{ ResourceKey="labelFolderFamilyAndFriends", TargetFolderName="FamilyAndFriends" },
            new FolderName{ ResourceKey="labelFolderShopping", TargetFolderName="Shopping" },
            new FolderName{ ResourceKey="labelFolderSocialMedia", TargetFolderName="SocialMedia" },
            new FolderName{ ResourceKey="labelFolderCar", TargetFolderName="Car" },
            new FolderName{ ResourceKey="labelFolderTelecommunication", TargetFolderName="Telecommunication" },
            new FolderName{ ResourceKey="labelFolderBankAndPayments", TargetFolderName="BankAndPayments" },
            new FolderName{ ResourceKey="labelFolderOther", TargetFolderName="Other" },
        };

        public void Configure(EntityTypeBuilder<EmailTargetFolderEntity> builder)
        {
            var entities = new List<EmailTargetFolderEntity>();

            int id = 0;

            foreach (var name in _folderNames)
            {
                entities.Add(new EmailTargetFolderEntity
                {
                    Id = ++id,
                    ResourceKey = name.ResourceKey,
                    TargetFolderName = name.TargetFolderName,
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
