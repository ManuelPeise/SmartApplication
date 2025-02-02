namespace Logic.Ai.Models.Prediction
{
    public class SpamClassificationExportModel
    {
        public string Label { get; set; }
        public List<float> Scores { get; set; }
    }
}
