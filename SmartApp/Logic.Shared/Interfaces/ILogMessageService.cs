using Shared.Models.Administration.Log;

namespace Logic.Shared.Interfaces
{
    public interface ILogMessageService: IDisposable
    {
        Task<List<LogMessageExportModel>> GetLogmessages();
        Task<List<LogMessageExportModel>> DeleteMessages(List<int> messageIds);
    }
}
