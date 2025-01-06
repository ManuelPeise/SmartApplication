namespace Data.Shared.AccessRights
{
    public static class AccessRights
    {
        public static Dictionary<string, string> AvailableAccessRights = new Dictionary<string, string>
        {
            { UserAdministration,  Administration },
            { MessageLog,  Administration },
            { EmailAccountSettings,  Settings }


        };

        public const string Administration = "Administration";
        public const string UserAdministration = "UserAdministration";
        public const string MessageLog = "MessageLog";
        public const string Settings = "Settings";
        public const string EmailAccountSettings = "EmailAccountSettings";
    }
}
