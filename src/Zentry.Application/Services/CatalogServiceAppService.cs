using Zentry.Application.Common;
using Zentry.Application.DTOs.Services;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;
using Zentry.Domain.Entities;

namespace Zentry.Application.Services;

public class CatalogServiceAppService : ICatalogServiceAppService
{
    private readonly ICatalogServiceRepository _services;
    private readonly ICurrentUserService _current;
    public CatalogServiceAppService(ICatalogServiceRepository services, ICurrentUserService current) { _services = services; _current = current; }

    public async Task<ApiResponse<List<CatalogServiceDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _services.ListAsync(cancellationToken);
        return ApiResponse<List<CatalogServiceDto>>.Success(items.Select(x => new CatalogServiceDto { Id = x.Id, BranchId = x.BranchId, Code = x.Code, Name = x.Name, Description = x.Description, DurationMinutes = x.DurationMinutes, Price = x.Price, Category = x.Category, IsActive = x.IsActive }).ToList());
    }

    public async Task<ApiResponse<CatalogServiceDto>> CreateAsync(CreateCatalogServiceRequest request, CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue) return ApiResponse<CatalogServiceDto>.Fail("Tenant no encontrado.");
        if (string.IsNullOrWhiteSpace(request.Name)) return ApiResponse<CatalogServiceDto>.Fail("El nombre del servicio es obligatorio.");
        if (request.DurationMinutes <= 0) return ApiResponse<CatalogServiceDto>.Fail("La duración debe ser mayor a 0.");
        if (request.Price < 0) return ApiResponse<CatalogServiceDto>.Fail("El precio no puede ser negativo.");
        var entity = new CatalogService { Id = Guid.NewGuid(), TenantId = _current.TenantId.Value, BranchId = request.BranchId ?? _current.BranchId, Code = string.IsNullOrWhiteSpace(request.Code) ? null : request.Code.Trim(), Name = request.Name.Trim(), Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(), DurationMinutes = request.DurationMinutes, Price = request.Price, Category = string.IsNullOrWhiteSpace(request.Category) ? null : request.Category.Trim(), RequiresBooking = true, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        await _services.AddAsync(entity, cancellationToken);
        await _services.SaveChangesAsync(cancellationToken);
        return ApiResponse<CatalogServiceDto>.Success(new CatalogServiceDto { Id = entity.Id, BranchId = entity.BranchId, Code = entity.Code, Name = entity.Name, Description = entity.Description, DurationMinutes = entity.DurationMinutes, Price = entity.Price, Category = entity.Category, IsActive = entity.IsActive }, "Servicio creado");
    }

    public async Task<ApiResponse<CatalogServiceDto>> UpdateAsync(Guid id, UpdateCatalogServiceRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _services.GetByIdAsync(id, cancellationToken);
        if (entity is null) return ApiResponse<CatalogServiceDto>.Fail("Servicio no encontrado.");
        if (string.IsNullOrWhiteSpace(request.Name)) return ApiResponse<CatalogServiceDto>.Fail("El nombre del servicio es obligatorio.");
        if (request.DurationMinutes <= 0) return ApiResponse<CatalogServiceDto>.Fail("La duración debe ser mayor a 0.");
        if (request.Price < 0) return ApiResponse<CatalogServiceDto>.Fail("El precio no puede ser negativo.");

        entity.BranchId = request.BranchId;
        entity.Code = string.IsNullOrWhiteSpace(request.Code) ? null : request.Code.Trim();
        entity.Name = request.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        entity.DurationMinutes = request.DurationMinutes;
        entity.Price = request.Price;
        entity.Category = string.IsNullOrWhiteSpace(request.Category) ? null : request.Category.Trim();
        entity.UpdatedAt = DateTime.UtcNow;

        await _services.SaveChangesAsync(cancellationToken);

        return ApiResponse<CatalogServiceDto>.Success(new CatalogServiceDto { Id = entity.Id, BranchId = entity.BranchId, Code = entity.Code, Name = entity.Name, Description = entity.Description, DurationMinutes = entity.DurationMinutes, Price = entity.Price, Category = entity.Category, IsActive = entity.IsActive }, "Servicio actualizado");
    }
}
