using Shared.Models.Administration.AccessRights;

namespace Shared.Models.Administration
{
    public class UserAdministrationUserModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public List<AccessRight> AccessRights { get; set; } = new List<AccessRight>();
    }
}
