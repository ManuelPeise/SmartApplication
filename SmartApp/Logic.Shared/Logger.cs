using Data.ContextAccessor.Interfaces;
using Data.Shared.Logging;
using Shared.Enums;

namespace Logic.Shared
{
    public class Logger<TModel> where TModel : class
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public Logger(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _unitOfWork = applicationUnitOfWork;
        }

        public async Task Info(string message)
        {
            await _unitOfWork.LogMessageRepository.AddAsync(new LogMessageEntity
            {
                Message = message,
                ExceptionMessage = string.Empty,
                TimeStamp = DateTime.UtcNow,
                MessageType = LogMessageTypeEnum.Info,
                Module = nameof(TModel)

            });

            await _unitOfWork.LogMessageRepository.SaveChangesAsync();
        }

        public async Task Error(string message, string? exceptionMessage = null)
        {
            await _unitOfWork.LogMessageRepository.AddAsync(new LogMessageEntity
            {
                Message = message,
                ExceptionMessage = exceptionMessage,
                TimeStamp = DateTime.UtcNow,
                MessageType = LogMessageTypeEnum.Error,
                Module = nameof(TModel)

            });

            await _unitOfWork.LogMessageRepository.SaveChangesAsync();
        }

        public async Task CriticalError(string message, string? exceptionMessage)
        {
            await _unitOfWork.LogMessageRepository.AddAsync(new LogMessageEntity
            {
                Message = message,
                ExceptionMessage = exceptionMessage,
                TimeStamp = DateTime.UtcNow,
                MessageType = LogMessageTypeEnum.CriticalError,
                Module = nameof(TModel)

            });

            await _unitOfWork.LogMessageRepository.SaveChangesAsync();
        }
    }
}
