namespace Data.Shared.Email
{
    public class EmailAccountSettingsEntity: AEntityBase
    {
        public string EmailServerAddress { get; set; }
        public int Port { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}
