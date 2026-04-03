using Microsoft.EntityFrameworkCore;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class StaffProfileRepository : IStaffProfileRepository
{
    private readonly ZentryDbContext _context;

    public StaffProfileRepository(ZentryDbContext context)
    {
        _context = context;
    }

    public async Task<List<StaffProfile>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StaffProfiles
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<StaffProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.StaffProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(StaffProfile entity, CancellationToken cancellationToken = default)
    {
        await _context.StaffProfiles.AddAsync(entity, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}