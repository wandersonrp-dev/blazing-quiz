using BlazingQuiz.Shared.DTOs.Auth;
using BlazingQuiz.Shared.Exceptions;
using BlazingQuiz.Shared.Validators.AuthServices;

namespace BlazingQuiz.Api.Services.Auth;

public class AuthServiceValidators
{
    public CustomError LoginValidator(LoginDto dto)
    {
        var validator = new LoginValidator();
        var result = validator.Validate(dto);

        if(!result.IsValid)
        {
            var errorMessages = result.Errors
                .Select(x => x.ErrorMessage)
                .ToList();

            return CustomError.ErrorOnValidation(errorMessages);    
        }

        return CustomError.None;
    }
}
