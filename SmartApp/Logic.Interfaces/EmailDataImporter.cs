using Data.ContextAccessor.Interfaces;
using Data.Shared.Email;
using Logic.Interfaces.Models;
using System.Collections.Generic;

namespace Logic.Interfaces
{
    public class EmailDataImporter: IDisposable
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
                addressEntities.AddRange(addedEntities);
            }

            return addressEntities.ToDictionary(e => e.EmailAddress, e => e.Id);
        }

        public async Task<Dictionary<string, int>> EnsureAllSubjectEntitiesExists(List<EmailDataModel> mappingTableList)
        {
            var subjectEntities = await _applicationUnitOfWork.EmailSubjectTable.GetAllAsync();
            var addedEntities = new List<EmailSubjectEntity>();

            foreach (var mapping in mappingTableList)
            {
                if (!subjectEntities.Any(x => x.EmailSubject.ToLower() == mapping.Subject.ToLower())
                    && !addedEntities.Any(x => x.EmailSubject.ToLower() == mapping.Subject.ToLower()))
                {
                    var entity = new EmailSubjectEntity
                    {
                        EmailSubject = mapping.Subject,
                    };

                    await _applicationUnitOfWork.EmailSubjectTable.AddAsync(entity);

                    addedEntities.Add(entity);
                }
            }

            if (addedEntities.Any())
            {
                await _applicationUnitOfWork.EmailSubjectTable.SaveChangesAsync();
                subjectEntities.AddRange(addedEntities);
            }

            return subjectEntities.ToDictionary(e => e.EmailSubject, e => e.Id);
        }

        public async Task<Dictionary<string, int>> GetTargetFolderDictionary()
        {
            var folderEntities = await _applicationUnitOfWork.EmailTargetFolderTable.GetAllAsync();

            return folderEntities.ToDictionary(e => e.TargetFolderName, e => e.Id);
        }

        public async Task SaveEmailCleanupConfigurationEntities(List<EmailCleanupConfigurationEntity> entities)
        {
            await _applicationUnitOfWork.EmailCleanupConfigurationTable.AddRange(entities);

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
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
