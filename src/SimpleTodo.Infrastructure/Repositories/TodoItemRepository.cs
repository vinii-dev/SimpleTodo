using Microsoft.EntityFrameworkCore;
using SimpleTodo.Domain.Common;
using SimpleTodo.Domain.Contracts.Pagination;
using SimpleTodo.Domain.Entities;
using SimpleTodo.Domain.Interfaces.Repositories;
using SimpleTodo.Infrastructure.Helpers;

namespace SimpleTodo.Infrastructure.Repositories;

/// <summary>
/// Repository that manages operations for the <see cref="TodoItem"/> entity.
/// </summary>
public class TodoItemRepository(SimpleTodoDbContext dbContext) : ITodoItemRepository
{
    private readonly DbSet<TodoItem> todoItems = dbContext.TodoItems;

    /// <summary>
    /// Gets a paged list of <see cref="TodoItem"/> for a specific <see cref="User"/>.
    /// </summary>
    /// <param name="userId">The ID of the <see cref="User"/>.</param>
    /// <param name="paginationRequest">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>PagedList containing the user's <see cref="TodoItem"/> items.</returns>
    public async Task<PagedList<TodoItem>> GetPagedByUserIdAsync(
        Guid userId, PaginationParameters paginationRequest, CancellationToken cancellationToken = default)
    {
        var query = todoItems
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt);

        var paginated = await PaginationHelper
            .CreateAsync(query, paginationRequest, cancellationToken);

        return paginated;
    }

    /// <summary>
    /// Updates an existing <see cref="TodoItem"/> in the database.
    /// </summary>
    /// <param name="todoItem">The <see cref="TodoItem"/> instance to be updated.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    public async Task UpdateAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        todoItems.Update(todoItem);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a new <see cref="TodoItem"/> in the database.
    /// </summary>
    /// <param name="todoItem">The <see cref="TodoItem"/> instance to be created.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    public async Task CreateAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        await todoItems.AddAsync(todoItem, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Removes a <see cref="TodoItem"/> from the database.
    /// </summary>
    /// <param name="todoItem">The <see cref="TodoItem"/> instance to be removed.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    public async Task Remove(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        todoItems.Remove(todoItem);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a <see cref="TodoItem"/> by its ID.
    /// </summary>
    /// <param name="id">The ID of the <see cref="TodoItem"/> to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the <see cref="TodoItem"/> if found, or null if not.</returns>
    public Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => todoItems.SingleOrDefaultAsync(ti => ti.Id == id, cancellationToken);
}
