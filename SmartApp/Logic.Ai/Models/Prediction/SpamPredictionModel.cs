using Microsoft.ML.Data;

namespace Logic.Ai.Models.Prediction
{
    public class SpamPredictionModel
    {
        [ColumnName("PredictedLabel")] 
        public bool Label { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
