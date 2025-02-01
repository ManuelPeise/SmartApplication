using Data.ContextAccessor.Interfaces;
using Data.Shared.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public class EmailCleanerConfiguration : IDisposable
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private bool disposedValue;

        public EmailCleanerConfiguration(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<List<EmailCleanupConfigurationEntity>> LoadEntities(int accountId)
        {
            var configurationEntities = await _applicationUnitOfWork.EmailCleanupConfigurationTable.GetAllAsyncBy(x =>
                x.AccountId == accountId) ?? new List<EmailCleanupConfigurationEntity>();

            await _applicationUnitOfWork.EmailAccountsTable.GetFirstOrDefault(x => x.Id == accountId);

            await LoadFolders(configurationEntities.Select(e => e.TargetFolderId));

            await LoadFolders(configurationEntities.Where(e => e.PredictedTargetFolderId != null).Select(e => (int)e.PredictedTargetFolderId));
           
            var addressIds = configurationEntities.Select(x => x.AddressId).Distinct().ToList();

            await _applicationUnitOfWork.EmailAddressTable.GetAllAsyncBy(e => addressIds.Contains(e.Id));

            var subjectIds = configurationEntities.Select(x => x.SubjectId).Distinct().ToList();

            await _applicationUnitOfWork.EmailSubjectTable.GetAllAsyncBy(e => subjectIds.Contains(e.Id));

            return configurationEntities?? new List<EmailCleanupConfigurationEntity>();
        }

        public async Task<List<EmailTargetFolderEntity>> GetAllTargetFolderEntities()
        {
            return await _applicationUnitOfWork.EmailTargetFolderTable.GetAllAsync();
        }

        #region private

        private async Task LoadFolders(IEnumerable<int> folderIds)
        {
            await _applicationUnitOfWork.EmailTargetFolderTable.GetAllAsyncBy(e => folderIds.Contains(e.Id));
        }

        #endregion


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
