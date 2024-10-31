using BlazingQuiz.Shared.DTOs.Auth;
using BlazingQuiz.Shared.Exceptions.Messages;
using FluentValidation;

namespace BlazingQuiz.Shared.Validators.AuthServices;
public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.INVALID_EMAIL)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.INVALID_EMAIL);        

        RuleFor(x => x.Password).NotEmpty().WithMessage(ResourceErrorMessages.PASSWORD_REQUIRED);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<LoginDto>.CreateWithOptions((LoginDto)model, x => x.IncludeProperties(propertyName)));
        
        if (result.IsValid)
        {
            return Array.Empty<string>();
        }    

        return result.Errors.Select(e => e.ErrorMessage);
    };
}
