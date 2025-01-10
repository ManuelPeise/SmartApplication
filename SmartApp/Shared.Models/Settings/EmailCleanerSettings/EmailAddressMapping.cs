namespace Shared.Models.Settings.EmailCleanerSettings
{
    public class EmailAddressMapping
    {
        public string? SourceAddress { get; set; }
        public string? Domain { get; set; }
        public bool ShouldCleanup { get; set; }
        public bool IsSpam { get; set; }
        public string? PredictedAs { get; set; }
    }
}
