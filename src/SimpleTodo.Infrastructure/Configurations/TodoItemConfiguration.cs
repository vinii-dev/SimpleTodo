using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleTodo.Domain.Entities;

namespace SimpleTodo.Infrastructure.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.Property(ti => ti.Title)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(ti => ti.Description)
            .IsRequired();

        builder.Property(ti => ti.IsCompleted)
            .IsRequired();

        builder.HasOne(ti => ti.User)
            .WithMany(u => u.TodoItems)
            .IsRequired();
    }
}
