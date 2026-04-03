using Microsoft.EntityFrameworkCore;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class RefreshTokenRepository : EfRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ZentryDbContext context) : base(context) { }

    public async Task<RefreshToken?> GetActiveByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => await Context.RefreshTokens.Where(x => x.UserId == userId && x.RevokedAt == null && x.ExpiresAt > DateTime.UtcNow).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(cancellationToken);
}
