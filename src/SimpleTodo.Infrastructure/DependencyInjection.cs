using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleTodo.Domain.Interfaces.Repositories;
using SimpleTodo.Infrastructure.Interceptors;
using SimpleTodo.Infrastructure.Repositories;

namespace SimpleTodo.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Adds the infrastructure layer to the service collection, including database context, custom interceptors, and repositories.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration containing the connection string.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with infrastructure services.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the connection string is missing from configuration.</exception>
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .SetupDbContext(configuration)
            .RegisterRepositories();
    }

    /// <summary>
    /// Configures the application's database context to use SQL Server, 
    /// and registers custom interceptors implementing (e.g., <see cref="ISaveChangesInterceptor"/>) (e.g., <see cref="AuditableEntityInterceptor"/>).
    /// The interceptor may rely on other services (e.g., <see cref="TimeProvider"/>) that are resolved via dependency injection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Application configuration to retrieve the connection string.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the configured database context and interceptors.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the connection string is null or whitespace.</exception>
    private static IServiceCollection SetupDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("The connection string 'DefaultConnection' is missing or empty. Cannot proceed without a valid connection string.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<SimpleTodoDbContext>((serviceProvider, options) =>
        {
            var interceptors = serviceProvider.GetServices<ISaveChangesInterceptor>();
            options.AddInterceptors(interceptors);

            options.UseSqlServer(connectionString);
        });

        return services;
    }

    /// <summary>
    /// Registers repositories in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with registered repositories.</returns>
    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}

