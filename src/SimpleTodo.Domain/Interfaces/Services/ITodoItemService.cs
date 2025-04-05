using SimpleTodo.Domain.Common;
using SimpleTodo.Domain.Contracts.Pagination;
using SimpleTodo.Domain.DTOs.TodoItems;

namespace SimpleTodo.Application.Services;

/// <summary>
/// Interface for TodoItem service operations.
/// </summary>
public interface ITodoItemService
{
    /// <summary>
    /// Creates a new TodoItem.
    /// </summary>
    /// <param name="userId">The user id that the to-do item will be associated with.</param>
    /// <param name="itemCreateDto">The DTO containing the details of the item to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the created TodoItem.</returns>
    Task<Guid> CreateAsync(Guid userId, TodoItemCreateDto itemCreateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a TodoItem by its ID.
    /// </summary>
    /// <param name="userId">The ID of the user who owns the item.</param>
    /// <param name="id">The ID of the item to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The DTO of the retrieved TodoItem, or null if not found.</returns>
    Task<TodoItemDto?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paginated list of TodoItems.
    /// </summary>
    /// <param name="userId">The ID of the user who owns the items.</param>
    /// <param name="paginationRequest">The pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of TodoItem DTOs.</returns>
    Task<PagedList<TodoItemDto>> GetPagedAsync(Guid userId, PaginationParameters paginationRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Partially updates a TodoItem.
    /// </summary>
    /// <param name="userId">The ID of the user who owns the item.</param>
    /// <param name="id">The ID of the item to update.</param>
    /// <param name="todoItemPatchDto">The DTO containing the properties to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PatchAsync(Guid userId, Guid id, TodoItemPatch todoItemPatchDto, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a TodoItem.
    /// </summary>
    /// <param name="userId">The ID of the user who owns the item.</param>
    /// <param name="todoItemId">The ID of the item to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RemoveAsync(Guid userId, Guid todoItemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a TodoItem.
    /// </summary>
    /// <param name="userId">The ID of the user who owns the item.</param>
    /// <param name="todoItemId">The ID of the item to update.</param>
    /// <param name="itemUpdateDto">The DTO with the properties being updated.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task UpdateAsync(Guid userId, Guid todoItemId, TodoItemUpdateDto itemUpdateDto, CancellationToken cancellationToken = default);
}