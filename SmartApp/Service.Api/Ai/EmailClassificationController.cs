using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Ai;

namespace Service.Api.Ai
{
    [Authorize]
    public class EmailClassificationController : ApiControllerBase
    {
        private IAiPredictionService _predictionService;

        public EmailClassificationController(IAiPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

       
        [HttpPost(Name = "PredictSpamMail")]
        public async Task<AiSpamEmailPredictionResult> PredictSpamMail([FromBody] SpamMailInputModel inputModel)
        {
            return await _predictionService.PredictSpamMail(inputModel.From, inputModel.Subject);
        }
    }
}
