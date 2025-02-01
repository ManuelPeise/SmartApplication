namespace Logic.Interfaces.Models
{
    public class EmailAccountConnectionData
    {
        public int AccountId { get; set; }
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
