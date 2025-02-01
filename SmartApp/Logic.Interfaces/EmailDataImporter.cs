using Data.ContextAccessor.Interfaces;
using Data.Shared.Email;
using Logic.Interfaces.Models;

namespace Logic.Interfaces
{
    public class EmailDataImporter : IDisposable
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private bool disposedValue;

        public EmailDataImporter(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<Dictionary<string, int>> EnsureAllAddressEntitiesExists(List<EmailDataModel> mappingTableList)
        {
            var addressEntities = await _applicationUnitOfWork.EmailAddressTable.GetAllAsync();
            var addedEntities = new List<EmailAddressEntity>();

            foreach (var mapping in mappingTableList)
            {
                if (!addressEntities.Any(x => x.EmailAddress.ToLower() == mapping.FromAddress.ToLower())
                    && !addedEntities.Any(x => x.EmailAddress.ToLower() == mapping.FromAddress.ToLower()))
                {
                    var entity = new EmailAddressEntity
                    {
                        EmailAddress = mapping.FromAddress.Trim(),
                        Domain = mapping.FromAddress.Split('@')[1].Trim(),
                    };

                    await _applicationUnitOfWork.EmailAddressTable.AddAsync(entity);

                    addedEntities.Add(entity);
                }
            }

            if (addedEntities.Any())
            {
                await _applicationUnitOfWork.EmailAddressTable.SaveChangesAsync();
            }

            var updatedAddressEntities = await _applicationUnitOfWork.EmailAddressTable.GetAllAsync();

            return updatedAddressEntities.ToDictionary(e => e.EmailAddress, e => e.Id);
        }

        public async Task<Dictionary<string, int>> EnsureAllSubjectEntitiesExists(List<EmailDataModel> emailData)
        {
            var subjectEntities = await _applicationUnitOfWork.EmailSubjectTable.GetAllAsync();
            var newAddedSubjects = new List<string>();

            foreach (var email in emailData)
            {
                if (!subjectEntities.Any(x => x.EmailSubject == email.Subject) && !newAddedSubjects.Contains(email.Subject))
                {
                    var entity = new EmailSubjectEntity
                    {
                        EmailSubject = email.Subject,
                    };

                    newAddedSubjects.Add(email.Subject);

                    await _applicationUnitOfWork.EmailSubjectTable.AddAsync(entity);


                }
            }

            await _applicationUnitOfWork.EmailSubjectTable.SaveChangesAsync();

            var updatedSubjectEntities = await _applicationUnitOfWork.EmailSubjectTable.GetAllAsync();
            
            return updatedSubjectEntities.ToDictionary(e => e.EmailSubject, e => e.Id);
        }

        public async Task<Dictionary<string, int>> GetTargetFolderDictionary()
        {
            var folderEntities = await _applicationUnitOfWork.EmailTargetFolderTable.GetAllAsync();

            return folderEntities.ToDictionary(e => e.TargetFolderName, e => e.Id);
        }

        public async Task SaveEmailCleanupConfigurationEntities(List<EmailCleanupConfigurationEntity> entities, int userId, int accountId)
        {
            var entitiesToAdd = new List<EmailCleanupConfigurationEntity>();

            var existingConfigurations = await _applicationUnitOfWork.EmailCleanupConfigurationTable
                .GetAllAsyncBy(e => e.UserId == userId && e.AccountId == accountId);

            // check if entity to add already exists
            foreach (var entity in entities)
            {
                var existingEntity = existingConfigurations.FirstOrDefault(e => e.AccountId == entity.AccountId
                    && e.UserId == entity.UserId
                    && e.AddressId == entity.AddressId
                    && e.SubjectId == entity.SubjectId);

                if (existingEntity == null)
                {
                    entitiesToAdd.Add(entity);
                }
            }

            await _applicationUnitOfWork.EmailCleanupConfigurationTable.AddRange(entitiesToAdd);

            await _applicationUnitOfWork.EmailCleanupConfigurationTable.SaveChangesAsync();
        }

        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationUnitOfWork.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
