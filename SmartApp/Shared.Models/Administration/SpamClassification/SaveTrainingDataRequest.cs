namespace Shared.Models.Administration.SpamClassification
{
    public class SaveTrainingDataRequest
    {
        public List<SpamClassificationDataSet> Models { get; set; } = new List<SpamClassificationDataSet>();
    }
}
