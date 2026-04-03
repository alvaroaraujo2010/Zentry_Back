using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class CatalogServiceRepository : EfRepository<CatalogService>, ICatalogServiceRepository
{
    public CatalogServiceRepository(ZentryDbContext context) : base(context) { }
}
