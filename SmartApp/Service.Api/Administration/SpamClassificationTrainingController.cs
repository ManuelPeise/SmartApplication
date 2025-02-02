using Data.ContextAccessor.Interfaces;
using Logic.Administration;
using Logic.Ai.Models.Input;
using Logic.Ai.Models.Prediction;
using Logic.Ai.SpamPrediction;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Administration.SpamClassification;

namespace Service.Api.Administration
{
    public class SpamClassificationTrainingController : ApiControllerBase
    {
        private readonly IAiRepository _aiRepository;

        public SpamClassificationTrainingController(IAiRepository aiRepository)
        {
            _aiRepository = aiRepository;
        }

        [HttpGet(Name = "GetSpamClassificationPageData")]
        public async Task<SpamClassificationPageData> GetSpamClassificationPageData()
        {
            using (var classification = new SpamClassificationDataService(_aiRepository))
            {
                return await classification.GetSpamClassificationPageData();
            }
        }

        [HttpPost(Name = "SaveTrainingData")]
        public async Task<bool> SaveTrainingData([FromBody] SaveTrainingDataRequest request)
        {
            using (var classification = new SpamClassificationDataService(_aiRepository))
            {
                return await classification.SaveTrainingData(request);
            }
        }

        [HttpPost(Name = "UpdateTrainingDataCsv")]
        public async Task UpdateTrainingDataCsv()
        {
            using (var classification = new SpamClassificationDataService(_aiRepository))
            {
                 await classification.UpdateTrainingDataCsv();
            }
        }

        [HttpPost(Name = "Train")]
        public async Task Train()
        {
            var classification = new SpamClassification(_aiRepository);

            await classification.TrainSpamPredictionModel();
        }

        [HttpPost(Name = "Predict")]
        public SpamPredictionModel Predict([FromBody] SpamClassificationModel model)
        {
            var classification = new SpamClassification(_aiRepository);

            return  classification.Predict(model);
        }
    }
}
