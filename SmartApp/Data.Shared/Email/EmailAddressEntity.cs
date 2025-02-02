using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Email
{
    public class EmailAddressEntity: AEntityBase
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
    }
}
