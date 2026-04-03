using Zentry.Application.Common;
using Zentry.Application.DTOs.StaffProfiles;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class StaffProfileAppService : IStaffProfileAppService
{
    private readonly IStaffProfileRepository _staffProfiles;
    private readonly IUserRepository _users;

    public StaffProfileAppService(
        IStaffProfileRepository staffProfiles,
        IUserRepository users)
    {
        _staffProfiles = staffProfiles;
        _users = users;
    }

    public async Task<ApiResponse<List<StaffProfileDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _staffProfiles.ListAsync(cancellationToken);

        var result = items.Select(x => new StaffProfileDto
        {
            UserId = x.UserId,
            TenantId = x.TenantId,
            BranchId = x.BranchId,
            Title = x.Title,
            CommissionRate = x.CommissionRate,
            CanTakeAppointments = x.CanTakeAppointments
        }).ToList();

        return ApiResponse<List<StaffProfileDto>>.Success(result);
    }

    public async Task<ApiResponse<StaffProfileDto>> CreateAsync(CreateStaffProfileRequest request, CancellationToken cancellationToken = default)
    {
        if (request.UserId == Guid.Empty)
            return ApiResponse<StaffProfileDto>.Fail("El userId es obligatorio.");

        if (request.TenantId == Guid.Empty)
            return ApiResponse<StaffProfileDto>.Fail("El tenantId es obligatorio.");

        if (request.CommissionRate < 0)
            return ApiResponse<StaffProfileDto>.Fail("La comisión no puede ser negativa.");

        var user = await _users.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return ApiResponse<StaffProfileDto>.Fail("El usuario no existe.");

        var existing = await _staffProfiles.GetByUserIdAsync(request.UserId, cancellationToken);
        if (existing is not null)
            return ApiResponse<StaffProfileDto>.Fail("Ya existe un perfil staff para este usuario.");

        var entity = new StaffProfile
        {
            UserId = request.UserId,
            TenantId = request.TenantId,
            BranchId = request.BranchId,
            Title = request.Title,
            CommissionRate = request.CommissionRate,
            CanTakeAppointments = request.CanTakeAppointments,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _staffProfiles.AddAsync(entity, cancellationToken);
        await _staffProfiles.SaveChangesAsync(cancellationToken);

        return ApiResponse<StaffProfileDto>.Success(new StaffProfileDto
        {
            UserId = entity.UserId,
            TenantId = entity.TenantId,
            BranchId = entity.BranchId,
            Title = entity.Title,
            CommissionRate = entity.CommissionRate,
            CanTakeAppointments = entity.CanTakeAppointments
        }, "Perfil staff creado");
    }
}