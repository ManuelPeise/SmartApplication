using MimeKit;
using Shared.Models.Administration.Email;

namespace Logic.Shared.Interfaces
{
    public interface IEmailClient: IDisposable
    {
        Task<bool> TestConnection(EmailProviderSettings settings);
       
    }
}
