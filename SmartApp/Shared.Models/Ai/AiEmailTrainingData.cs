using Shared.Enums.Ai;

namespace Shared.Models.Ai
{
    public class AiEmailTrainingData
    {
        public int Id { get; set; }
        public string? From { get; set; }
        public string? Domain { get; set; }
        public string Subject { get; set; } = string.Empty;
        public SpamClassificationEnum Classification { get; set; }

    }
}
