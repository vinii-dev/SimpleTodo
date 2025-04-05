using SimpleTodo.Domain.Entities;

namespace SimpleTodo.Domain.Interfaces.Services;

/// <summary>
/// Provides methods for generating tokens for user authentication.
/// </summary>
public interface ITokenGenerator
{
    /// <summary>
    /// Generates a token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is to be generated.</param>
    /// <returns>A string representing the generated token.</returns>
    string Generate(User user);
}
