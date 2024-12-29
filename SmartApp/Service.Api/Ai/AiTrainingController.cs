using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Service.Api.Ai
{
    public class AiTrainingController: ApiControllerBase
    {
        private readonly IAiTrainingService _aiTrainingService;
        private readonly IAiTrainingDataCollector _aiTrainingDataCollector;

        public AiTrainingController(IAiTrainingService service, IAiTrainingDataCollector aiTrainingDataCollector)
        {
            _aiTrainingService = service;
            _aiTrainingDataCollector = aiTrainingDataCollector;
        }

        [HttpPost(Name = "TrainSpamDetectionAiModel")]
        public async Task TrainSpamDetectionAiModel()
        {
            await _aiTrainingService.TrainSpamDetectionAiModel();
        }

        [HttpPost(Name ="CollectTrainingData")]
        public async Task CollectTrainingData()
        {
            await _aiTrainingDataCollector.CollectTrainingData();
        }
    }
}
