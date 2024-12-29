using Shared.Enums.Ai;

namespace Shared.Models.Ai
{
    public class AiSpamEmailPredictionResult
    {
        public string Label { get; set; }
        public float[] Scores { get; set; }
        public SpamClassificationEnum Classification { get; set; }
    }
}
