using Microsoft.ML.Data;

namespace Logic.Ai.Models
{
    public class EmailAiInputModel
    {
        [LoadColumn(0)]
        [ColumnName(@"Label")]
        public string? Label { get; set; }
        [LoadColumn(1)]
        [ColumnName(@"From")]
        public string? From { get; set; }
        [LoadColumn(2)]
        [ColumnName(@"Subject")]
        public string? Subject { get; set; }

    }
}
