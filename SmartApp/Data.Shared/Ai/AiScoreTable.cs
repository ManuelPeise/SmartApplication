namespace Data.Shared.Ai
{
    public class AiScore:AEntityBase
    {
        public string Type { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public double Accuracy { get; set; }
        public double F1Score { get; set; }
        public double Entropy { get; set; }
        public double LogLoss { get; set; }
        public double LogLossReduction { get; set; }
    }
}
