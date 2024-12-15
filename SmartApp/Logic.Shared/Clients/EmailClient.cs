using Data.Shared.Logging;
using Logic.Shared.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using Shared.Enums;
using Shared.Models.Administration.Email;

namespace Logic.Shared.Clients
{
    public class EmailClient : IEmailClient
    {
        private IAdministrationUnitOfWork _unitOfWork;
        private bool disposedValue;

        public EmailClient(IAdministrationUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }


        #region private members

        private async Task<List<MimeMessage>> LoadEmailsFromServer(EmailAccountSettings accountSettings)
        {
            using (var client = new ImapClient())
            {
                try
                {
                    if (!await ConnectAsync(client, accountSettings.EmailServerAddress, (int)accountSettings.Port) || !await AuthenticateAsync(client, accountSettings.EmailAddress, accountSettings.Password))
                    {
                        await _unitOfWork.AddLogMessage(new LogMessageEntity
                        {
                            Message = "Smtpclient could not Connected or Authenticated!",
                            TimeStamp = DateTime.UtcNow,
                            Module = nameof(EmailClient),
                            MessageType = LogMessageTypeEnum.Error,
                        });

                        return new List<MimeMessage>();
                    }

                    var emails = await ReceiveEmails(client);

                    if (emails.Any())
                    {
                        return emails;
                    }

                }
                catch (Exception exception)
                {
                    await _unitOfWork.AddLogMessage(new LogMessageEntity
                    {
                        Message = "Could not receive emails from server!",
                        ExceptionMessage = exception.Message,
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                        Module = nameof(EmailClient),
                    });
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        await DisconnectAsync(client);
                    }
                }

                return new List<MimeMessage>();
            }
        }
        private async Task<bool> ConnectAsync(ImapClient client, string server, int port)
        {
            await client.ConnectAsync(server, port);

            return client.IsConnected;
        }
        private async Task<bool> AuthenticateAsync(ImapClient client, string username, string password)
        {
            await client.AuthenticateAsync(username, password);
            return client.IsAuthenticated;
        }
        private async Task<List<MimeMessage>> ReceiveEmails(ImapClient client)
        {
            var messages = new List<MimeMessage>();

            var inbox = client.Inbox;

            await inbox.OpenAsync(FolderAccess.ReadOnly);

            if (inbox.IsOpen)
            {
                for (var mailIndex = 0; mailIndex < inbox.Count; mailIndex++)
                {
                    var mail = await inbox.GetMessageAsync(mailIndex);

                    messages.Add(mail);
                }

                return messages;
            }

            return new List<MimeMessage>();
        }
        private async Task DisconnectAsync(ImapClient client)
        {
            await client.DisconnectAsync(true);
        }

        #endregion

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
