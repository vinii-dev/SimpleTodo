using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SimpleTodo.Domain.Contracts.Auth.Register;

public record RegisterRequest(
    [Required]
    [SwaggerSchema(Description = "The user's username.", Nullable = false)]
    string Username,

    [Required]
    [SwaggerSchema(Description = "The user's password.", Nullable = false)]
    string Password);
