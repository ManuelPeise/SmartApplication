using Data.ContextAccessor.Interfaces;
using Data.Shared.Email;
using Logic.Interfaces.Models;

namespace Logic.Interfaces.Repositories
{
    internal class EmailRepository
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public EmailRepository(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmailAddressEntity>> GetAddressEntities()
        {
            return await _unitOfWork.EmailAddressTable.GetAllAsync();
        }

        public async Task<Dictionary<string, EmailAddressEntity>> EnsureAllAddressEntitiesExists(List<EmailDataModel> mappingTableList)
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

        public async Task<List<EmailSubjectEntity>> GetAppSubjects()
        {
            return await _unitOfWork.EmailSubjectTable.GetAllAsync();
        }

        public async Task<Dictionary<string, EmailSubjectEntity>> EnsureAllSubjectEntitiesExists(List<EmailDataModel> mappingTableList)
        {
            var subjectEntities = await _unitOfWork.EmailSubjectTable.GetAllAsync();
            var existingSubjects = subjectEntities.Select(x => x.EmailSubject.ToLower()).ToList();

            var entitiesToAdd = mappingTableList
               .Where(mapping => !existingSubjects.Contains(mapping.Subject.ToLower()))
               .Select(mapping => new EmailSubjectEntity
               {
                   EmailSubject = mapping.Subject,
               })
               .GroupBy(mapping => mapping.EmailSubject)
               .Select(grp => grp.First())
               .ToList();


            if (entitiesToAdd.Any())
            {
                await _unitOfWork.EmailSubjectTable.AddRange(entitiesToAdd);
                await _unitOfWork.EmailSubjectTable.SaveChangesAsync();
            }
            subjectEntities = await _unitOfWork.EmailSubjectTable.GetAllAsync();

            return subjectEntities.ToDictionary(x => x.EmailSubject);
        }

        public async Task<List<EmailFolderMappingEntity>> GetAllFolderMappings(int userId, string settingsGuid)
        {
            return await _unitOfWork.EmailFolderMappingTable.GetAllAsyncBy(mapping => mapping.UserId == userId && mapping.SettingsGuid == settingsGuid);
        }

        public async Task<List<EmailTargetFolderEntity>> GetAllAvailableEmailFolders()
        {
            return await _unitOfWork.EmailTargetFolderTable.GetAllAsync();
        }

        public async Task<Dictionary<int, EmailTargetFolderEntity>> GetFolderDictionary()
        {
            var folderEntities = await _unitOfWork.EmailTargetFolderTable.GetAllAsync();

            return folderEntities.ToDictionary(x => x.Id);
        }

        public async Task<List<EmailDataModel>> GetUnmappedDomainMailData(int userId, string settingdGuid, List<EmailDataModel> inboxResult)
        {
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

        public async Task SaveNewFolderMappings(List<EmailFolderMappingEntity> newMappings)
        {
            await _unitOfWork.EmailFolderMappingTable.AddRange(newMappings);
        }
    }
}
