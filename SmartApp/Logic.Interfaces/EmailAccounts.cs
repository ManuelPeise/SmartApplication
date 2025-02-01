using Data.ContextAccessor.Interfaces;
using Data.Shared.Email;

namespace Logic.Interfaces
{
    public class EmailAccounts: IDisposable
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private bool disposedValue;

        public EmailAccounts(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _unitOfWork = applicationUnitOfWork;
        }

        public async Task<List<EmailAccountEntity>> LoadUserEmailAccounts()
        {
            return await _unitOfWork.EmailAccountsTable.GetAllAsyncBy(x => x.UserId == _unitOfWork.CurrentUserId)?? new List<EmailAccountEntity>();
        }

        public async Task<EmailAccountEntity?> LoadUserEmailAccount(int accountId)
        {
            return await _unitOfWork.EmailAccountsTable.GetFirstOrDefault(x => x.Id == accountId) ?? null;
        }

        public async Task SaveAccount(EmailAccountEntity entity)
        {
            await _unitOfWork.EmailAccountsTable.AddAsync(entity);

            await _unitOfWork.EmailAccountsTable.SaveChangesAsync();
        }

        public async Task UpdateAccount(EmailAccountEntity entity)
        {
            _unitOfWork.EmailAccountsTable.Update(entity);

            await _unitOfWork.EmailAccountsTable.SaveChangesAsync();
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
