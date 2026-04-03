using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;

public interface ITenantRepository
{
    Task<List<Tenant>> ListAsync(CancellationToken cancellationToken = default);
    Task<Tenant?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task AddAsync(Tenant entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}