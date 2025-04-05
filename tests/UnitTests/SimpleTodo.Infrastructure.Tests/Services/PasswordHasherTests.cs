using SimpleTodo.Infrastructure.Services;

namespace SimpleTodo.Infrastructure.Tests.Services;

public class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher;
    private const string ValidPassword = "ValidPassword123";
    private const string WrongPassword = "WrongPassword123";

    public PasswordHasherTests()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void Hash_ValidPassword_ReturnsHashedPassword()
    {
        // Act
        var hashedPassword = _passwordHasher.Hash(ValidPassword);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(hashedPassword));
        Assert.Contains("-", hashedPassword);
    }

    [Fact]
    public void Verify_ValidPassword_ReturnsTrue()
    {
        // Arrange
        var hashedPassword = _passwordHasher.Hash(ValidPassword);

        // Act
        var isValid = _passwordHasher.Verify(ValidPassword, hashedPassword);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Verify_InvalidPassword_ReturnsFalse()
    {
        // Arrange
        var hashedPassword = _passwordHasher.Hash(ValidPassword);

        // Act
        var isValid = _passwordHasher.Verify(WrongPassword, hashedPassword);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Hash_SamePasswordDifferentCalls_ReturnDifferentHashes()
    {
        // Act
        var hash1 = _passwordHasher.Hash(ValidPassword);
        var hash2 = _passwordHasher.Hash(ValidPassword);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Hash_InvalidInput_Throws(string input)
    {
        // Arrange
        Action hash = () => _passwordHasher.Hash(input);
        
        // Act & Assert
        Assert.Throws<ArgumentException>(hash);
    }
}

