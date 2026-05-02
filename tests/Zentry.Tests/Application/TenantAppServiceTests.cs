using Moq;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Application.Services;
using Zentry.Domain.Entities;

namespace Zentry.Tests.Application;

public class TenantAppServiceTests
{
    [Fact]
    public async Task GetPublicBrandingAsync_ReturnsMappedBranding_ForNormalizedTenantCode()
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Code = "BARBERIA",
            Name = "Barberia Central",
            SettingsJson = """
            {
              "themePreset": "graphite",
              "publicHeroTitle": "Reserva tu cita",
              "dashboardImageUrl": "/images/dashboard.png"
            }
            """
        };

        var tenants = new Mock<ITenantRepository>();
        tenants.Setup(x => x.GetByCodeAsync("BARBERIA", It.IsAny<CancellationToken>()))
            .ReturnsAsync(tenant);

        var sut = new TenantAppService(tenants.Object, Mock.Of<ICurrentUserService>());

        var result = await sut.GetPublicBrandingAsync(" barberia ");

        Assert.True(result.Ok);
        Assert.NotNull(result.Data);
        Assert.Equal("Barberia Central", result.Data!.BusinessName);
        Assert.Equal("graphite", result.Data.ThemePreset);
        Assert.Equal("Reserva tu cita", result.Data.PublicHeroTitle);
        Assert.Equal("/images/dashboard.png", result.Data.DashboardImageUrl);
    }
}
