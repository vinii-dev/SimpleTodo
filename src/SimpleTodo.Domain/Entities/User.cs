using SimpleTodo.Domain.Common;

namespace SimpleTodo.Domain.Entities;

/// <summary>
/// Represents a user in the system, including credentials and profile information.
/// </summary>
public class User : Entity
{
    /// <summary>
    /// Gets the username of the user that will be used for authentication.
    /// </summary>
    public string Username { get; } = string.Empty;

    /// <summary>
    /// Gets the hashed password of the user that will be used for authentication.
    /// </summary>
    public string Password { get; } = string.Empty;

    /// <summary>
    /// Gets the collection of to-do items associated with the user.
    /// </summary>
    public ICollection<TodoItem> TodoItems { get; } = [];

    /// <summary>
    /// EF Core requires a parameterless constructor for the entity to be able to create it.
    /// </summary>
    private User() { }

    /// <summary>
    /// Creates a new instance of <see cref="User"/> with the specified credentials and user name.
    /// </summary>
    /// <param name="username">The username for the user's authentication</param>
    /// <param name="hashedPassword">The securely hashed password of the user. Must be pre-hashed.</param>
    /// <exception cref="ArgumentException">Throw if any parameter is null or whitespace.</exception>
    public User(string username, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or whitespace.", nameof(username));

        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentException("Password cannot be null or whitespace.", nameof(hashedPassword));

        Username = username;
        Password = hashedPassword;
    }
}
