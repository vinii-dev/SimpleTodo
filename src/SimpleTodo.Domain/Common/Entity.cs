namespace SimpleTodo.Domain.Common;

/// <summary>
/// Base class for all entities in the domain.
/// It provides a unique identifier and timestamps for creation and updates.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
