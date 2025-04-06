using Microsoft.OpenApi.Models;

namespace SimpleTodo.Api.Extensions;

public static class SwaggerDocumentationExtensions
{
    /// <summary>
    /// Adds Swagger services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with Swagger services.</returns>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.EnableAnnotations();
            opt.NonNullableReferenceTypesAsRequired();

            opt.SwaggerDoc("v1", new() { Title = "SimpleTodo API", Version = "v1" });

            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Description = "JWT Authorization header using the Bearer scheme. \n\r Enter your token in the text input below.\n\r Example: \"12345abcdef\""
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

        });
        return services;
    }

    /// <summary>
    /// Configures the application to use Swagger middleware for API documentation.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The updated <see cref="IApplicationBuilder"/> with Swagger middleware configured.</returns>
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.ConfigObject.AdditionalItems.Add("persistAuthorization", true);
        });

        return app;
    }
}
