using Microsoft.EntityFrameworkCore;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class UserRepository : EfRepository<User>, IUserRepository
{
    public UserRepository(ZentryDbContext context) : base(context) { }

    public override async Task<List<User>> ListAsync(CancellationToken cancellationToken = default)
        => await Context.Users.Include(x => x.RoleEntity).AsNoTracking().ToListAsync(cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await Context.Users.Include(x => x.RoleEntity).FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public Task<string> GetRoleCodeAsync(User user, CancellationToken cancellationToken = default)
        => Task.FromResult(user.RoleEntity?.Code ?? "OWNER");
}
