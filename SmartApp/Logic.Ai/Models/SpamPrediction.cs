using Microsoft.ML.Data;

namespace Logic.Ai.Models
{
    public class SpamPrediction
    {
        [ColumnName("PredictedLabel")] 
        public string? PredictedLabel { get; set; }
        public float[] Scores { get; set; }
    }

}
