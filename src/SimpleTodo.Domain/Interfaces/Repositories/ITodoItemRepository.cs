using SimpleTodo.Domain.Common;
using SimpleTodo.Domain.Contracts.Pagination;
using SimpleTodo.Domain.Entities;

namespace SimpleTodo.Domain.Interfaces.Repositories;

/// <summary>
/// Interface for the repository that handles CRUD operations for TodoItem entities.
/// </summary>
public interface ITodoItemRepository
{
    /// <summary>
    /// Creates a new TodoItem.
    /// </summary>
    /// <param name="todoItem">The TodoItem to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateAsync(TodoItem todoItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a TodoItem by its ID.
    /// </summary>
    /// <param name="id">The ID of the TodoItem to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the TodoItem if found, or null if not.</returns>
    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a paginated list of todoitems for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose TodoItems to retrieve.</param>
    /// <param name="paginationParameters">The pagination parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a result of a paginated list of TodoItems.</returns>
    Task<PagedList<TodoItem>> GetPagedByUserIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a TodoItem.
    /// </summary>
    /// <param name="todoItem">The TodoItem to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Remove(TodoItem todoItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a TodoItem.
    /// </summary>
    /// <param name="todoItem">The TodoItem to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
}
