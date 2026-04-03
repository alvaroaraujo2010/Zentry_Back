using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Repositories;
public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetActiveByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
