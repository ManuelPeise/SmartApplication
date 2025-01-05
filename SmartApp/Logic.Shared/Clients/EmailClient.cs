using Data.Shared;
using Logic.Shared.Interfaces;
using MailKit.Net.Imap;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;


namespace Logic.Shared.Clients
{
    public class EmailClient : IEmailClient
    {

        private readonly PasswordHandler _passwordHandler;
        private readonly ImapClient _imapClient;
        public EmailClient(IOptions<SecurityData> securityData)
        {
            _passwordHandler = new PasswordHandler(securityData);
            _imapClient = new ImapClient();
        }

        public async Task<bool> IsConnectedToServer(string server, int port = 993)
        {
            await _imapClient.ConnectAsync(server, port);

            return _imapClient.IsConnected;
        }

        public async Task<bool> IsAuthenticated(string email, string decodedPassword)
        {
            await _imapClient.AuthenticateAsync(email, decodedPassword);

            return _imapClient.IsAuthenticated;
        }

        #region private members

       

        #endregion 
    }
}
