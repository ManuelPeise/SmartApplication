using Data.ContextAccessor.Interfaces;
using Data.Shared.Email;
using Logic.Interfaces.Models;

namespace Logic.Interfaces
{
    internal class EmailDatabaseHandler
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public EmailDatabaseHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<Dictionary<string, EmailAddressEntity>> EnsureAllAddressEntitiesExists(List<EmailDataModel> mappingTableList)
        {
            var addressEntities = await _applicationUnitOfWork.EmailAddressTable.GetAllAsync();
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
                await _applicationUnitOfWork.EmailAddressTable.AddRange(entitiesToAdd);
                await _applicationUnitOfWork.EmailAddressTable.SaveChangesAsync();
            }

            addressEntities = await _applicationUnitOfWork.EmailAddressTable.GetAllAsync();

            return addressEntities.ToDictionary(x => x.EmailAddress);
        }

        public async Task<Dictionary<string, EmailSubjectEntity>> EnsureAllSubjectEntitiesExists(List<EmailDataModel> mappingTableList)
        {
            var subjectEntities = await _applicationUnitOfWork.EmailSubjectTable.GetAllAsync();
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
                await _applicationUnitOfWork.EmailSubjectTable.AddRange(entitiesToAdd);
                await _applicationUnitOfWork.EmailSubjectTable.SaveChangesAsync();
            }
            subjectEntities = await _applicationUnitOfWork.EmailSubjectTable.GetAllAsync();

            return subjectEntities.ToDictionary(x => x.EmailSubject);
        }

        public async Task<EmailMappingEntity?> GetEmailMappingEntityAsync(int userId, int addressId, int subjectId)
        {
            var entity = await _applicationUnitOfWork.EmailMappingTable.GetFirstOrDefault(x =>
                x.UserId == userId && x.AddressId == addressId && x.SubjectId == subjectId);

            return entity;
        }

        public EmailMappingEntity? GetEmailMappingEntity(int userId, int addressId, int subjectId)
        {
            var entity = _applicationUnitOfWork.EmailMappingTable.GetFirstOrDefault(x =>
                x.UserId == userId && x.AddressId == addressId && x.SubjectId == subjectId).Result;

            return entity;
        }

        public async Task<List<EmailMappingEntity>> GetEmailMappingEntities(int userId)
        {
            var entity = await _applicationUnitOfWork.EmailMappingTable.GetAllAsyncBy(x =>
                x.UserId == userId);

            return entity;
        }

        public async Task<List<EmailFolderMappingEntity>> GetUpdatedFolderMappings(List<EmailDataModel> emailsFromInbox, string settingsGuid)
        {
            var existingMappingEntities = await _applicationUnitOfWork.EmailFolderMappingTable
                .GetAllAsyncBy(x => x.SettingsGuid == settingsGuid && x.UserId == _applicationUnitOfWork.CurrentUserId);
            
            // load all folders
            var folderEntities = await _applicationUnitOfWork.EmailTargetFolderTable.GetAllAsync();

            var addressDictionary = await EnsureAllAddressEntitiesExists(emailsFromInbox);

            var mappingEntities = new List<EmailFolderMappingEntity>();

            // TODO group by domain part 
            emailsFromInbox.Where(x => !existingMappingEntities.Select(mapping => mapping?.AddressEntity.EmailAddress)
                .Contains(x.FromAddress)).ToList()
                .ForEach(mail =>
                {
                    mappingEntities.Add(new EmailFolderMappingEntity
                    {
                        UserId = _applicationUnitOfWork.CurrentUserId,
                        AddressId = addressDictionary[mail.FromAddress].Id,
                        FolderId = 1,
                        SettingsGuid = settingsGuid,
                        SourceFolder = "INBOX",
                       
                        IsActive = false,
                        ShouldCleanup = false,
                    });
                });

            if (mappingEntities.Any())
            {
                await _applicationUnitOfWork.EmailFolderMappingTable.AddRange(mappingEntities);

                await _applicationUnitOfWork.EmailFolderMappingTable.SaveChangesAsync();
            }

            return await _applicationUnitOfWork.EmailFolderMappingTable
                .GetAllAsyncBy(x => x.SettingsGuid == settingsGuid && x.UserId == _applicationUnitOfWork.CurrentUserId);
        }
    }
}
