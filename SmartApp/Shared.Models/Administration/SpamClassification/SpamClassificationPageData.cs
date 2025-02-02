namespace Shared.Models.Administration.SpamClassification
{
    public class SpamClassificationPageData
    {
        public SpamPredictionStatisticData Statistics { get; set; } = new SpamPredictionStatisticData();
        public List<EmailSpamDomainModel> Domains { get; set; } = new List<EmailSpamDomainModel>();
    }
}
