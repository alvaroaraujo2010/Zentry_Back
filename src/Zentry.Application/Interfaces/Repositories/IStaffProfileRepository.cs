using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;

public interface IStaffProfileRepository
{
    Task<List<StaffProfile>> ListAsync(CancellationToken cancellationToken = default);
    Task<StaffProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(StaffProfile entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}