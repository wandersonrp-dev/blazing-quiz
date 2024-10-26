namespace BlazingQuiz.Api.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid Identitfier { get; set; } = Guid.NewGuid();
}
