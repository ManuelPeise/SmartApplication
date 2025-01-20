namespace Data.Shared.Tools
{
    public class EmailDataEntity : AEntityBase
    {
        public string FromAddress { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string? Body { get; set; }
    }
}
