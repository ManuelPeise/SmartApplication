namespace Shared.Models.Administration.SpamClassification
{
    public class SpamPredictionMetric
    {
        public string TimeStamp { get; set; } = string.Empty;
        public double Accuracy { get; set; }
        public double F1Score { get; set; }
        public double Entropy { get; set; }
        public double LogLoss { get; set; }
        public double LogLossReduction { get; set; }
    }
}
