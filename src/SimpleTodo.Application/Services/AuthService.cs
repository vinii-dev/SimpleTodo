using ErrorOr;
using SimpleTodo.Domain.Contracts.Auth.Login;
using SimpleTodo.Domain.Contracts.Auth.Register;
using SimpleTodo.Domain.Entities;
using SimpleTodo.Domain.Errors;
using SimpleTodo.Domain.Interfaces.Repositories;
using SimpleTodo.Domain.Interfaces.Services;

namespace SimpleTodo.Application.Services;

/// <summary>
/// Service for handling authentication-related operations such as login and registration.
/// </summary>
public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenGenerator tokenGenerator) : IAuthService
{
    /// <summary>
    /// Logs in a user with the specified credentials.
    /// </summary>
    /// <param name="request">The login request containing the username and password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The LoginResponse if the user was authenticated succesfully; otherwise returns <see cref="AuthErrors.InvalidCredentials"/></returns>
    public async Task<ErrorOr<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        if (user == null)
            return AuthErrors.InvalidCredentials;

        var isPasswordValid = passwordHasher.Verify(request.Password, user.Password);

        if (!isPasswordValid)
            return AuthErrors.InvalidCredentials;

        var token = tokenGenerator.Generate(user);

        return new LoginResponse(token);
    }

    /// <summary>
    /// Registers a new user with the specified credentials.
    /// </summary>
    /// <param name="request">The registration request containing the username and password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A created result status if the user was successfully created; otherwise, returns <see cref="AuthErrors.UsernameAlreadyInUse"/>. </returns>
    public async Task<ErrorOr<Created>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        if (existingUser != null)
            return AuthErrors.UsernameAlreadyInUse;

        var hashedPassword = passwordHasher.Hash(request.Password);
        var user = new User(request.Username, hashedPassword);
        await userRepository.AddUserAsync(user, cancellationToken);

        return Result.Created;
    }
}
