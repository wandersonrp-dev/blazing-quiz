using BlazingQuiz.Api.Services.Auth;
using BlazingQuiz.Shared.DTOs;
using BlazingQuiz.Shared.DTOs.Auth;
using BlazingQuiz.Shared.Exceptions;

namespace BlazingQuiz.Api.Endpoints;
public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (IAuthService authService, LoginDto dto) =>
        {
            var result = await authService.Login(dto);

            if (result.IsFailure)
            {
                var error = result.Error;

                var response = error.Code switch
                {
                    nameof(ErrorCodes.InvalidCredential) => Results.Unauthorized(),
                    nameof(ErrorCodes.ErrorOnValidation) => Results.BadRequest(new ResponseError(error.Messages!)),
                    _ => Results.BadRequest()
                };

                return response;
            }

            return Results.Ok(result.Value);
        }).Produces<LoginResponseDto>(statusCode: StatusCodes.Status200OK)
        .Produces<LoginResponseDto>(statusCode: StatusCodes.Status401Unauthorized)
        .Produces<LoginResponseDto>(statusCode: StatusCodes.Status400BadRequest);

        return app;
    }
}
