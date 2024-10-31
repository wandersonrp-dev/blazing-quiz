using BlazingQuiz.Shared.DTOs.Auth;
using Bogus;

namespace Common.Tests.Utilities.DTOs.Auth;
public static class LoginDtoBuilder
{
    public static LoginDto Build()
    {
        return new Faker<LoginDto>()
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password());
    }
}
