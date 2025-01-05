namespace Shared.Models.Administration.AccessRights
{
    public class UserAccessRightModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<AccessRight> AccessRights { get; set; } = new List<AccessRight>();
    }

    public class AccessRight
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool Deny { get; set; }
    }
}
