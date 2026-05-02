using Moq;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Application.Services;
using Zentry.Domain.Entities;

namespace Zentry.Tests.Application;

public class CatalogServiceAppServiceTests
{
    [Fact]
    public async Task ListPublicAsync_UsesFirstActiveTenantAndReturnsMappedServices()
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Code = "ZENTRY",
            Name = "Zentry Demo"
        };

        var catalogServices = new List<CatalogService>
        {
            new()
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                Name = "Corte premium",
                Description = "Corte con asesoria",
                DurationMinutes = 45,
                Price = 35000,
                Category = "Barberia",
                IsActive = true
            }
        };

        var repository = new Mock<ICatalogServiceRepository>();
        repository.Setup(x => x.ListActiveByTenantAsync(tenant.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(catalogServices);

        var tenants = new Mock<ITenantRepository>();
        tenants.Setup(x => x.GetFirstActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tenant);

        var sut = new CatalogServiceAppService(repository.Object, tenants.Object, Mock.Of<ICurrentUserService>());

        var result = await sut.ListPublicAsync(null);

        Assert.True(result.Ok);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data!);
        Assert.Equal("Corte premium", result.Data[0].Name);
        Assert.Equal(35000, result.Data[0].Price);
        Assert.Equal("Barberia", result.Data[0].Category);
    }
}
