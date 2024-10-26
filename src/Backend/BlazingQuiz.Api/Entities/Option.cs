namespace BlazingQuiz.Api.Entities;

public class Option : BaseEntity
{
    public long QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public virtual Question Question { get; set; } = default!;
}
