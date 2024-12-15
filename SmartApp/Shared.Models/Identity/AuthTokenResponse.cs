namespace Shared.Models.Identity
{
    public class AuthTokenResponse
    {
        public string? Token { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
