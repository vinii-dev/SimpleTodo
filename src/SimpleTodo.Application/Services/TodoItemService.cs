using ErrorOr;
using SimpleTodo.Domain.Common;
using SimpleTodo.Domain.Contracts.Pagination;
using SimpleTodo.Domain.DTOs.TodoItems;
using SimpleTodo.Domain.Errors;
using SimpleTodo.Domain.Interfaces.Repositories;
using SimpleTodo.Domain.Interfaces.Services;
using SimpleTodo.Domain.Mappings;

namespace SimpleTodo.Application.Services;

/// <summary>
/// Service for managing TodoItems.
/// </summary>
public class TodoItemService(ITodoItemRepository todoItemRepository, IUserRepository userRepository) : ITodoItemService
{
    /// <summary>
    /// Retrieves a paginated list of TodoItems for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="paginationRequest">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of TodoItemDto.</returns>
    public async Task<PagedList<TodoItemDto>> GetPagedAsync(
        Guid userId, PaginationParameters paginationRequest, CancellationToken cancellationToken = default)
    {
        if (!await userRepository.ExistsAsync(userId, cancellationToken))
            return PagedList<TodoItemDto>.Empty;

        var pagedTodoItems = await todoItemRepository
            .GetPagedByUserIdAsync(userId, paginationRequest, cancellationToken);

        var pagedTodoItemsDto = pagedTodoItems.Map(TodoItemMappings.ToDto);

        return pagedTodoItemsDto;
    }

    /// <summary>
    /// Retrieves a TodoItem by its ID for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="id">The ID of the TodoItem.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TodoItemDto if the user and item exists; otherwise, <see cref="UserErrors.NotFound"/> or <see cref="TodoItemErrors.NotFound"/>.</returns>
    public async Task<ErrorOr<TodoItemDto?>> GetByIdAsync(
        Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        if (!await userRepository.ExistsAsync(userId, cancellationToken))
            return UserErrors.NotFound;

        var todoItem = await todoItemRepository.GetByIdAsync(id, cancellationToken);
        if (todoItem == null || todoItem.UserId != userId) return TodoItemErrors.NotFound;

        return todoItem.ToDto();
    }

    /// <summary>
    /// Creates a new TodoItem.
    /// </summary>
    /// <param name="userId">The user id that the to-do item will be associated with.</param>
    /// <param name="itemCreateDto">The DTO containing the details of the TodoItem to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the created TodoItem if the user exists; otherwise, <see cref="UserErrors.NotFound"/></returns>
    public async Task<ErrorOr<Guid>> CreateAsync(
        Guid userId, TodoItemCreateDto itemCreateDto, CancellationToken cancellationToken = default)
    {
        if (!await userRepository.ExistsAsync(userId, cancellationToken))
            return UserErrors.NotFound;

        var todoItem = itemCreateDto.ToEntity(userId);
        await todoItemRepository.CreateAsync(todoItem, cancellationToken);

        return todoItem.Id;
    }

    /// <summary>
    /// Removes a TodoItem.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="todoItemId">The ID of the TodoItem to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a Deleted result if the the user and the to-do item was found;
    /// otherwise, <see cref="UserErrors.NotFound" /> or <see cref="TodoItemErrors.NotFound"/> ></returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(
        Guid userId, Guid todoItemId, CancellationToken cancellationToken = default)
    {
        if (!await userRepository.ExistsAsync(userId, cancellationToken))
            return UserErrors.NotFound;

        var todoItem = await todoItemRepository.GetByIdAsync(todoItemId, cancellationToken);
        if (todoItem == null || todoItem.UserId != userId)
            return TodoItemErrors.NotFound;

        await todoItemRepository.RemoveAsync(todoItem, cancellationToken);

        return Result.Deleted;
    }

    /// <summary>
    /// Updates a TodoItem.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="todoItemId">The ID of the TodoItem to update.</param>
    /// <param name="itemUpdateDto">The DTO with the properties being updated.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a Updated result if the the user and the to-do item was found;
    /// otherwise, <see cref="UserErrors.NotFound" /> or <see cref="TodoItemErrors.NotFound"/> ></returns>
    public async Task<ErrorOr<Updated>> UpdateAsync(
        Guid userId, Guid todoItemId, TodoItemUpdateDto itemUpdateDto, CancellationToken cancellationToken = default)
    {
        if (!await userRepository.ExistsAsync(userId, cancellationToken))
            return UserErrors.NotFound;

        var todoItem = await todoItemRepository.GetByIdAsync(todoItemId, cancellationToken);
        if (todoItem == null || todoItem.UserId != userId)
            return TodoItemErrors.NotFound;

        todoItem.Update(itemUpdateDto.Title, itemUpdateDto.Description);
        await todoItemRepository.UpdateAsync(todoItem, cancellationToken);

        return Result.Updated;
    }

    /// <summary>
    /// Partially updates a TodoItem.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="todoItemId">The ID of the TodoItem to update.</param>
    /// <param name="itemPatchDto">The DTO containing the fields to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a Updated result if the the user and the to-do item was found;
    /// otherwise, <see cref="UserErrors.NotFound" /> or <see cref="TodoItemErrors.NotFound"/> ></returns>
    public async Task<ErrorOr<Updated>> PatchAsync(
        Guid userId, Guid todoItemId, TodoItemPatch itemPatchDto, CancellationToken cancellationToken = default)
    {
        if (!await userRepository.ExistsAsync(userId, cancellationToken))
            return UserErrors.NotFound;

        var todoItem = await todoItemRepository.GetByIdAsync(todoItemId, cancellationToken);
        if (todoItem == null || todoItem.UserId != userId)
            return TodoItemErrors.NotFound;

        if (itemPatchDto.IsCompleted is bool isCompleted
            && todoItem.IsCompleted != isCompleted)
        {
            todoItem.ToggleCompleted();
        }

        await todoItemRepository.UpdateAsync(todoItem, cancellationToken);

        return Result.Updated;
    }
}
