namespace Logic.Interfaces.Models
{
    internal class EmailMappingModel
    {
        public string MessageId { get; set; } = string.Empty;
        public DateTime MessageDate { get; set; }
        public string FromAddress { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string SourceFolder { get; set; } = string.Empty;
        public bool IsNew { get; set; }
    }
}
