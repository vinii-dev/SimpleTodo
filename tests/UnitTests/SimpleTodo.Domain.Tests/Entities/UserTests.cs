using TestCommon.Consts;
using TestCommon.Factory;

namespace SimpleTodo.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ValidParameters_CreatesUser()
    {
        // Act
        var user = UserFactory.CreateUser(UserConsts.Username, UserConsts.HashedPassword);

        // Assert
        Assert.Equal(UserConsts.Username, user.Username);
        Assert.Equal(UserConsts.HashedPassword, user.Password);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_UsernameWhitespaceOrNull_ThrowsArgumentException(string? username)
    {
        // Arrange
        Action instantiateUser = () => UserFactory.CreateUser(username!, UserConsts.HashedPassword);

        // Act & Assert
        Assert.Throws<ArgumentException>(instantiateUser);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_PasswordWhitespaceOrNull_ThrowsArgumentException(string? hashedPassword)
    {
        // Arrange
        Action instantiateUser = () => UserFactory.CreateUser(UserConsts.Username, hashedPassword!);

        // Act & Assert
        Assert.Throws<ArgumentException>(instantiateUser);
    }
}
