﻿using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Data.Shared.Email;
using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
using Logic.Shared;
using Shared.Enums;

namespace Logic.Interfaces.EmailCleaner
{
    public class EmailCleanerImportModule : IEmailCleanerImportModule
    {

        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly PasswordHandler _passwordHandler;
        private Logger<EmailCleanerImportModule>? _logger;


        public EmailCleanerImportModule(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _passwordHandler = new PasswordHandler(applicationUnitOfWork.SecurityData);
            _logger = new Logger<EmailCleanerImportModule>(applicationUnitOfWork);
        }


        public async Task Import(EmailCleanerSettingsEntity? settingsEntity, int? accountId)
        {
            try
            {
                var emailClient = new EmailInterfaceEmailClient(_applicationUnitOfWork);

                if (settingsEntity == null)
                {
                    using (var settings = new EmailCleanerSettings(_applicationUnitOfWork))
                    {
                        var settingsEntities = await settings.GetEmailCleanerSettings();

                        foreach (var entity in settingsEntities)
                        {
                            
                            if(accountId != null && entity.AccountId == accountId)
                            {
                                await Import(entity, entity.AccountId);
                            }
                            
                        }
                    }

                    return;
                }

                using (var importer = new EmailDataImporter(_applicationUnitOfWork))
                {
                    var account = await importer.LoadAccountEntity(settingsEntity.AccountId);

                    var emailData = await emailClient.LoadMailsFromServer(new EmailAccountConnectionData
                    {
                        AccountId = account.Id,
                        Server = account.ImapServer,
                        Port = account.ImapPort,
                        EmailAddress = account.EmailAddress,
                        Password = _passwordHandler.Decrypt(account.Password)
                    });

                    var validatedEmailData = emailData
                        .Where(e => !string.IsNullOrEmpty(e.Subject))
                        .ToList();

                    var addressIdDictionary = await importer.EnsureAllAddressEntitiesExists(validatedEmailData);
                    var subjectDictionary = await importer.EnsureAllSubjectEntitiesExists(validatedEmailData);
                    var targetFolderDictionary = await importer.GetTargetFolderDictionary();

                    var emailCleanupConfigurationEntities = validatedEmailData.Select(data => new EmailCleanupConfigurationEntity
                    {
                        UserId = settingsEntity.UserId,
                        AccountId = settingsEntity.AccountId,
                        AddressId = addressIdDictionary[data.FromAddress],
                        SubjectId = subjectDictionary[data.Subject],
                        TargetFolderId = targetFolderDictionary["Unknown"],
                        IsSharedWithAi = settingsEntity.ShareDataWithAi, 
                        IsSpam = false,
                        IsPredictedAsSpam = false,
                        Backup = false,
                        Delete = false,
                    }).ToList();

                    if (emailCleanupConfigurationEntities.Any())
                    {
                        await importer.SaveEmailCleanupConfigurationEntities(emailCleanupConfigurationEntities, settingsEntity.UserId, settingsEntity.AccountId);

                        if (_logger != null)
                        {
                            await _logger.Info($"Email data for {settingsEntity.UserId} / {settingsEntity.AccountId} imported with success.");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error($"Import email data failed!", exception.Message);
                }
            }
        }

        public async Task ImportAll()
        {
            using (var settings = new EmailCleanerSettings(_applicationUnitOfWork))
            {
                var activeEmailCleanerSettings = await settings.GetAllActiveSettings();

                foreach (var settingsEntity in activeEmailCleanerSettings)
                {
                    await Import(settingsEntity, settingsEntity.AccountId);
                }
            }
        }


        #region dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationUnitOfWork?.Dispose();
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
