namespace Data.Shared.Identity.Interfaces
{
    public interface IUserCredentials
    {
        public string Password { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
