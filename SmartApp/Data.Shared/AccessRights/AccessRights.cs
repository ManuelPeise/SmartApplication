using Shared.Models.Administration.AccessRights;

namespace Data.Shared.AccessRights
{
    public static class AccessRights
    {
        public static Dictionary<string, string> AvailableAccessRights = new Dictionary<string, string>
        {
            { UserAdministration,  AdministrationGroup },
            { MessageLog,  AdministrationGroup },
            { EmailAccountInterface, InterfaceGroup },
           
        };

        public readonly static Dictionary<string, AccessRightValues> DefaultActivatedUserAccessRights = new Dictionary<string, AccessRightValues>
        {
            { EmailAccountInterface, new AccessRightValues{ Deny = false, CanView = true, CanEdit = true }},
        };

        public readonly static Dictionary<string, AccessRightValues> DefaultActivatedAdminAccessRights = new Dictionary<string, AccessRightValues>
        {
            { MessageLog, new AccessRightValues{ Deny = false, CanView = true, CanEdit = true }},
            { UserAdministration, new AccessRightValues{ Deny = false, CanView = true, CanEdit = true }},
            { EmailAccountInterface, new AccessRightValues{ Deny = false, CanView = true, CanEdit = true }},
            
        };

        // right groups
        public const string AdministrationGroup = "Administration";
        public const string InterfaceGroup = "Interface";
       
        // user rights
        public const string UserAdministration = "UserAdministration";
        public const string MessageLog = "MessageLog";
        public const string EmailAccountInterface = "EmailAccountInterface";
    }
}
