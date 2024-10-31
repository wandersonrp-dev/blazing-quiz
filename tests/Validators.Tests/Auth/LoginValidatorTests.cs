using BlazingQuiz.Shared.Exceptions.Messages;
using BlazingQuiz.Shared.Validators.AuthServices;
using Common.Tests.Utilities.DTOs.Auth;
using FluentAssertions;

namespace Validators.Tests.Auth;
public class LoginValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new LoginValidator();

        var dto = LoginDtoBuilder.Build();

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Theory]    
    [InlineData("johndoe.com")]
    [InlineData("johndoe")]       
    [InlineData(null)]   
    public void Error_Invalid_Email(string email)
    {
        var validator = new LoginValidator();

        var dto = LoginDtoBuilder.Build();        
        dto.Email = email;

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_EMAIL));
    }    

    [Fact]    
    public void Error_Password_Required()
    {
        var validator = new LoginValidator();

        var dto = LoginDtoBuilder.Build();
        dto.Password = string.Empty;        

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_REQUIRED));
    }
}
