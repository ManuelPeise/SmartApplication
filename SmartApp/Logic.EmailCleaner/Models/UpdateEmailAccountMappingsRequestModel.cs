namespace Shared.Models.Settings.EmailAccountMappings
{
    public class UpdateEmailAccountMappingsRequestModel
    {
        public int AccountId { get; set; }
        public List<string> Folders { get; set; } = new List<string>();
    }
}
