using Moq;
using Zentry.Application.DTOs.Auth;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Application.Services;
using Zentry.Domain.Entities;

namespace Zentry.Tests.Application;

public class AuthAppServiceTests
{
    [Fact]
    public async Task LoginAsync_ReturnsTokensAndCurrentUser_WhenCredentialsAreValid()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            Email = "alvaroaraujo3@gmail.com",
            PasswordHash = "stored-hash",
            FullName = "Alvaro Araujo",
            RoleEntity = new Role { Code = "ADMIN" }
        };

        var users = new Mock<IUserRepository>();
        users.Setup(x => x.GetByEmailAsync("alvaroaraujo3@gmail.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var refreshTokens = new Mock<IRefreshTokenRepository>();
        refreshTokens.Setup(x => x.GetActiveByUserAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((RefreshToken?)null);
        refreshTokens.Setup(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        refreshTokens.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var passwordHasher = new Mock<IPasswordHasher>();
        passwordHasher.Setup(x => x.Verify("123456", "stored-hash")).Returns(true);
        passwordHasher.Setup(x => x.Hash("refresh-token")).Returns("refresh-hash");

        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(x => x.CreateRefreshToken()).Returns("refresh-token");
        tokenService.Setup(x => x.CreateAccessToken(user, "ADMIN")).Returns("access-token");
        tokenService.Setup(x => x.ToCurrentUserDto(user, "ADMIN")).Returns(new CurrentUserDto
        {
            UserId = user.Id,
            TenantId = user.TenantId,
            Email = user.Email,
            FullName = user.FullName,
            RoleCode = "ADMIN"
        });

        var sut = new AuthAppService(users.Object, refreshTokens.Object, passwordHasher.Object, tokenService.Object);

        var result = await sut.LoginAsync(new LoginRequest
        {
            Email = "  AlvaroAraujo3@gmail.com ",
            Password = "123456"
        });

        Assert.True(result.Ok);
        Assert.NotNull(result.Data);
        Assert.Equal("access-token", result.Data!.AccessToken);
        Assert.Equal("refresh-token", result.Data.RefreshToken);
        Assert.Equal("ADMIN", result.Data.User.RoleCode);

        refreshTokens.Verify(
            x => x.AddAsync(
                It.Is<RefreshToken>(token => token.UserId == user.Id && token.TokenHash == "refresh-hash"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
