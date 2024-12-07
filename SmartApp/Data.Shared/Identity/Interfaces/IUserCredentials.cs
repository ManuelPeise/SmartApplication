namespace Data.Shared.Identity.Interfaces
{
    public interface IUserCredentials
    {
        public string Salt { get; set; }
        public string Password { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
