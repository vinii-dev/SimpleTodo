using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SimpleTodo.Domain.Contracts.Auth.Login;

public record LoginRequest(
    [Required]
    [SwaggerSchema(Description = "The user's username.", Nullable = false)]
    string Username,

    [Required]
    [SwaggerSchema(Description = "The user's password.", Nullable = false)]
    string Password);
