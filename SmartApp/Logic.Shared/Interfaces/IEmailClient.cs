namespace Logic.Shared.Interfaces
{
    public interface IEmailClient
    {
        Task<bool> IsConnectedToServer(string server, int port = 993);
        Task<bool> IsAuthenticated(string email, string decodedPassword);
        Task SendMailViaSmtp(string smtpServer, int port, string from, string password, string to, string subject, string body, string? html = null);
    }
}
