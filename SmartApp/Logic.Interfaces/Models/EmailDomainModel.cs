namespace Logic.Interfaces.Models
{
    public class EmailDomainModel
    {
        public string EmailId { get; set; } = string.Empty;
        public string SourceAddress { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string? PredictionResult { get; set; }
        public bool IsSpam { get; set; }
        public bool IsNew { get; set; }

    }
}
