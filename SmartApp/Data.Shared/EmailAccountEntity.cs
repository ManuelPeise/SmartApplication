namespace Data.Shared
{
    public class EmailAccountEntity : AEntityBase
    {
        public int UserId { get; set; }
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 993;
        public string EmailAddress { get; set; } = string.Empty;
        public string EncodedPassword { get; set; } = string.Empty;
        public bool ConnectionEstablished { get; set; }
    }
}
