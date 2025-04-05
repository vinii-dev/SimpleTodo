namespace SimpleTodo.Domain.DTOs.TodoItems;

public record TodoItemDto(
    Guid Id,
    string Title,
    string Description,
    bool IsCompleted,
    DateTime CreatedAt);
