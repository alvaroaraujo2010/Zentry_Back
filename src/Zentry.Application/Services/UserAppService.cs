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
                TenantId = x.TenantId,
                RoleId = x.RoleId,
                BranchId = x.BranchId,
                Email = x.Email,
                FullName = x.FullName,
                Role = x.RoleEntity?.Code ?? "UNKNOWN",
                Status = x.Status
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
            TenantId = entity.TenantId,
            RoleId = entity.RoleId,
            BranchId = entity.BranchId,
            Email = entity.Email,
            FullName = entity.FullName,
            Role = "CREATED",
            Status = entity.Status
        });
    }

    public async Task<ApiResponse<UserDto>> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var entity = await _users.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse<UserDto>.Fail("Usuario no encontrado.");

        if (request.RoleId == Guid.Empty)
            return ApiResponse<UserDto>.Fail("El rol es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.FullName))
            return ApiResponse<UserDto>.Fail("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Email))
            return ApiResponse<UserDto>.Fail("El email es obligatorio.");

        entity.RoleId = request.RoleId;
        entity.BranchId = request.BranchId;
        entity.FullName = request.FullName.Trim();
        entity.Email = request.Email.ToLower().Trim();
        entity.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(request.Password))
            entity.PasswordHash = _hasher.Hash(request.Password);

        await _users.SaveChangesAsync();

        return ApiResponse<UserDto>.Success(new UserDto
        {
            Id = entity.Id,
            TenantId = entity.TenantId,
            RoleId = entity.RoleId,
            BranchId = entity.BranchId,
            Email = entity.Email,
            FullName = entity.FullName,
            Role = entity.RoleEntity?.Code ?? "UPDATED",
            Status = entity.Status
        }, "Usuario actualizado");
    }
}