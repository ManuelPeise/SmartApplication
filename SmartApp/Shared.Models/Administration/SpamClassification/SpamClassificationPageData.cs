namespace Shared.Models.Administration.SpamClassification
{
    public class SpamClassificationPageData
    {
        public SpamPredictionStatisticData Statistics { get; set; } = new SpamPredictionStatisticData();
        public List<EmailDomainModel> Domains { get; set; } = new List<EmailDomainModel>();
    }
}
