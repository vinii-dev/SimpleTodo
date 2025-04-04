using Microsoft.EntityFrameworkCore;
using SimpleTodo.Domain.Entities;

namespace SimpleTodo.Infrastructure;

public class SimpleTodoDbContext(DbContextOptions<SimpleTodoDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SimpleTodoDbContext).Assembly);
    }
}
