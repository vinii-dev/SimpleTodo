using Microsoft.AspNetCore.Mvc;
using SimpleTodo.Api.Extensions;
using SimpleTodo.Domain.Contracts.Auth.Login;
using SimpleTodo.Domain.Contracts.Auth.Register;
using SimpleTodo.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SimpleTodo.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Login user",
        Description = "Authenticates a user and returns a JWT token upon successful login."
    )]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);

        return result.Match(
            Ok,
            ErrorOrExtensions.ToProblemDetails);
    }

    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "Register user",
        Description = "Authenticates a user and returns a no content upon successful registration."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return result.Match(
            _ => NoContent(),
            ErrorOrExtensions.ToProblemDetails);
    }
}

