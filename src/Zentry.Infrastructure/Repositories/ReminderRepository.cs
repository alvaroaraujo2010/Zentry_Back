using Microsoft.EntityFrameworkCore;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class ReminderRepository : EfRepository<ReminderQueue>, IReminderRepository
{
    public ReminderRepository(ZentryDbContext context) : base(context) { }

    public async Task<List<ReminderQueue>> ListByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await Context.ReminderQueue
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.ScheduledFor)
            .ToListAsync(cancellationToken);
    }
}
