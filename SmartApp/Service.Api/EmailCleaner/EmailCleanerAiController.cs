using Data.ContextAccessor.Interfaces;
using Logic.Ai.Models.Input;
using Logic.Ai.Models.Prediction;
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

        //[HttpPost(Name = "Predict")]
        //public async Task<SpamClassificationExportModel?> Predict([FromBody] SpamClassificationModel model)
        //{
        //    using (var classification = new SpamClassification(_aiRepository))
        //    {
        //        return await classification.Predict(model);
        //    }
        //}
    }
}
