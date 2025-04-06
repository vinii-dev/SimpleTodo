using SimpleTodo.Domain.Common;

namespace SimpleTodo.Domain.Entities;

/// <summary>
/// Represents a single to-do item in the system.
/// </summary>
public class TodoItem : Entity
{
    /// <summary>
    /// Gets the title of the to-do item.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the description of the to-do item.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the to-do item is completed.
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// The ID of the user who owns this task.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Navigation property to the user who owns this task.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Required by EF Core and serializers. Initializes a new instance of the <see cref="TodoItem"/> class.
    /// </summary>
    private TodoItem() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoItem"/> class with the specified title and description.
    /// </summary>
    /// <param name="title">The title of the to-do item.</param>
    /// <param name="description">The description of the to-do item.</param>
    /// <exception cref="ArgumentException">Thrown when title or description is null or whitespace.</exception>
    public TodoItem(string title, string description, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or whitespace.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or whitespace.", nameof(description));

        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty. ", nameof(userId));

        Title = title;
        Description = description;
        UserId = userId;

        IsCompleted = false;
    }

    /// <summary>
    /// Updates the title and description of the to-do item.
    /// </summary>
    /// <param name="title">The new title.</param>
    /// <param name="description">The new description.</param>
    /// <exception cref="ArgumentException">Thrown when title or description is null or whitespace.</exception>
    public void Update(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or whitespace.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or whitespace.", nameof(description));

        Title = title;
        Description = description;
    }

    /// <summary>
    /// Toggles the completion status of the to-do item.
    /// </summary>
    public void ToggleCompleted()
    {
        IsCompleted = !IsCompleted;
    }
}

