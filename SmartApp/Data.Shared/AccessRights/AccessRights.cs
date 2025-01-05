namespace Data.Shared.AccessRights
{
    public static class AccessRights
    {
        public static List<string> AvailableAccessRights = new List<string>
        {
            Administration,
            UserAdministration,
            Settings,
            EmailAccountSettings
        };

        public const string Administration = "Administration";
        public const string UserAdministration = "UserAdministration";
        public const string Settings = "Settings";
        public const string EmailAccountSettings = "EmailAccountSettings";
    }
}
