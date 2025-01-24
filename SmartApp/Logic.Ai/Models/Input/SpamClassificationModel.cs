using Microsoft.ML.Data;

namespace Logic.Ai.Models.Input
{
    public class SpamClassificationModel
    {
        [LoadColumn(0)] public bool Label { get; set; }
        [LoadColumn(1)] public string EmailAddress { get; set; }
        [LoadColumn(2)] public string Subject { get; set; }

    }
}
