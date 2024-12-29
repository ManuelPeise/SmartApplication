using Shared.Enums.Ai;

namespace Logic.Ai.Models
{
    public class SpamEmailAiMapping
    {
        public string? Label { get; set; }
        public SpamClassificationEnum Classification { get; set; }
    }
}
