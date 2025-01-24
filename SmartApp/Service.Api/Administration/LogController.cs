using Data.ContextAccessor.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Administration.Log;

namespace Service.Api.Administration
{
    [Authorize]
    public class LogController : ApiControllerBase
    {
        private readonly IAdministrationRepository _administrationRepository;

        public LogController(IAdministrationRepository administrationRepository)
        {
            _administrationRepository = administrationRepository;
        }

        [HttpGet(Name = "GetLogMessages")]
        public async Task<List<LogMessageExportModel>> GetLogMessages()
        {
            var entities = await _administrationRepository.LogMessageRepository.GetAllAsync();

            return entities.Select(x => new LogMessageExportModel
            {
                Id = x.Id,
                Message = x.Message,
                ExceptionMessage = x.ExceptionMessage,
                MessageType = x.MessageType,
                TimeStamp = x.TimeStamp,
            }).ToList();
        }

        [HttpPost(Name = "DeleteMessages")]
        public async Task DeleteMessages([FromBody] List<int> messageIds)
        {
            //foreach (var messageId in messageIds) {
            //{
            //    await _administrationRepository.LogMessageRepository.Delete(messageId);
            //})
            //await _administrationRepository.LogMessageRepository.Delete.DeleteMessages(messageIds);

        }
    }
}
