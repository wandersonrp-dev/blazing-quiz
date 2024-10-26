using BlazingQuiz.Api.Enums.Users;

namespace BlazingQuiz.Api.Entities;

public class User : BaseEntity
{
    public Guid Identifier { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Password { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.Student;
    public bool IsApproved { get; set; }
}
