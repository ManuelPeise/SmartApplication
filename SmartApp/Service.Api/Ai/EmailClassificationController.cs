using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Ai;

namespace Service.Api.Ai
{
    public class EmailClassificationController : ApiControllerBase
    {
        private IAiTrainingService _aiTrainingService;

        public EmailClassificationController(IAiTrainingService aiTrainingService)
        {
            _aiTrainingService = aiTrainingService;
        }

        // called by scheduler
        [HttpGet(Name = "LoadEmailClassificationTrainingData")]
        public async Task LoadEmailClassificationTrainingData([FromQuery] int maxMails)
        {
            await _aiTrainingService.CollectTrainingData(maxMails);
        }

        [Authorize]
        [HttpGet(Name = "GetAiEmailTrainingData")]
        public async Task<List<AiEmailTrainingData>> GetAiEmailTrainingData()
        {
            return await _aiTrainingService.GetAiTrainingData();
        }

        [Authorize]
        [HttpPost(Name = "UpdateAiEmailTrainingData")]
        public async Task UpdateAiEmailTrainingData([FromBody]List<AiEmailTrainingData> trainingData)
        {
            await _aiTrainingService.UpdateTrainingData(trainingData);
        }
    }
}
