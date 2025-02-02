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
                x.AccountId == accountId, [x => x.Account, x => x.Address, x => x.Subject, x => x.TargetFolder, x => x.PredictedTargetFolder]) ?? new List<EmailCleanupConfigurationEntity>();

            return configurationEntities ?? new List<EmailCleanupConfigurationEntity>();
        }

        public async Task<List<EmailCleanupConfigurationEntity>> LoadEntities(IEnumerable<int> configurationIds)
        {
            var configurationEntities = await _applicationUnitOfWork.EmailCleanupConfigurationTable.GetAllAsyncBy(x =>
                configurationIds.Contains(x.Id)) ?? new List<EmailCleanupConfigurationEntity>();

            return configurationEntities ?? new List<EmailCleanupConfigurationEntity>();
        }

        public async Task<List<EmailTargetFolderEntity>> GetAllTargetFolderEntities()
        {
            return await _applicationUnitOfWork.EmailTargetFolderTable.GetAllAsync();
        }

        public async Task UpdateConfigurations(List<EmailCleanupConfigurationEntity> configurationEntities)
        {
            configurationEntities.ForEach(e => _applicationUnitOfWork.EmailCleanupConfigurationTable.Update(e));

            await _applicationUnitOfWork.EmailCleanupConfigurationTable.SaveChangesAsync();
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
