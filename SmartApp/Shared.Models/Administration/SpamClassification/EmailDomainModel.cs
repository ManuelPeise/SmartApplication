namespace Shared.Models.Administration.SpamClassification
{
    public class EmailDomainModel
    {
        public int Id { get; set; }
        public string DomainName { get; set; } = string.Empty;
        public List<SpamClassificationDataSet> ClassificationDataSets { get; set; } = new List<SpamClassificationDataSet>();

    }
}
