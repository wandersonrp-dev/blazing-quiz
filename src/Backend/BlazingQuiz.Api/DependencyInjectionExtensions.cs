using BlazingQuiz.Api.Data.Repositories;
using BlazingQuiz.Api.Repositories;
using BlazingQuiz.Api.Services.Auth;

namespace BlazingQuiz.Api;
public static class DependencyInjectionExtensions
{
    public static void AddDi(this IServiceCollection services)
    {
        AddRepositories(services);
        AddServices(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }
}
