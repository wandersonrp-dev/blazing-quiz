using System.Text.Json.Serialization;

namespace BlazingQuiz.Shared.Enums.Users;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    Admin, 
    student
}
