﻿using NSubstitute;
using SimpleTodo.Application.Services;
using SimpleTodo.Domain.Contracts.Auth.Login;
using SimpleTodo.Domain.Contracts.Auth.Register;
using SimpleTodo.Domain.Entities;
using SimpleTodo.Domain.Errors;
using SimpleTodo.Domain.Interfaces.Repositories;
using SimpleTodo.Domain.Interfaces.Services;
using TestCommon.Asserts;
using TestCommon.Consts;

namespace SimpleTodo.Application.Tests.Services;

public class AuthServiceTests
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly IPasswordHasher _passwordHasherMock;
    private readonly ITokenGenerator _tokenGeneratorMock;
    private readonly AuthService _authService;

    private const string Password = "Password123";
    private const string HashedPassword = "HashedPassword";

    public AuthServiceTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _passwordHasherMock = Substitute.For<IPasswordHasher>();
        _tokenGeneratorMock = Substitute.For<ITokenGenerator>();

        _authService = new AuthService(
            _userRepositoryMock,
            _passwordHasherMock,
            _tokenGeneratorMock);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnGeneratedToken()
    {
        // Arrange
        var request = new LoginRequest(UserConsts.Username, Password);

        var user = new User(request.Username, HashedPassword);
        _userRepositoryMock.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasherMock.Verify(Password, HashedPassword)
            .Returns(true);

        var generatedToken = "generated token";
        _tokenGeneratorMock.Generate(user)
            .Returns(generatedToken);

        // Act
        var result = await _authService.LoginAsync(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(generatedToken, result.Value.Token);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnInvalidCredentialsError()
    {
        // Arrange
        var request = new LoginRequest(UserConsts.Username, HashedPassword);
        _userRepositoryMock.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var result = await _authService.LoginAsync(request, CancellationToken.None);

        // Act & Assert
        ErrorOrAssert.IsError(result, AuthErrors.InvalidCredentials);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnInvalidCredentialError()
    {
        // Arrange
        var request = new LoginRequest(UserConsts.Username, HashedPassword);
        var user = new User(UserConsts.Username, HashedPassword);

        _userRepositoryMock.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasherMock.Verify(request.Password, user.Password)
            .Returns(false);

        // Act
        var result = await _authService.LoginAsync(request, CancellationToken.None);

        // Assert
        ErrorOrAssert.IsError(result, AuthErrors.InvalidCredentials);
    }

    [Fact]
    public async Task RegisterAsync_ValidRequest_CreateUser()
    {
        // Arrange
        var request = new RegisterRequest(UserConsts.Username, HashedPassword);

        _userRepositoryMock.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        _passwordHasherMock.Hash(request.Password)
            .Returns(HashedPassword);

        // Act
        var result = await _authService.RegisterAsync(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        await _userRepositoryMock.Received().AddUserAsync(
            Arg.Is<User>(u => u.Username == request.Username && u.Password == HashedPassword),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task RegisterAsync_UserExists_ReturnUsernameAlreadyInUseError()
    {
        // Arrange
        var request = new RegisterRequest(UserConsts.Username, HashedPassword);
        var existingUser = new User(UserConsts.Username, HashedPassword);

        _userRepositoryMock.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns(existingUser);

        // Act
        var result = await _authService.RegisterAsync(request, CancellationToken.None);

        // Assert
        ErrorOrAssert.IsError(result, AuthErrors.UsernameAlreadyInUse);
    }
}

