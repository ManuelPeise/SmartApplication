using Shared.Enums;

namespace Data.Shared.Logging
{
    public class LogMessageEntity: AEntityBase
    {
        public string? Message { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? Module { get; set; }
        public LogMessageTypeEnum MessageType { get; set; } = LogMessageTypeEnum.Info;
        public DateTime TimeStamp { get; set; }
        
    }
}
