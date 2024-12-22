using Logic.Shared.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using Shared.Models.Administration.Email;
using Shared.Models.Ai;


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

        public async Task<bool> TestConnection(EmailProviderSettings settings)
        {
            var canConnect = false;

            using (var client = new ImapClient())
            {
                if (await IsConnected(client, settings))
                {
                    canConnect = true;

                    await DisconnectAsync(client);
                }
            }

            return canConnect;
        }

        public async Task<List<AiEmailTrainingData>> GetEmailAiTrainingDataModel(EmailProviderSettings settings, int maxMessages)
        {
            var data = new List<AiEmailTrainingData>();

            using (var client = new ImapClient())
            {
                if (await IsConnected(client, settings))
                {
                    await client.Inbox.OpenAsync(FolderAccess.ReadOnly);

                    if (!client.Inbox.IsOpen)
                    {
                        return data;
                    }

                    var messagesToProcess = client.Inbox.Count > maxMessages ? maxMessages : client.Inbox.Count;

                    for (int message = 0; message < messagesToProcess; message++)
                    {
                        var mail = await client.Inbox.GetMessageAsync(message);
                        var fromAddress = mail.From.Mailboxes.FirstOrDefault()?.Address;

                        if (fromAddress != null)
                        {
                            data.Add(new AiEmailTrainingData
                            {
                                From = fromAddress,
                                Domain = fromAddress.Split('@')[1],
                                Subject = mail.Subject,
                            });
                        }
                    }
                }
            }

            if (data.Any())
            {
                data = data.DistinctBy(entity => new { entity.From, entity.Subject }).ToList();
            }

            return data;
        }

        #region private members

        private async Task<bool> IsConnected(ImapClient client, EmailProviderSettings settings)
        {
            await client.ConnectAsync(settings.Provider.IMapServerAddress, (int)settings.Provider.ImapPort);

            if (!client.IsConnected)
            {
                return false;
            }

            await client.AuthenticateAsync(settings.EmailAddress, settings.Password);

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
