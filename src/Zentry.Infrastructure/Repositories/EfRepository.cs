using Microsoft.EntityFrameworkCore;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ZentryDbContext Context;
    protected readonly DbSet<TEntity> Set;
    public EfRepository(ZentryDbContext context) { Context = context; Set = context.Set<TEntity>(); }
    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await Set.FindAsync([id], cancellationToken);
    public virtual async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default) => await Set.AsNoTracking().ToListAsync(cancellationToken);
    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) => await Set.AddAsync(entity, cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Context.SaveChangesAsync(cancellationToken);
}
