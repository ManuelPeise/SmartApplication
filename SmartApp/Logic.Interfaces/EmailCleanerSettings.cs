using Data.ContextAccessor.Interfaces;
using Data.Shared.Email;

namespace Logic.Interfaces
{
    public class EmailCleanerSettings : IDisposable
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private bool disposedValue;


        public EmailCleanerSettings(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _unitOfWork = applicationUnitOfWork;
        }

        public async Task<List<EmailCleanerSettingsEntity>> GetEmailCleanerSettings(bool loadAccounts = false)
        {
            var settingsEntities = await _unitOfWork.EmailCleanerSettingsTable.GetAllAsyncBy(x =>
                x.UserId == _unitOfWork.CurrentUserId) ?? new List<EmailCleanerSettingsEntity>();

            if (loadAccounts)
            {
                var accountIds = settingsEntities.Select(x => x.AccountId).ToList();

                var accountsEntities = await _unitOfWork.EmailAccountsTable.GetAllAsyncBy(e => accountIds.Contains(e.Id));

                foreach (var entity in settingsEntities)
                {
                    var accountEntity = accountsEntities.FirstOrDefault(e => e.Id == entity.AccountId);

                    if (accountEntity != null)
                    {
                        entity.Account = accountEntity;
                    }
                }
            }

            return settingsEntities;
        }

        public async Task<EmailCleanerSettingsEntity?> GetEmailCleanerSetting(int settingId)
        {
            return await _unitOfWork.EmailCleanerSettingsTable.GetFirstOrDefault(e => e.Id == settingId);
        }

        public async Task UpdateEmailCleanerSetting(EmailCleanerSettingsEntity entity)
        {
            _unitOfWork.EmailCleanerSettingsTable.Update(entity);

        }

        public async Task CreateEmailCleanerSettings()
        {
            var accountEntities = await _unitOfWork.EmailAccountsTable.GetAllAsyncBy(e => 
                e.UserId == _unitOfWork.CurrentUserId)?? new List<EmailAccountEntity>();

            foreach (var accountEntity in accountEntities)
            {
                var entity = new EmailCleanerSettingsEntity
                {
                    AccountId = accountEntity.Id,
                    UserId = accountEntity.UserId,
                    EmailCleanerEnabled = false
                };

                await _unitOfWork.EmailCleanerSettingsTable.AddAsync(entity);
            }

            await _unitOfWork.EmailCleanerSettingsTable.SaveChangesAsync();
        }

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
