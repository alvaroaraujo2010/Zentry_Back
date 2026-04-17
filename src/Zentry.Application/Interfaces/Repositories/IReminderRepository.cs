using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;
public interface IReminderRepository : IRepository<ReminderQueue>
{
    Task<List<ReminderQueue>> ListByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
