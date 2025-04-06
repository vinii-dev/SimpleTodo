using ErrorOr;
using SimpleTodo.Domain.Common;
using SimpleTodo.Domain.Contracts.Pagination;
using SimpleTodo.Domain.DTOs.TodoItems;

namespace SimpleTodo.Domain.Interfaces.Services;

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
    /// <returns>The ID of the created TodoItem if the user exists; otherwise, <see cref="UserErrors.NotFound"/></returns>
    Task<ErrorOr<Guid>> CreateAsync(Guid userId, TodoItemCreateDto itemCreateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a TodoItem by its ID.
    /// </summary>
    /// <param name="userId">The ID of the user who owns the item.</param>
    /// <param name="id">The ID of the item to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TodoItemDto if the user and item exists; otherwise, <see cref="UserErrors.NotFound"/> or null.</returns>
    Task<ErrorOr<TodoItemDto?>> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);

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
    /// <returns>Returns a Updated result if the the user and the to-do item was found;
    /// otherwise, <see cref="UserErrors.NotFound" /> or <see cref="TodoItemErrors.NotFound"/> ></returns>
    Task<ErrorOr<Updated>> PatchAsync(Guid userId, Guid id, TodoItemPatch todoItemPatchDto, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a TodoItem.
    /// </summary>
    /// <param name="userId">The ID of the user who owns the item.</param>
    /// <param name="todoItemId">The ID of the item to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a Deleted result if the the user and the to-do item was found;
    /// otherwise, <see cref="UserErrors.NotFound" /> or <see cref="TodoItemErrors.NotFound"/> ></returns>
    Task<ErrorOr<Deleted>> RemoveAsync(Guid userId, Guid todoItemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a TodoItem.
    /// </summary>
    /// <param name="userId">The ID of the user who owns the item.</param>
    /// <param name="todoItemId">The ID of the item to update.</param>
    /// <param name="itemUpdateDto">The DTO with the properties being updated.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a Updated result if the the user and the to-do item was found;
    /// otherwise, <see cref="UserErrors.NotFound" /> or <see cref="TodoItemErrors.NotFound"/> ></returns>
    Task<ErrorOr<Updated>> UpdateAsync(Guid userId, Guid todoItemId, TodoItemUpdateDto itemUpdateDto, CancellationToken cancellationToken = default);
}