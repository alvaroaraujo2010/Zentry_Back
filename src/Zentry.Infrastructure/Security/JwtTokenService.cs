using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Zentry.Application.DTOs.Auth;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;

namespace Zentry.Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public JwtTokenService(IConfiguration configuration) { _configuration = configuration; }

    public string CreateAccessToken(User user, string roleCode)
    {
        var secret = _configuration["Jwt:AccessSecret"];
        if (string.IsNullOrWhiteSpace(secret))
            throw new Exception("Jwt:AccessSecret no configurado.");

        var issuer = _configuration["Jwt:Issuer"] ?? "Zentry";
        var audience = _configuration["Jwt:Audience"] ?? "Zentry.Client";
        var expiresMinutes = int.TryParse(_configuration["Jwt:AccessExpiresMinutes"], out var mins) ? mins : 15;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new("tenant_id", user.TenantId.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, roleCode)
        };
        if (user.BranchId.HasValue) claims.Add(new Claim("branch_id", user.BranchId.Value.ToString()));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(expiresMinutes), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CreateRefreshToken() => Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Convert.ToBase64String(Guid.NewGuid().ToByteArray());

    public CurrentUserDto ToCurrentUserDto(User user, string roleCode) => new() { UserId = user.Id, TenantId = user.TenantId, BranchId = user.BranchId, FullName = user.FullName, Email = user.Email, RoleCode = roleCode };
}
