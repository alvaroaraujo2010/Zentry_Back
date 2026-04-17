using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;
public interface ICashSessionRepository : IRepository<CashSession>
{
    Task<CashSession?> GetOpenSessionAsync(Guid tenantId, Guid branchId, CancellationToken cancellationToken = default);
    Task<List<CashSession>> ListByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
