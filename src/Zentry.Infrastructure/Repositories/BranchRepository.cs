using Microsoft.EntityFrameworkCore;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly ZentryDbContext _context;

    public BranchRepository(ZentryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Branch>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Branches
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Branch?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken = default)
    {
        return await _context.Branches
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Code == code, cancellationToken);
    }

    public async Task AddAsync(Branch entity, CancellationToken cancellationToken = default)
    {
        await _context.Branches.AddAsync(entity, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}