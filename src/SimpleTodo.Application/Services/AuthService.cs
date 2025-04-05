﻿using SimpleTodo.Application.Exceptions.Auth;
using SimpleTodo.Domain.Contracts.Auth.Login;
using SimpleTodo.Domain.Contracts.Auth.Register;
using SimpleTodo.Domain.Entities;
using SimpleTodo.Domain.Interfaces.Repositories;
using SimpleTodo.Domain.Interfaces.Services;
using System.Security.Authentication;

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
    /// <returns>A task that represents the asynchronous operation. The task result contains the login response with the generated token.</returns>
    /// <exception cref="InvalidCredentialException">Thrown when the user is not found or the password is invalid.</exception>
    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        if (user == null)
            throw new InvalidCredentialException();

        var isPasswordValid = passwordHasher.Verify(request.Password, user.Password);

        if (!isPasswordValid)
            throw new InvalidCredentialException();

        var token = tokenGenerator.Generate(user);

        return new LoginResponse(token);
    }

    /// <summary>
    /// Registers a new user with the specified credentials.
    /// </summary>
    /// <param name="request">The registration request containing the username and password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="UsernameAlreadyInUseException">Thrown when a user with the specified username already exists.</exception>
    public async System.Threading.Tasks.Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        if (existingUser != null)
            throw new UsernameAlreadyInUseException();

        var hashedPassword = passwordHasher.Hash(request.Password);
        var user = new User(request.Username, hashedPassword);
        await userRepository.AddUserAsync(user, cancellationToken);
    }
}
