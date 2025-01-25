using Data.ContextAccessor.Interfaces;
using Logic.Interfaces.EmailAccountInterface.Models;
using Logic.Shared;
using MailKit;
using MailKit.Net.Imap;

namespace Logic.Interfaces.EmailAccountInterface
{
    internal class EmailAccountInterfaceClient
    {
        private Logger<EmailAccountInterfaceClient>? _logger;
        private bool _isConnected = false;
        private bool _isAuthenticated = false;

        public EmailAccountInterfaceClient(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _logger = new Logger<EmailAccountInterfaceClient>(applicationUnitOfWork);
        }

        public async Task<bool> ExecuteConnectionTest(EmailAccountConnectionTestRequest model)
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

        public async Task<List<EmailMappingModel>> LoadMailsFromServer(EmailAccountConnectionTestRequest model)
        {
            var models = new List<EmailMappingModel>();

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
                            models.Add(new EmailMappingModel
                            {
                                MessageId = dataSet.Envelope.MessageId,
                                MessageDate = dataSet.Envelope.Date.Value.UtcDateTime,
                                FromAddress = dataSet.Envelope.From.Mailboxes.First().Address,
                                Subject = dataSet.Envelope.Subject,
                                SourceFolder = dataSet.Folder.Name
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
