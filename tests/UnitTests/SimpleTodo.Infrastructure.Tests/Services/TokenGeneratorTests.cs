using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using SimpleTodo.Domain.Options;
using SimpleTodo.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TestCommon.Factory;

namespace SimpleTodo.Infrastructure.Tests.Services;

public class TokenGeneratorTests
{
    private readonly TimeProvider _mockTimeProvider;
    private readonly DateTime _fixedDateTime;

    public TokenGeneratorTests()
    {
        _mockTimeProvider = Substitute.For<TimeProvider>();

        var now = DateTimeOffset.UtcNow;
        _fixedDateTime = now.UtcDateTime;
        _mockTimeProvider.GetUtcNow().Returns(now);
    }

    [Fact]
    public void Constructor_ValidParameters_CreateInstance()
    {
        // Arrange
        var mockOptions = Options.Create(TokenOptionsFactory.CreateTokenOptions());

        // Act
        var service = new TokenGenerator(mockOptions, _mockTimeProvider);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Constructor_OptionsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IOptions<TokenOptions> options = null!;
        var instantiateService = () => new TokenGenerator(options, _mockTimeProvider);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(instantiateService);
    }

    [Fact]
    public void Constructor_OptionsValueNull_ThrowsInvalidOperationException()
    {
        // Arrange
        IOptions<TokenOptions> options = Options.Create<TokenOptions>(null!);
        var instantiateService = () => new TokenGenerator(options, _mockTimeProvider);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(instantiateService);
    }

    [Fact]
    public void Generate_ValidParameters_GenerateToken()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var options = TokenOptionsFactory.CreateTokenOptions();
        var service = new TokenGenerator(Options.Create(options), _mockTimeProvider);

        // Act
        var token = service.Generate(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Equal(options.Issuer, jwt.Issuer);
        Assert.Contains(options.Audience, jwt.Audiences);

        var mappedNameId = JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[ClaimTypes.NameIdentifier];
        var mappedName = JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[ClaimTypes.Name];

        Assert.Contains(jwt.Claims, c => c.Type == mappedNameId && c.Value == user.Id.ToString());

        Assert.Equal(SecurityAlgorithms.HmacSha256, jwt.Header.Alg);
    }

    [Fact]
    public void Generate_ValidOptions_SetsCorrectExpiration()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        var expiresInMinutes = 10;
        var options = TokenOptionsFactory.CreateTokenOptions(expiresInMinutes: expiresInMinutes);
        var validTo = _fixedDateTime.AddMinutes(expiresInMinutes);

        var service = new TokenGenerator(Options.Create(options), _mockTimeProvider);

        // Act
        var token = service.Generate(user);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        // Assert
        Assert.Equal(jwt.ValidTo, validTo, TimeSpan.FromSeconds(1));
    }
}

