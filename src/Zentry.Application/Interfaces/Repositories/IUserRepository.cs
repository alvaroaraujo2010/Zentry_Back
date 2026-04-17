using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<List<User>> ListAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}