using SimpleTodo.Domain.Entities;

namespace SimpleTodo.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}