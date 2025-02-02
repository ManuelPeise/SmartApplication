using Data.ContextAccessor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models.Administration.Log;

namespace Service.Api.Administration
{
    public class LogController : ApiControllerBase
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public LogController(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.Admin, AllowAdmin = true)]
        [HttpGet(Name = "GetLogMessages")]
        public async Task<List<LogMessageExportModel>> GetLogMessages()
        {
            var entities = await _applicationUnitOfWork.LogMessageRepository.GetAllAsync();

            return entities.Select(x => new LogMessageExportModel
            {
                Id = x.Id,
                Message = x.Message,
                ExceptionMessage = x.ExceptionMessage,
                MessageType = x.MessageType,
                TimeStamp = x.TimeStamp,
            }).ToList();
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.Admin, AllowAdmin = true)]
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
