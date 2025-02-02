namespace Shared.Models.Administration.SpamClassification
{
    public class SpamPredictionStatisticData
    {
        public int TrainingEntityCount { get; set; }
        public string? TrainingFileTimeStamp { get; set; }
        public string? ModelsFileTimeStamp { get; set; }
        public decimal AverageEntrophy { get; set; }
        public List<SpamPredictionMetric> Metrics { get; set; } = new List<SpamPredictionMetric>();
    }
}
