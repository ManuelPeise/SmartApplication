using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Service.Api.Ai
{
    public class AiTrainingController: ApiControllerBase
    {
        private readonly IAiTrainingService _aiTrainingService;

        public AiTrainingController(IAiTrainingService service)
        {
            _aiTrainingService = service;    
        }

        [HttpPost(Name = "TrainSpamDetectionAiModel")]
        public async Task TrainSpamDetectionAiModel()
        {
            await _aiTrainingService.TrainSpamDetectionAiModel();
        }
    }
}
