using Shared.Models.Administration.Email;

namespace Logic.Shared.Interfaces
{
    public interface IEmailProviderConfiguration: IDisposable
    {
        Task<List<EmailProviderSettings>> GetSettings();
        Task<bool> TestConnection(EmailProviderSettings settings);
        Task<bool> OnEstablishConnection(EmailProviderSettings settings);
        Task<bool> DeleteConnection(int connectionId);
    }
}
