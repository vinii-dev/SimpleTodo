using Microsoft.IdentityModel.Tokens;
using SimpleTodo.Domain.Options;
using System.Text;

namespace SimpleTodo.Api.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenOptions = configuration.GetRequiredSection("TokenOptions").Get<TokenOptions>()!;

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = tokenOptions.Issuer, 
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecretKey))
                };
            });

        services.AddAuthorization();

        return services;
    }
}
