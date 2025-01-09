using Data.Shared;
using Logic.Shared.Interfaces;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
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

        public async Task SendMailViaSmtp(string smtpServer, int port, string from, string password, string to, string subject, string body, string? html = null)
        {
            if(string.IsNullOrEmpty(smtpServer) || string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            using(var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, port);

                if (client.IsConnected) 
                {
                    await client.AuthenticateAsync(from, password);
                }

                if (!client.IsAuthenticated)
                {
                    await client.DisconnectAsync(true);

                    return;
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Smart-Application", from));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                message.Body = new BodyBuilder
                {
                    TextBody = "",
                    HtmlBody = body
                }.ToMessageBody();

                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }


        #region private members

       

        #endregion 
    }
}
