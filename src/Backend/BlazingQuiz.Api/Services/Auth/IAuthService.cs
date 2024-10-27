using BlazingQuiz.Shared.DTOs.Auth;
using BlazingQuiz.Shared.Exceptions;

namespace BlazingQuiz.Api.Services.Auth;

public interface IAuthService
{
    Task<CustomResult<LoginResponseDto>> Login(LoginDto dto);
}