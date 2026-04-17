using Microsoft.EntityFrameworkCore;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class CashSessionRepository : EfRepository<CashSession>, ICashSessionRepository
{
    public CashSessionRepository(ZentryDbContext context) : base(context) { }

    public async Task<CashSession?> GetOpenSessionAsync(Guid tenantId, Guid branchId, CancellationToken cancellationToken = default)
    {
        return await Context.CashSessions
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.BranchId == branchId && x.Status == "OPEN", cancellationToken);
    }

    public async Task<List<CashSession>> ListByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await Context.CashSessions
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.OpenedAt)
            .ToListAsync(cancellationToken);
    }
}
