using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;
public interface ICatalogServiceRepository : IRepository<CatalogService>
{
    Task<List<CatalogService>> ListActiveByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
