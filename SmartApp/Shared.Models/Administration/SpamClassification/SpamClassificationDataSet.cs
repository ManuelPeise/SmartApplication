using Shared.Enums;

namespace Shared.Models.Administration.SpamClassification
{
    public class SpamClassificationDataSet
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public SpamClassificationEnum Classification { get; set; }
    }
}
