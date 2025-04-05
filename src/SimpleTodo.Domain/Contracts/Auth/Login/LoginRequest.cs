using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SimpleTodo.Domain.Contracts.Auth.Login;

public record LoginRequest(
    [SwaggerSchema("The user's username.", Nullable = false), Required]
    string Username,

    [SwaggerSchema("The user's password.", Nullable = false), Required]
    string Password);
