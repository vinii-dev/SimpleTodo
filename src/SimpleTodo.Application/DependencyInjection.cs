using Microsoft.Extensions.DependencyInjection;
using SimpleTodo.Application.Services;
using SimpleTodo.Domain.Interfaces.Services;

namespace SimpleTodo.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Adds the infrastructure layer to the <see cref="IServiceCollection"/>,
    /// in which is added services for the application layer.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the application services.</returns>
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        return services
            .AddServices();
    }

    /// <summary>
    /// Adds the individual services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the application services.</returns>
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
