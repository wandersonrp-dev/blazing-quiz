namespace BlazingQuiz.Api.Entities;

public class Question : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public long QuizId { get; set; }
    public virtual Quiz Quiz { get; set; } = default!;
    public ICollection<Option> Options { get; set; } = [];
}
