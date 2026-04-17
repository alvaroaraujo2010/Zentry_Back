using Microsoft.EntityFrameworkCore;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Domain.Enums;
using Zentry.Infrastructure.Persistence;

namespace Zentry.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ZentryDbContext _context;

    public UserRepository(ZentryDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> ListAsync(CancellationToken cancellationToken = default)
    {
        var users = await _context.Users
            .Include(x => x.RoleEntity)
            .OrderBy(x => x.FullName)
            .ToListAsync(cancellationToken);

        foreach (var user in users)
            MapRole(user);

        return users;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .Include(x => x.RoleEntity)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (user is null)
            return null;

        MapRole(user);
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .Include(x => x.RoleEntity)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user is null)
            return null;

        MapRole(user);
        return user;
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(entity, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    private static void MapRole(User user)
    {
        if (user.RoleEntity?.Code is null)
            return;

        user.Role = user.RoleEntity.Code.ToUpperInvariant() switch
        {
            "OWNER" => UserRole.Owner,
            "ADMIN" => UserRole.Admin,
            "STAFF" => UserRole.Staff,
            "CASHIER" => UserRole.Cashier,
            _ => UserRole.Owner
        };
    }
}