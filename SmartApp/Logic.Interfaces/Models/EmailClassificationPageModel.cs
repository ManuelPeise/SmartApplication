

namespace Logic.Interfaces.Models
{
    public class EmailClassificationPageModel
    {
        public int AccountId { get; set; }
        public bool SpamPredictionEnabled { get; set; }
        public bool FolderPredictionEnabled { get; set; }
        public List<EmailFolderModel> Folders { get; set; } = new List<EmailFolderModel>();
        public List<EmailClassificationModel> ClassificationModels { get; set; } = new List<EmailClassificationModel>();
    }


}
