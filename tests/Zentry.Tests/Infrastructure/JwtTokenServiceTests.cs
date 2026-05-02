using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Security;

namespace Zentry.Tests.Infrastructure;

public class JwtTokenServiceTests
{
    [Fact]
    public void CreateAccessToken_EmbedsRoleTenantAndBranchClaims()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:AccessSecret"] = "super-secret-key-with-32-characters",
                ["Jwt:Issuer"] = "https://zentry.local",
                ["Jwt:Audience"] = "zentry-client",
                ["Jwt:AccessExpiresMinutes"] = "60"
            })
            .Build();

        var user = new User
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Email = "alvaroaraujo3@gmail.com",
            FullName = "Alvaro Araujo"
        };

        var sut = new JwtTokenService(configuration);

        var token = sut.CreateAccessToken(user, "ADMIN");
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Equal("https://zentry.local", jwt.Issuer);
        Assert.Contains("zentry-client", jwt.Audiences);
        Assert.Contains(jwt.Claims, claim => claim.Type == ClaimTypes.Role && claim.Value == "ADMIN");
        Assert.Contains(jwt.Claims, claim => claim.Type == "tenant_id" && claim.Value == user.TenantId.ToString());
        Assert.Contains(jwt.Claims, claim => claim.Type == "branch_id" && claim.Value == user.BranchId.ToString());
    }
}
