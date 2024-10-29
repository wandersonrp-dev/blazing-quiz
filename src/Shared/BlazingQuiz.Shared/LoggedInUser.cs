using BlazingQuiz.Shared.Enums.Users;
using System.Security.Claims;
using System.Text.Json;

namespace BlazingQuiz.Shared;
public record LoggedInUser(Guid Identifier, string Name, Role Role, string Token)
{
    public string ToJson() => JsonSerializer.Serialize(this);

    public List<Claim> GetClaims()
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, Identifier.ToString()),
            new Claim(ClaimTypes.Name, Name),
            new Claim(ClaimTypes.Role, Role.ToString()),
            new Claim(nameof(Token), Token),
        };

        return claims; 
    }
}

