using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Administration.Log;
using Shared.Models.Response;

namespace Service.Api.Administration
{
    [Authorize]
    public class LogController : ApiControllerBase
    {
        private readonly ILogMessageService _logMessageService;

        public LogController(ILogMessageService logMessageService)
        {
            _logMessageService = logMessageService;
        }

        [HttpGet(Name = "GetLogMessages")]
        public async Task<List<LogMessageExportModel>> GetLogMessages()
        {
            return await _logMessageService.GetLogmessages();
        }

        [HttpGet(Name = "DeleteMessages")]
        public async Task<List<LogMessageExportModel>> DeleteMessages([FromBody] List<int> messageIds)
        {
            return await _logMessageService.DeleteMessages(messageIds);

        }
    }
}
