using SimpleTodo.Application.Exceptions.TodoItem;
using SimpleTodo.Application.Exceptions.User;
using SimpleTodo.Domain.Common;
using SimpleTodo.Domain.Contracts.Pagination;
using SimpleTodo.Domain.DTOs.TodoItems;
using SimpleTodo.Domain.Interfaces.Repositories;
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
    /// <returns>The TodoItemDto if found; otherwise, null.</returns>
    /// <exception cref="UserNotFoundException">Thrown if the user is not found.</exception>
    public async Task<TodoItemDto?> GetByIdAsync(
        Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        await CheckIfUserExists(userId, cancellationToken);

        var todoItem = await todoItemRepository.GetByIdAsync(id, cancellationToken);
        if (todoItem == null || todoItem.UserId != userId) return null;

        return todoItem.ToDto();
    }

    /// <summary>
    /// Creates a new TodoItem.
    /// </summary>
    /// <param name="userId">The user id that the to-do item will be associated with.</param>
    /// <param name="itemCreateDto">The DTO containing the details of the TodoItem to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the created TodoItem.</returns>
    /// <exception cref="UserNotFoundException">Thrown if the user is not found.</exception>
    public async Task<Guid> CreateAsync(
        Guid userId, TodoItemCreateDto itemCreateDto, CancellationToken cancellationToken = default)
    {
        await CheckIfUserExists(userId, cancellationToken);

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
    /// <exception cref="UserNotFoundException">Thrown if the user is not found.</exception>
    /// <exception cref="TodoItemNotFoundException">Thrown if the TodoItem is not found or does not belong to the user.</exception>
    public async Task RemoveAsync(
        Guid userId, Guid todoItemId, CancellationToken cancellationToken = default)
    {
        await CheckIfUserExists(userId, cancellationToken);

        var todoItem = await todoItemRepository.GetByIdAsync(todoItemId, cancellationToken);
        if (todoItem == null || todoItem.UserId != userId)
            throw new TodoItemNotFoundException();

        await todoItemRepository.Remove(todoItem, cancellationToken);
    }

    /// <summary>
    /// Updates a TodoItem.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="todoItemId">The ID of the TodoItem to update.</param>
    /// <param name="itemUpdateDto">The DTO with the properties being updated.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="UserNotFoundException">Thrown if the user is not found.</exception>
    /// <exception cref="TodoItemNotFoundException">Thrown if the TodoItem is not found or does not belong to the user.</exception>
    public async Task UpdateAsync(
        Guid userId, Guid todoItemId, TodoItemUpdateDto itemUpdateDto, CancellationToken cancellationToken = default)
    {
        await CheckIfUserExists(userId, cancellationToken);

        var todoItem = await todoItemRepository.GetByIdAsync(todoItemId, cancellationToken);
        if (todoItem == null || todoItem.UserId != userId)
            throw new TodoItemNotFoundException();

        todoItem.Update(todoItem.Title, todoItem.Description);
        await todoItemRepository.UpdateAsync(todoItem, cancellationToken);
    }

    /// <summary>
    /// Partially updates a TodoItem.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="todoItemId">The ID of the TodoItem to update.</param>
    /// <param name="itemPatchDto">The DTO containing the fields to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="UserNotFoundException">Thrown if the user is not found.</exception>
    /// <exception cref="TodoItemNotFoundException">Thrown if the TodoItem is not found or does not belong to the user.</exception>
    public async Task PatchAsync(
        Guid userId, Guid todoItemId, TodoItemPatch itemPatchDto, CancellationToken cancellationToken = default)
    {
        await CheckIfUserExists(userId, cancellationToken);

        var todoItem = await todoItemRepository.GetByIdAsync(todoItemId, cancellationToken);
        if (todoItem == null || todoItem.UserId != userId)
            throw new TodoItemNotFoundException();

        if (itemPatchDto.IsCompleted is bool isCompleted
            && todoItem.IsCompleted != isCompleted)
        {
            todoItem.ToggleCompleted();
        }

        await todoItemRepository.UpdateAsync(todoItem, cancellationToken);
    }

    /// <summary>
    /// Checks if a user exists.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="UserNotFoundException">Thrown if the user is not found.</exception>
    private async Task CheckIfUserExists(Guid userId, CancellationToken cancellationToken)
    {
        if (!await userRepository.ExistsAsync(userId, cancellationToken))
            throw new UserNotFoundException();
    }
}
