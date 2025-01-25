using Data.ContextAccessor.Interfaces;
using Logic.Shared;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces.EmailAccountInterface.Models
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
