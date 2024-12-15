namespace Shared.Models.Administration.Email
{
    public class EmailAccountSettings
    {
        public string? EmailServerAddress { get; set; }
        public int? Port { get; set; }
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
    }
}
