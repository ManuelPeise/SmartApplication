using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Data.Shared.Email;
using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
using Logic.Shared;
using Org.BouncyCastle.Crypto.Prng;
using Shared.Enums;

namespace Logic.Interfaces.EmailAccountInterface
{
    public class EmailAccountInterfaceModule : IEmailAccountInterfaceModule
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly PasswordHandler _passwordHandler;
        private Logger<EmailAccountInterfaceModule>? _logger;

        private bool disposedValue;

        public EmailAccountInterfaceModule(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _passwordHandler = new PasswordHandler(applicationUnitOfWork.SecurityData);
            _logger = new Logger<EmailAccountInterfaceModule>(applicationUnitOfWork);
        }

        public async Task<List<EmailAccount>> GetEmailAccounts()
        {
            try
            {
                if (!_applicationUnitOfWork.IsAuthenticated)
                {
                    throw new Exception("Could not get email account settings, reason: unauthenticated!");
                }

                using (var accounts = new EmailAccounts(_applicationUnitOfWork))
                {
                    var existingAccounts = await accounts.LoadUserEmailAccounts();

                    if (!existingAccounts.Any())
                    {
                        var defaultAccountModel = GetDefaultEmailAccountModel();

                        return new List<EmailAccount> { defaultAccountModel };
                    }


                    return existingAccounts
                        .Select(acc => new EmailAccount
                        {
                            AccountId = acc.Id,
                            AccountName = acc.AccountName,
                            ImapServer = acc.ImapServer,
                            ImapPort = acc.ImapPort,
                            EmailAddress = acc.EmailAddress,
                            Password = acc.Password,
                            ConnectionTestPassed = acc.ConnectionTestPassed,
                            ProviderType = acc.ProviderType
                        }).ToList();
                }
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error("Could not load settings.", exception.Message);
                }
            }

            return new List<EmailAccount>();
        }

        public async Task<bool> UpdateEmailAccount(EmailAccount account)
        {
            try
            {
                if (!_applicationUnitOfWork.IsAuthenticated)
                {
                    throw new Exception("Could not update email account settings, reason: unauthenticated!");
                }

                using (var accounts = new EmailAccounts(_applicationUnitOfWork))
                {
                    var client = new EmailInterfaceEmailClient(_applicationUnitOfWork);
                    var accountEntity = await accounts.LoadUserEmailAccount(account.AccountId);

                    if (accountEntity == null)
                    {
                        accountEntity = new EmailAccountEntity
                        {
                            UserId = _applicationUnitOfWork.CurrentUserId,
                            ProviderType = account.ProviderType,
                            AccountName = account.AccountName,
                            EmailAddress = account.EmailAddress,
                            ImapServer = account.ImapServer,
                            ImapPort = account.ImapPort,
                            Password = _passwordHandler.Encrypt(account.Password),
                            ConnectionTestPassed = account.ConnectionTestPassed,
                        };

                        await accounts.SaveAccount(accountEntity);

                        return true;
                    }

                    accountEntity.AccountName = account.AccountName;
                    accountEntity.ImapServer = account.ImapServer;
                    accountEntity.ImapPort = account.ImapPort;
                    accountEntity.EmailAddress = account.EmailAddress;
                    accountEntity.ProviderType = account.ProviderType;

                    if (account.PasswordDiffers(accountEntity.Password))
                    {
                        accountEntity.Password = _passwordHandler.Encrypt(account.Password);
                    }

                    if (_logger != null)
                    {
                        await _logger.Info($"Email account updated [{account.AccountId}].");
                    }

                    return true;
                }
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error($"Could not update account settings [{account.AccountId}].", exception.Message);
                }

                return false;
            }
        }

        public async Task<bool> ExcecuteConnectionTest(EmailAccountConnectionData model)
        {
            try
            {
                if (!_applicationUnitOfWork.IsAuthenticated)
                {
                    throw new Exception("Could not execute connection test, reason: unauthenticated!");
                }

                using (var accounts = new EmailAccounts(_applicationUnitOfWork))
                {
                    var client = new EmailInterfaceEmailClient(_applicationUnitOfWork);

                    var accountEntity = await accounts.LoadUserEmailAccount(model.AccountId);

                    if (accountEntity == null)
                    {
                        return await client.ExecuteConnectionTest(model);
                    }

                    accountEntity.ConnectionTestPassed = await client.ExecuteConnectionTest(model);

                    await accounts.UpdateAccount(accountEntity);

                    return true;
                }
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error($"Could test account connection for [{model.EmailAddress}].", exception.Message);
                }

                return false;
            }
        }

        #region private 

        private EmailAccount GetDefaultEmailAccountModel()
        {
            return new EmailAccount
            {
                AccountName = string.Empty,
                ImapServer = string.Empty,
                ImapPort = -1,
                EmailAddress = string.Empty,
                Password = string.Empty,
                ProviderType = EmailProviderTypeEnum.None,
                ConnectionTestPassed = false,
            };
        }

        #endregion

        #region dispose 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationUnitOfWork?.Dispose();
                }

                _logger = null;
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
