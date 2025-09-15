using System.IdentityModel.Tokens.Jwt;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.Infrastructure;

/// <summary>
/// Тесты для сервиса JWT токенов
/// </summary>
public class JwtServiceTests
{
    /// <summary>
    /// Проверяет успешную генерацию валидного JWT токена
    /// </summary>
    [Fact]
    public void GenerateToken_ShouldCreateValidToken()
    {
        // Arrange
        var jwtSettings = new JwtSettings
        {
            Secret = "super_secret_key_for_jwt_at_least_32_characters",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };
        
        var mockOptions = new Mock<IOptions<JwtSettings>>();
        mockOptions.Setup(x => x.Value).Returns(jwtSettings);
        
        var jwtService = new JwtService(mockOptions.Object);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com"
        };
        
        // Act
        var token = jwtService.GenerateToken(user);
        
        // Assert
        token.Should().NotBeNullOrEmpty();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        
        jwtToken.Should().NotBeNull();
        jwtToken!.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id.ToString());
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Name && c.Value == user.UserName);
        jwtToken.Issuer.Should().Be(jwtSettings.Issuer);
        jwtToken.Audiences.Should().Contain(jwtSettings.Audience);
    }
    
    /// <summary>
    /// Проверяет корректное извлечение идентификатора пользователя из JWT токена
    /// </summary>
    [Fact]
    public void GetUserIdFromToken_ShouldExtractUserId()
    {
        // Arrange
        var jwtSettings = new JwtSettings
        {
            Secret = "super_secret_key_for_jwt_at_least_32_characters",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };
        
        var mockOptions = new Mock<IOptions<JwtSettings>>();
        mockOptions.Setup(x => x.Value).Returns(jwtSettings);
        
        var jwtService = new JwtService(mockOptions.Object);
        
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            UserName = "testuser",
            Email = "test@example.com"
        };
        
        var token = jwtService.GenerateToken(user);
        
        // Act
        var extractedUserId = jwtService.GetUserIdFromToken(token);
        
        // Assert
        extractedUserId.Should().Be(userId);
    }
}
