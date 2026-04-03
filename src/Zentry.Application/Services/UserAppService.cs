using Zentry.Application.Common;
using Zentry.Application.DTOs.Users;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class UserAppService : IUserAppService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;

    public UserAppService(IUserRepository users, IPasswordHasher hasher)
    {
        _users = users;
        _hasher = hasher;
    }

    public async Task<ApiResponse<List<UserDto>>> ListAsync()
    {
        var users = await _users.ListAsync();

        return ApiResponse<List<UserDto>>.Success(
            users.Select(x => new UserDto
            {
                Id = x.Id,
                Email = x.Email,
                FullName = x.FullName,
                Role = x.RoleEntity?.Code ?? "UNKNOWN"
            }).ToList()
        );
    }

    public async Task<ApiResponse<UserDto>> CreateAsync(CreateUserRequest request)
    {
        var entity = new User
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            RoleId = request.RoleId,
            BranchId = request.BranchId,
            Email = request.Email.ToLower().Trim(),
            PasswordHash = _hasher.Hash(request.Password),
            FullName = request.FullName.Trim(),
            Status = "ACTIVE",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _users.AddAsync(entity);
        await _users.SaveChangesAsync();

        return ApiResponse<UserDto>.Success(new UserDto
        {
            Id = entity.Id,
            Email = entity.Email,
            FullName = entity.FullName,
            Role = "CREATED"
        });
    }
}