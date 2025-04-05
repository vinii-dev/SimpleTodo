namespace SimpleTodo.Domain.Options;

/// <summary>
/// Options for configuring JWT token generation.
/// </summary>
public class TokenOptions
{
    public const string PATH = "TokenOptions";

    public required string SecretKey { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public int ExpiresInMinutes { get; init; }
}
