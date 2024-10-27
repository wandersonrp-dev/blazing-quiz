using BlazingQuiz.Shared.DTOs.Auth;
using BlazingQuiz.Shared.Exceptions.Messages;
using FluentValidation;

namespace BlazingQuiz.Shared.Validators.AuthServices;
public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage(ResourceErrorMessages.INVALID_EMAIL);
        RuleFor(x => x.Password).NotEmpty().WithMessage(ResourceErrorMessages.PASSWORD_REQUIRED);
    }
}
