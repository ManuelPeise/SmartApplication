using Data.ContextAccessor.Interfaces;
using Logic.Interfaces.Models;
using Logic.Shared;
using MailKit;
using MailKit.Net.Imap;

namespace Logic.Interfaces
{
    internal class EmailInterfaceEmailClient
    {
        private Logger<EmailInterfaceEmailClient>? _logger;
        private bool _isConnected = false;
        private bool _isAuthenticated = false;

        public EmailInterfaceEmailClient(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _logger = new Logger<EmailInterfaceEmailClient>(applicationUnitOfWork);
        }

        public async Task<bool> ExecuteConnectionTest(EmailAccountConnectionData model)
        {
            try
            {
                using (var client = new ImapClient())
                {
                    await ConnectToServer(client, model.Server, model.Port);

                    if (!_isConnected)
                    {
                        throw new Exception("Connect to IMAP server failed.");
                    }
                    var connected = _isConnected;

                    await Authenticate(client, model.EmailAddress, model.Password);

                    await client.DisconnectAsync(connected);

                    return _isConnected;
                }
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error("Could not execute email account connection test.", exception.Message);
                }

                return false;
            }
        }

        public async Task<List<EmailDataModel>> LoadMailsFromServer(EmailAccountConnectionData model)
        {
            var models = new List<EmailDataModel>();

            try
            {
                using (var client = new ImapClient())
                {
                    await ConnectToServer(client, model.Server, model.Port);

                    if (!_isConnected)
                    {
                        throw new Exception("Connect to IMAP server failed.");
                    }

                    await Authenticate(client, model.EmailAddress, model.Password);

                    if (!_isAuthenticated)
                    {
                        throw new Exception("Authentication on IMAP server failed.");
                    }

                    await client.Inbox.OpenAsync(FolderAccess.ReadOnly);

                    if (!client.Inbox.IsOpen)
                    {
                        throw new Exception("Open inbox on IMAP server failed.");
                    }

                    var envelopeData = await client.Inbox.FetchAsync(0, -1, MessageSummaryItems.Envelope);

                    foreach (var dataSet in envelopeData)
                    {
                        if (!string.IsNullOrEmpty(dataSet.Envelope.Subject) && dataSet.Envelope.Date != null)
                        {
                            models.Add(new EmailDataModel
                            {
                                MessageId = dataSet.Envelope.MessageId,
                                FromAddress = dataSet.Envelope.From.Mailboxes.First().Address,
                                Subject = dataSet.Envelope.Subject,
                            });
                        }
                    }

                    await client.DisconnectAsync(_isConnected);
                }

                return models;
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error("Could not execute email account connection test.", exception.Message);
                }

                return models;
            }
        }

        private async Task ConnectToServer(ImapClient client, string server, int port)
        {
            await client.ConnectAsync(server, port);

            _isConnected = client.IsConnected;
        }

        private async Task Authenticate(ImapClient client, string email, string password)
        {
            await client.AuthenticateAsync(email, password);

            _isAuthenticated = client.IsAuthenticated;
        }
    }
}
