using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;

public interface IBranchRepository
{
    Task<List<Branch>> ListAsync(CancellationToken cancellationToken = default);
    Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Branch?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken = default);
    Task AddAsync(Branch entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
