using SimpleTodo.Domain.DTOs.TodoItems;
using SimpleTodo.Domain.Entities;

namespace SimpleTodo.Domain.Mappings;

public static class TodoItemMappings
{
    public static TodoItemDto ToDto(this TodoItem todoItem)
    {
        return new TodoItemDto(
            todoItem.Id,
            todoItem.Title,
            todoItem.Description,
            todoItem.CreatedAt);
    }

    public static TodoItem ToEntity(this TodoItemCreateDto itemCreateDto, Guid userId)
    {
        return new TodoItem(
            itemCreateDto.Title,
            itemCreateDto.Description,
            userId);
    }
}
