using ErrorOr;
using SimpleTodo.Domain.Contracts.Auth.Login;
using SimpleTodo.Domain.Contracts.Auth.Register;

namespace SimpleTodo.Domain.Interfaces.Services;

/// <summary>
/// Interface for authentication services.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Asynchronously logs in a user.
    /// </summary>
    /// <param name="request">The login request containing user credentials.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous login operation. The task result contains the login response.</returns>
    Task<ErrorOr<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously registers a new user.
    /// </summary>
    /// <param name="request">The registration request containing user details.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A created result status if the user was successfully created; otherwise, returns <see cref="AuthErrors.UsernameAlreadyInUse"/>. </returns>
    Task<ErrorOr<Created>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}
