using BlazingQuiz.Shared.DTOs.Auth;
using Refit;

namespace BlazingQuiz.Web.Apis;

public interface IAuthApi
{
    [Post("/api/auth/login")]
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto);
}
