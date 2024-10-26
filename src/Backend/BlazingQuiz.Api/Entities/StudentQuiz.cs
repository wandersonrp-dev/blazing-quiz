namespace BlazingQuiz.Api.Entities;

public class StudentQuiz : BaseEntity
{
    public long StudentId { get; set; }
    public long QuizId { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime CompletedOn { get; set; }
    public int Score { get; set; }
    public virtual User Student { get; set; } = default!;
    public virtual Quiz Quiz { get; set; } = default!;
}
