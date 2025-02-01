using Data.Shared.Email;

namespace Logic.Interfaces.Interfaces
{
    public interface IEmailCleanerImportModule: IDisposable
    {
        Task Import(EmailCleanerSettingsEntity? settingsEntity, int? accountId);
        Task ImportAll();
    }
}
