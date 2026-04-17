using Zentry.Application.Common;
using Zentry.Application.DTOs.Auth;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class AuthAppService : IAuthAppService
{
    private readonly IUserRepository _users;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthAppService(IUserRepository users, IRefreshTokenRepository refreshTokens, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _users = users;
        _refreshTokens = refreshTokens;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByEmailAsync(request.Email.Trim().ToLowerInvariant(), cancellationToken);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            return ApiResponse<AuthResponse>.Fail("Credenciales inválidas.");

        var roleCode = user.RoleEntity?.Code ?? "OWNER";

        var currentRefresh = await _refreshTokens.GetActiveByUserAsync(user.Id, cancellationToken);
        if (currentRefresh is not null)
        {
            currentRefresh.RevokedAt = DateTime.UtcNow;
            await _refreshTokens.SaveChangesAsync(cancellationToken);
        }

        var refresh = _tokenService.CreateRefreshToken();
        await _refreshTokens.AddAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = _passwordHasher.Hash(refresh),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);
        await _refreshTokens.SaveChangesAsync(cancellationToken);

        return ApiResponse<AuthResponse>.Success(new AuthResponse
        {
            AccessToken = _tokenService.CreateAccessToken(user, roleCode),
            RefreshToken = refresh,
            User = _tokenService.ToCurrentUserDto(user, roleCode)
        }, "Login correcto");
    }
}
