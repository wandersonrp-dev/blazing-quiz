namespace BlazingQuiz.Api.Entities;

public class Quiz : BaseEntity
{
    public Guid Identifier { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public long CategoryId { get; set; }
    public int TotalQuestions { get; set; }
    public int TimeInMinutes { get; set; }
    public bool IsActive { get; set; }
    public virtual Category Category { get; set; } = default!;
    public ICollection<Question> Questions { get; set; } = [];
}
