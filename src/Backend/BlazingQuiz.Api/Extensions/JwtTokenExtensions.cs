namespace BlazingQuiz.Api.Extensions;

public static class JwtTokenExtensions
{
    public static string GetJwtSecret(this IConfiguration configuration)
    {
        var secret = configuration.GetRequiredSection("AppSettings:Jwt:Secret").Value ?? string.Empty;

        return secret;
    }

    public static int GetJwtExpireMinutes(this IConfiguration configuration)
    {
        var expireInMinutes = int.Parse(configuration.GetRequiredSection("AppSettings:Jwt:ExpireInMinutes").Value!);

        return expireInMinutes;
    }

    public static string GetJwtIssuer(this IConfiguration configuration)
    {
        var issuer = configuration.GetRequiredSection("AppSettings:Jwt:Issuer").Value ?? string.Empty;

        return issuer;
    }

    public static string GetJwtAudience(this IConfiguration configuration)
    {
        var audience = configuration.GetRequiredSection("AppSettings:Jwt:Audience").Value ?? string.Empty;

        return audience;
    }
}
