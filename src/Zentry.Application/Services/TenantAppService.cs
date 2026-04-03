using Zentry.Application.Common;
using Zentry.Application.DTOs.Tenants;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Domain.Entities;
using Zentry.Domain.Enums;

namespace Zentry.Application.Services;

public class TenantAppService : ITenantAppService
{
    private readonly ITenantRepository _tenants;

    public TenantAppService(ITenantRepository tenants)
    {
        _tenants = tenants;
    }

    public async Task<ApiResponse<List<TenantDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _tenants.ListAsync(cancellationToken);

        return ApiResponse<List<TenantDto>>.Success(items.Select(x => new TenantDto
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            LegalName = x.LegalName,
            OwnerName = x.OwnerName,
            OwnerEmail = x.OwnerEmail,
            IsActive = x.IsActive
        }).ToList());
    }

    public async Task<ApiResponse<TenantDto>> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
            return ApiResponse<TenantDto>.Fail("El código es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Name))
            return ApiResponse<TenantDto>.Fail("El nombre es obligatorio.");

        var existing = await _tenants.GetByCodeAsync(request.Code.Trim(), cancellationToken);
        if (existing is not null)
            return ApiResponse<TenantDto>.Fail("Ya existe un tenant con ese código.");

        var entity = new Tenant
        {
            Id = Guid.NewGuid(),
            Code = request.Code.Trim().ToUpperInvariant(),
            Name = request.Name.Trim(),
            LegalName = request.LegalName,
            OwnerName = request.OwnerName,
            OwnerEmail = request.OwnerEmail,
            BusinessType = BusinessType.BARBERSHOP,
            PlanCode = PlanCode.BASIC,
            Timezone = "America/Bogota",
            CurrencyCode = "COP",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _tenants.AddAsync(entity, cancellationToken);
        await _tenants.SaveChangesAsync(cancellationToken);

        return ApiResponse<TenantDto>.Success(new TenantDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            LegalName = entity.LegalName,
            OwnerName = entity.OwnerName,
            OwnerEmail = entity.OwnerEmail,
            IsActive = entity.IsActive
        }, "Tenant creado");
    }
}