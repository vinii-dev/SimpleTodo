using NSubstitute;
using SimpleTodo.Application.Services;
using SimpleTodo.Domain.Contracts.Auth.Login;
using SimpleTodo.Domain.Contracts.Auth.Register;
using SimpleTodo.Domain.Entities;
using SimpleTodo.Domain.Interfaces.Repositories;
using SimpleTodo.Domain.Interfaces.Services;
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
    public async System.Threading.Tasks.Task LoginAsync_ValidCredentials_ReturnGeneratedToken()
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
        Assert.Equal(generatedToken, result.Token);
    }

    [Fact]
    public async System.Threading.Tasks.Task LoginAsync_UserNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new LoginRequest(UserConsts.Username, HashedPassword);
        _userRepositoryMock.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        Func<System.Threading.Tasks.Task> loginAsync = async () => await _authService.LoginAsync(request, CancellationToken.None);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(loginAsync);
    }

    [Fact]
    public async System.Threading.Tasks.Task LoginAsync_InvalidPassword_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new LoginRequest(UserConsts.Username, HashedPassword);
        var user = new User(UserConsts.Username, HashedPassword);

        _userRepositoryMock.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasherMock.Verify(request.Password, user.Password)
            .Returns(false);

        Func<System.Threading.Tasks.Task> loginAsync = async () => await _authService.LoginAsync(request, CancellationToken.None);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(loginAsync);
    }

    [Fact]
    public async System.Threading.Tasks.Task RegisterAsync_ValidRequest_CreateUser()
    {
        // Arrange
        var request = new RegisterRequest(UserConsts.Username, HashedPassword);

        _userRepositoryMock.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        _passwordHasherMock.Hash(request.Password)
            .Returns(HashedPassword);

        // Act
        await _authService.RegisterAsync(request, CancellationToken.None);

        // Assert
        await _userRepositoryMock.Received().AddUserAsync(
            Arg.Is<User>(u => u.Username == request.Username && u.Password == HashedPassword),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async System.Threading.Tasks.Task RegisterAsync_UserExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RegisterRequest(UserConsts.Username, HashedPassword);
        var existingUser = new User(UserConsts.Username, HashedPassword);

        _userRepositoryMock.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns(existingUser);

        Func<System.Threading.Tasks.Task> registerAsync = () => _authService.RegisterAsync(request, CancellationToken.None);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(registerAsync);
    }
}

