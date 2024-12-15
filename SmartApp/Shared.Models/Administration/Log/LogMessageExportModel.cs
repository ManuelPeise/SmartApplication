using Shared.Enums;

namespace Shared.Models.Administration.Log
{
    public class LogMessageExportModel
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? ExceptionMessage { get; set; }
        public LogMessageTypeEnum MessageType { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
