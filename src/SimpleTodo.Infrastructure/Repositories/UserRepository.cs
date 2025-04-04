using Microsoft.EntityFrameworkCore;
using SimpleTodo.Domain.Entities;
using SimpleTodo.Domain.Interfaces.Repositories;

namespace SimpleTodo.Infrastructure.Repositories;

public class UserRepository(SimpleTodoDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
        => await context.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == Id, cancellationToken);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        => await context.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
