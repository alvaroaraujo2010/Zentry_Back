using Zentry.Application.Common;
using Zentry.Application.DTOs.Roles;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class RoleAppService : IRoleAppService
{
    private readonly IRoleRepository _roles;

    public RoleAppService(IRoleRepository roles)
    {
        _roles = roles;
    }

    public async Task<ApiResponse<List<RoleDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _roles.ListAsync(cancellationToken);

        return ApiResponse<List<RoleDto>>.Success(items.Select(x => new RoleDto
        {
            Id = x.Id,
            TenantId = x.TenantId,
            Code = x.Code,
            Name = x.Name,
            Description = x.Description,
            IsSystem = x.IsSystem
        }).ToList());
    }

    public async Task<ApiResponse<RoleDto>> CreateAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
    {
        if (request.TenantId == Guid.Empty)
            return ApiResponse<RoleDto>.Fail("El tenantId es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Code))
            return ApiResponse<RoleDto>.Fail("El código es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Name))
            return ApiResponse<RoleDto>.Fail("El nombre es obligatorio.");

        var existing = await _roles.GetByCodeAsync(request.TenantId, request.Code.Trim(), cancellationToken);
        if (existing is not null)
            return ApiResponse<RoleDto>.Fail("Ya existe un rol con ese código.");

        var entity = new Role
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            Code = request.Code.Trim().ToUpperInvariant(),
            Name = request.Name.Trim(),
            Description = request.Description,
            IsSystem = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _roles.AddAsync(entity, cancellationToken);
        await _roles.SaveChangesAsync(cancellationToken);

        return ApiResponse<RoleDto>.Success(new RoleDto
        {
            Id = entity.Id,
            TenantId = entity.TenantId,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsSystem = entity.IsSystem
        }, "Rol creado");
    }
}