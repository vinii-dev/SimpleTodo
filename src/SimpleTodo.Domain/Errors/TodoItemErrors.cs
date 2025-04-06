using ErrorOr;

namespace SimpleTodo.Domain.Errors;

public static class TodoItemErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: $"TodoItem.{nameof(NotFound)}",
        description: "The specified to-do item was not found. ");
}
