using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<string> GetRoleCodeAsync(User user, CancellationToken cancellationToken = default);
}
