using Data.ContextAccessor;
using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Data.Shared.Email;
using Logic.Interfaces.Models;
using Logic.Interfaces.Repositories;

namespace Logic.Interfaces
{
    internal class FolderMappings : IDisposable
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly PasswordHandler _passwordHandler;
        private bool disposedValue;

        public FolderMappings(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _unitOfWork = applicationUnitOfWork;
            _passwordHandler = new PasswordHandler(applicationUnitOfWork.SecurityData);
        }


        public async Task<bool> UpdateFolderMappings(EmailFolderMappingUpdate folderMappingUpdate)
        {
            var entitiesToUpdate = await GetFolderMappingEntities(folderMappingUpdate.Mappings.Select(mapping => mapping.Id), folderMappingUpdate.SettingsGuid);

            if (entitiesToUpdate.Any())
            {
                foreach (var entity in entitiesToUpdate)
                {
                    var relatedMapping = folderMappingUpdate.Mappings.First(x => x.Id == entity.Id);

                    entity.IsActive = relatedMapping.IsActive;
                    entity.FolderId = relatedMapping.TargetFolderId;

                    _unitOfWork.EmailFolderMappingTable.Update(entity);
                }

                await _unitOfWork.EmailFolderMappingTable.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task ExecuteFolderMapping(EmailCleanerInterfaceConfiguration configuration)
        {
            var inboxResult = await LoadEmailsFromServer(configuration);

            var unmappedEmailDomains = await GetUnmappedDomainMailData(_unitOfWork.CurrentUserId, configuration.SettingsGuid, configuration);

            var availableAddresses = await EnsureAllAddressEntitiesExists(inboxResult);

            var folderDictionary = await GetFolderDictionary();

            var newFolderMappingEntities = new List<EmailFolderMappingEntity>();

            unmappedEmailDomains.ForEach(entry =>
            {
                newFolderMappingEntities.Add(new EmailFolderMappingEntity
                {
                    AddressId = availableAddresses[entry.FromAddress].Id,
                    UserId = _unitOfWork.CurrentUserId,
                    FolderId = 1,
                    PredictedFolderId = 1,
                    SettingsGuid = configuration.SettingsGuid,
                    SourceFolder = "INBOX",
                    IsActive = false,
                    ShouldCleanup = false,

                });
            });

            await _unitOfWork.EmailFolderMappingTable.AddRange(newFolderMappingEntities);

            await _unitOfWork.EmailFolderMappingTable.SaveChangesAsync();
        }

        public async Task<EmailFolderMappingData> GetFolderMappingData(EmailCleanerInterfaceConfiguration configuration)
        {


            var unmappedEmailDomains = await GetUnmappedDomainMailData(_unitOfWork.CurrentUserId, configuration.SettingsGuid, configuration);

            var availableAddresses = await GetAllAddressEntities();

            var folderDictionary = await GetFolderDictionary();

            var allAvailableMappings = await GetAllFolderMappings(_unitOfWork.CurrentUserId, configuration.SettingsGuid);

            return new EmailFolderMappingData
            {
                Folders = folderDictionary.Values.Select(folder => new EmailTargetFolder
                {
                    Id = folder.Id,
                    ResourceKey = folder.ResourceKey,
                }).ToList(),
                Mappings = allAvailableMappings.Select(entity => new FolderMapping
                {
                    Id = entity.Id,
                    UserId = entity.UserId,
                    SettingsGuid = entity.SettingsGuid,
                    SourceFolder = entity.SourceFolder,
                    Domain = entity.AddressEntity.Domain,
                    TargetFolderId = entity.FolderId,
                    PredictedTargetFolderId = entity.PredictedFolderId,
                    ShouldCleanup = entity.ShouldCleanup,
                    IsActive = entity.IsActive,

                }).ToList()
            };
        }

        public async Task<List<EmailDataModel>> GetMailsFromServer(EmailCleanerInterfaceConfiguration configuration)
        {
            return await LoadEmailsFromServer(configuration);
        }

        public async Task DeleteFolderMappings(int userId, string settingsGuid)
        {
            var folderMappings  = await _unitOfWork.EmailFolderMappingTable
                .GetAllAsyncBy(x => x.SettingsGuid == settingsGuid)?? new List<EmailFolderMappingEntity>();

            await _unitOfWork.EmailFolderMappingTable.DeleteRange(folderMappings);

            await _unitOfWork.EmailFolderMappingTable.SaveChangesAsync();
        }

        #region private mambers

        private async Task<List<EmailFolderMappingEntity>> GetFolderMappingEntities(IEnumerable<int> mappingIds, string settingsGuid)
        {
            return await _unitOfWork.EmailFolderMappingTable.GetAllAsyncBy(x => x.SettingsGuid == settingsGuid && mappingIds.Contains(x.Id));
        }

        private async Task<List<EmailDataModel>> LoadEmailsFromServer(EmailCleanerInterfaceConfiguration configuration)
        {
            var client = new EmailInterfaceEmailClient(_unitOfWork);

            var emailData = await client.LoadMailsFromServer(new EmailAccountConnectionData
            {
                Server = configuration.Server,
                Port = configuration.Port,
                EmailAddress = configuration.EmailAddress,
                Password = _passwordHandler.Decrypt(configuration.Password),
            });

            return emailData;
        }

        private async Task<List<EmailDataModel>> GetUnmappedDomainMailData(int userId, string settingdGuid, EmailCleanerInterfaceConfiguration configuration)
        {
            var inboxResult = await LoadEmailsFromServer(configuration);

            var existingMappings = await _unitOfWork.EmailFolderMappingTable.GetAllAsyncBy(x => x.UserId == userId && x.SettingsGuid == settingdGuid);

            var addressIds = existingMappings.Select(x => x.AddressId).Distinct().ToList();

            await _unitOfWork.EmailAddressTable.GetAllAsyncBy(x => addressIds.Contains(x.Id));

            var alreadyIncludedDomains = existingMappings
                .GroupBy(x => x.AddressEntity.EmailAddress.Split('@')[1])
                .Select(grp => grp.First().AddressEntity.EmailAddress.Split('@')[1])
                .ToList();

            return inboxResult.Where(result => !alreadyIncludedDomains.Contains(result.FromAddress.Split('@')[1]))
                .GroupBy(x => x.FromAddress.Split('@')[1])
                .Select(grp => grp.First())
                .ToList();
        }

        private async Task<Dictionary<string, EmailAddressEntity>> EnsureAllAddressEntitiesExists(List<EmailDataModel> mappingTableList)
        {
            var addressEntities = await _unitOfWork.EmailAddressTable.GetAllAsync();
            var existingAddresses = addressEntities.Select(x => x.EmailAddress.ToLower()).ToList();

            var entitiesToAdd = mappingTableList
                .Where(mapping => !existingAddresses.Contains(mapping.FromAddress.ToLower()))
                .Select(mapping => new EmailAddressEntity
                {
                    EmailAddress = mapping.FromAddress.Trim(),
                    Domain = mapping.FromAddress.Split('@')[1].Trim()
                })
                .GroupBy(mapping => mapping.EmailAddress)
                .Select(grp => grp.First())
                .ToList();


            if (entitiesToAdd.Any())
            {
                await _unitOfWork.EmailAddressTable.AddRange(entitiesToAdd);
                await _unitOfWork.EmailAddressTable.SaveChangesAsync();
            }

            addressEntities = await _unitOfWork.EmailAddressTable.GetAllAsync();

            return addressEntities.ToDictionary(x => x.EmailAddress);
        }

        private async Task<Dictionary<string, EmailAddressEntity>> GetAllAddressEntities()
        {
            var addressEntities = await _unitOfWork.EmailAddressTable.GetAllAsync();

            return addressEntities.ToDictionary(x => x.EmailAddress);
        }

        private async Task<Dictionary<int, EmailTargetFolderEntity>> GetFolderDictionary()
        {
            var folderEntities = await _unitOfWork.EmailTargetFolderTable.GetAllAsync();

            return folderEntities.ToDictionary(x => x.Id);
        }

        private async Task<List<EmailFolderMappingEntity>> GetAllFolderMappings(int userId, string settingsGuid)
        {
            return await _unitOfWork.EmailFolderMappingTable.GetAllAsyncBy(mapping => mapping.UserId == userId && mapping.SettingsGuid == settingsGuid);
        }



        #endregion

        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
