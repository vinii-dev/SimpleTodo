using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleTodo.Domain.Entities;
using SimpleTodo.Domain.Interfaces.Services;
using SimpleTodo.Domain.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleTodo.Infrastructure.Services;

/// <summary>
/// Service responsible for generating JWT tokens for authenticated users.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TokenGenerator"/> class.
/// </remarks>
/// <param name="tokenOptions">The options used to configure the token generation, including values for key, issuer, audience, and expiration time.</param>
public class TokenGenerator : ITokenGenerator
{
    private readonly TokenOptions _tokenOptions;
    private readonly TimeProvider _timeProvider;

    public TokenGenerator(IOptions<TokenOptions> tokenOptions, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(tokenOptions);
        var options = tokenOptions.Value;
        if (options == null) throw new InvalidOperationException("Token options cannot be null.");

        _tokenOptions = options;
        _timeProvider = timeProvider;
    }

    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is being generated.</param>
    /// <returns>A JWT token as a string.</returns>
    public string Generate(User user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecretKey));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _tokenOptions.Issuer,
            Audience = _tokenOptions.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = utcNow.AddMinutes(_tokenOptions.ExpiresInMinutes),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
