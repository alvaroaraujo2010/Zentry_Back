using Microsoft.EntityFrameworkCore;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class CatalogServiceRepository : EfRepository<CatalogService>, ICatalogServiceRepository
{
    public CatalogServiceRepository(ZentryDbContext context) : base(context) { }

    public async Task<List<CatalogService>> ListActiveByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<CatalogService>()
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.IsActive)
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}
