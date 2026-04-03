using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<List<Role>> ListAsync(CancellationToken cancellationToken = default);
    Task<Role?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken = default);
    Task AddAsync(Role entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}