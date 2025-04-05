using SimpleTodo.Domain.Entities;
using TestCommon.Consts;

namespace TestCommon.Factory;

public static class TodoItemFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="TodoItem"/> with the specified parameters.
    /// </summary>
    /// <param name="title">The title of the to-do item.</param>
    /// <param name="description">The description of the to-do item.</param>
    /// <param name="userId">The user associated with the to-do item.</param>
    /// <param name="isCompleted">Indicates whether the to-do item is completed.</param>
    /// <returns>A new instance of <see cref="TodoItem"/>.</returns>
    /// <remarks>As the <see cref="TodoItem"/> doesn't accept a nullable <see cref="Guid"/> as a parameter, the factory accepts it to generate a new <see cref="Guid"/> at build time if not provided.</remarks>
    public static TodoItem CreateTodoItem(
        string title = TodoItemConsts.Title,
        string description = TodoItemConsts.Description,
        Guid? userId = null,
        bool isCompleted = false)
    {
        userId ??= Guid.NewGuid();

        var todoItem = new TodoItem(
            title: title,
            description: description,
            userId: userId.Value
        );

        if (isCompleted) todoItem.ToggleCompleted();

        return todoItem;
    }
}
