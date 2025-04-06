using SimpleTodo.Domain.Options;
using TestCommon.Consts;

namespace TestCommon.Factory;

public static class TokenOptionsFactory
{
    public static TokenOptions CreateTokenOptions(
        string key = TokenOptionsConsts.Key,
        string issuer = TokenOptionsConsts.Issuer,
        string audience = TokenOptionsConsts.Audience,
        int expiresInMinutes = TokenOptionsConsts.ExpiresInMinutes
    ) => new()
    {
        SecretKey = key,
        Issuer = issuer,
        Audience = audience,
        ExpiresInMinutes = expiresInMinutes
    };
}

