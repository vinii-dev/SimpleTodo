namespace SimpleTodo.Domain.Interfaces.Services;

/// <summary>
/// Interface for password hashing services.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes the specified password.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    public string Hash(string password);

    /// <summary>
    /// Verifies the specified password against the hashed password.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="hashedPassword">The hashed password to compare against.</param>
    /// <returns>True if the password matches the hashed password, otherwise false.</returns>
    public bool Verify(string password, string hashedPassword);
}
