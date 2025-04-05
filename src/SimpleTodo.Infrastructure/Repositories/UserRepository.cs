using Microsoft.EntityFrameworkCore;
using SimpleTodo.Domain.Common;
using SimpleTodo.Domain.Entities;
using SimpleTodo.Domain.Interfaces.Repositories;

namespace SimpleTodo.Infrastructure.Repositories;

/// <summary>
/// Repository for managing <see cref="User"/> entities in the database.
/// </summary>
public class UserRepository(SimpleTodoDbContext dbContext) : IUserRepository
{
    /// <summary>
    /// Retrieves a <see cref="User"/> by their <see cref="Entity.Id"/>  (unique identifier).
    /// </summary>
    /// <param name="Id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The <see cref="User"/> with the specified identifier, or null if no user is found.</returns>
    public async Task<User?> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
        => await dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == Id, cancellationToken);

    /// <summary>
    /// Retrieves a <see cref="User"/> by their <see cref="User.Username"/>.
    /// </summary>
    /// <param name="username">The username of the <see cref="User"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The <see cref="User"/> with the specified username, or null if no user is found.</returns>
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        => await dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    /// <summary>
    /// Adds a new <see cref="User"/> to the database.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Checks if a <see cref="User"/> exists in the database by their <see cref="Entity.Id"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if the user exists, otherwise false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == id, cancellationToken);
}
