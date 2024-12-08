namespace Data.Shared.Identity.Entities
{
    public class AccountRegistrationRequestEntity: AEntityBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsGranded { get; set; }
    }
}
