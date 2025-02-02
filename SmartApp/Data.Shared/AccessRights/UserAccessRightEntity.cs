using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.AccessRights
{
    public class UserAccessRightEntity : AEntityBase
    {
        public int UserId { get; set; }
        public int AccessRightId { get; set; }
        public bool View { get; set; }
        public bool Edit { get; set; }
        public bool Deny { get; set; }

        [ForeignKey("AccessRightId")]
        public AccessRightEntity? AccessRight { get; set; }
       
    }
}
