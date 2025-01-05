using Data.Shared;
using Newtonsoft.Json;
using Shared.Models.Settings.EmailAccountSettings;

namespace Logic.Settings.Extensions
{
    internal static class EmailAccountSettingsExtensions
    {
        internal static EmailAccountSettingsModel ToModel(this EmailAccountEntity entity)
        {
            return new EmailAccountSettingsModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                AccountName = entity.AccountName,
                ProviderType = entity.ProviderType,
                Server = entity.Server,
                Port = entity.Port,
                EmailAddress = entity.EmailAddress,
                Password = entity.EncodedPassword,
                
            };
        }

        internal static EmailAccountSettingsModel ToUiModel(this EmailAccountEntity entity)
        {
            return new EmailAccountSettingsModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                AccountName = entity.AccountName,
                ProviderType = entity.ProviderType,
                Server = entity.Server,
                Port = entity.Port,
                EmailAddress = entity.EmailAddress,
                Password = null,
                MessageLog = !string.IsNullOrWhiteSpace(entity.MessageLogJson) ? JsonConvert.DeserializeObject<MessageLog>(entity.MessageLogJson) : null,
            };
        }

        internal static EmailAccountEntity ToUpdatedEntity(this EmailAccountSettingsModel model, string encodedPassword, PasswordHandler passwordHandler, string logMessageJson)
        {
            return new EmailAccountEntity
            {
                Id = model.Id,
                UserId = model.UserId,
                AccountName = model.AccountName,
                ProviderType = model.ProviderType,
                Server = model.Server,
                Port = model.Port,
                EmailAddress = model.EmailAddress,
                EncodedPassword = model.Password == null ? encodedPassword : passwordHandler.Encrypt(model.Password),
                MessageLogJson = logMessageJson
            };
        }
    }
}
