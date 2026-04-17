using Zentry.Application.Common;
using Zentry.Application.DTOs.Memberships;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class MembershipAppService : IMembershipAppService
{
    private readonly IMembershipRepository _memberships;
    private readonly ICurrentUserService _current;

    public MembershipAppService(IMembershipRepository memberships, ICurrentUserService current)
    {
        _memberships = memberships;
        _current = current;
    }

    public async Task<ApiResponse<List<MembershipDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _memberships.ListAsync(cancellationToken);
        return ApiResponse<List<MembershipDto>>.Success(items.Select(x => new MembershipDto
        {
            Id = x.Id,
            BranchId = x.BranchId,
            Code = x.Code,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            BillingCycleDays = x.BillingCycleDays,
            VisitsPerCycle = x.VisitsPerCycle,
            IsActive = x.IsActive
        }).ToList());
    }

    public async Task<ApiResponse<MembershipDto>> CreateAsync(CreateMembershipRequest r, CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue && r.TenantId == Guid.Empty)
            return ApiResponse<MembershipDto>.Fail("Tenant no encontrado.");
        if (string.IsNullOrWhiteSpace(r.Name))
            return ApiResponse<MembershipDto>.Fail("El nombre de la membresia es obligatorio.");
        if (r.Price < 0)
            return ApiResponse<MembershipDto>.Fail("El precio no puede ser negativo.");
        if (r.BillingCycleDays <= 0)
            return ApiResponse<MembershipDto>.Fail("El ciclo de facturacion debe ser mayor a 0.");
        if (r.VisitsPerCycle.HasValue && r.VisitsPerCycle.Value <= 0)
            return ApiResponse<MembershipDto>.Fail("Las visitas por ciclo deben ser mayores a 0.");

        var tenantId = _current.TenantId ?? r.TenantId;
        var entity = new Membership
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            BranchId = r.BranchId,
            Code = string.IsNullOrWhiteSpace(r.Code) ? null : r.Code.Trim(),
            Name = r.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(r.Description) ? null : r.Description.Trim(),
            Price = r.Price,
            BillingCycleDays = r.BillingCycleDays,
            VisitsPerCycle = r.VisitsPerCycle,
            IsActive = r.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _memberships.AddAsync(entity, cancellationToken);
        await _memberships.SaveChangesAsync(cancellationToken);

        return ApiResponse<MembershipDto>.Success(new MembershipDto
        {
            Id = entity.Id,
            BranchId = entity.BranchId,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            BillingCycleDays = entity.BillingCycleDays,
            VisitsPerCycle = entity.VisitsPerCycle,
            IsActive = entity.IsActive
        }, "Membresia creada");
    }

    public async Task<ApiResponse<MembershipDto>> UpdateAsync(Guid id, UpdateMembershipRequest r, CancellationToken cancellationToken = default)
    {
        var entity = await _memberships.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResponse<MembershipDto>.Fail("Membresia no encontrada.");
        if (string.IsNullOrWhiteSpace(r.Name))
            return ApiResponse<MembershipDto>.Fail("El nombre de la membresia es obligatorio.");
        if (r.Price < 0)
            return ApiResponse<MembershipDto>.Fail("El precio no puede ser negativo.");
        if (r.BillingCycleDays <= 0)
            return ApiResponse<MembershipDto>.Fail("El ciclo de facturacion debe ser mayor a 0.");
        if (r.VisitsPerCycle.HasValue && r.VisitsPerCycle.Value <= 0)
            return ApiResponse<MembershipDto>.Fail("Las visitas por ciclo deben ser mayores a 0.");

        entity.BranchId = r.BranchId;
        entity.Code = string.IsNullOrWhiteSpace(r.Code) ? null : r.Code.Trim();
        entity.Name = r.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(r.Description) ? null : r.Description.Trim();
        entity.Price = r.Price;
        entity.BillingCycleDays = r.BillingCycleDays;
        entity.VisitsPerCycle = r.VisitsPerCycle;
        entity.IsActive = r.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;

        await _memberships.SaveChangesAsync(cancellationToken);

        return ApiResponse<MembershipDto>.Success(new MembershipDto
        {
            Id = entity.Id,
            BranchId = entity.BranchId,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            BillingCycleDays = entity.BillingCycleDays,
            VisitsPerCycle = entity.VisitsPerCycle,
            IsActive = entity.IsActive
        }, "Membresia actualizada");
    }
}
