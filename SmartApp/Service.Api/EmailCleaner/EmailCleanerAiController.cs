using Data.ContextAccessor.Interfaces;
using Logic.Ai.SpamPrediction;
using Microsoft.AspNetCore.Mvc;

namespace Service.Api.EmailCleaner
{
    public class EmailCleanerAiController : ApiControllerBase
    {
        private readonly IAiRepository _aiRepository;
        public EmailCleanerAiController(IAiRepository aiRepository)
        {
            _aiRepository = aiRepository;
        }

        [HttpGet(Name = "TrainSpamClassificationAI")]
        public async Task TrainSpamClassificationAI()
        {
            var classification = new SpamClassification(_aiRepository);

            await classification.TrainSpamPredictionModel();

        }
    }
}
