namespace Logic.Shared.Interfaces
{
    public interface IEmailClient
    {
        Task<bool> IsConnectedToServer(string server, int port = 993);
        Task<bool> IsAuthenticated(string email, string decodedPassword);
    }
}
