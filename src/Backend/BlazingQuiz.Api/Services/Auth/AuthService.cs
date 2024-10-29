using BlazingQuiz.Api.Entities;
using BlazingQuiz.Api.Extensions;
using BlazingQuiz.Api.Repositories;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs.Auth;
using BlazingQuiz.Shared.Enums.Users;
using BlazingQuiz.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazingQuiz.Api.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly string _secret;
    private readonly int _expireInMinutes;
    private readonly string _issuer;
    private readonly string _audience;

    public AuthService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _secret = configuration.GetJwtSecret();
        _expireInMinutes = configuration.GetJwtExpireMinutes();
        _issuer = configuration.GetJwtIssuer();
        _audience = configuration.GetJwtAudience();
    }

    public async Task<CustomResult<LoginResponseDto>> Login(LoginDto dto)
    {
        var validate = new AuthServiceValidators().LoginValidator(dto);

        if(validate.Code == nameof(ErrorCodes.ErrorOnValidation))
        {
            return CustomResult<LoginResponseDto>.Failure(validate);
        }

        var user = await _userRepository.ExistsByEmail(dto.Email);

        if (user is null)
        {
            return CustomResult<LoginResponseDto>.Failure(CustomError.InvalidCredential());
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return CustomResult<LoginResponseDto>.Failure(CustomError.InvalidCredential());
        }

        var token = GenerateJwtToken(user);

        var loggedInUser = new LoggedInUser(
            Identifier: user.Identifier,
            Name: user.Name,
            Role: (Role)user.Role,
            Token: token);

        return CustomResult<LoginResponseDto>.Success(new LoginResponseDto(User: loggedInUser));
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Identifier.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims, 
            expires: DateTime.UtcNow.AddMinutes(_expireInMinutes), 
            signingCredentials: signingCredential);

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.WriteToken(jwtSecurityToken);

        return token;
    }
}
