using Data.ContextAccessor.Interfaces;
using Data.Shared.Logging;
using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Models.Administration.Log;

namespace Logic.Administration
{
    public class LogMessageService : ILogMessageService
    {
        private readonly ILogRepository _logRepository;
        private bool disposedValue;

        public LogMessageService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<List<LogMessageExportModel>> GetLogmessages()
        {
            try
            {
                var messages = await _logRepository.GetAll();

                return (from msg in messages
                        select new LogMessageExportModel
                        {
                            Id = msg.Id,
                            Message = msg.Message,
                            ExceptionMessage = msg.ExceptionMessage,
                            TimeStamp = msg.TimeStamp,
                            MessageType = msg.MessageType,
                        }).ToList();
            }
            catch (Exception exception)
            {
                await _logRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not load log messages",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(LogMessageService)
                });

                return new List<LogMessageExportModel>();
            }
        }

        public async Task DeleteMessages(List<int> messageIds)
        {
            try
            {
                await _logRepository.DeleteMessages(messageIds);

                await _logRepository.SaveChanges();
            }
            catch (Exception exception)
            {
                await _logRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not delete log messages.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(LogMessageService)
                });

            }
        }

        #region dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _logRepository.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
