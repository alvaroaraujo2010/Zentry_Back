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
        return ApiResponse<List<CatalogServiceDto>>.Success(items.Select(x => new CatalogServiceDto { Id = x.Id, Code = x.Code, Name = x.Name, DurationMinutes = x.DurationMinutes, Price = x.Price, IsActive = x.IsActive }).ToList());
    }

    public async Task<ApiResponse<CatalogServiceDto>> CreateAsync(CreateCatalogServiceRequest request, CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue) return ApiResponse<CatalogServiceDto>.Fail("Tenant no encontrado.");
        var entity = new CatalogService { Id = Guid.NewGuid(), TenantId = _current.TenantId.Value, BranchId = request.BranchId ?? _current.BranchId, Code = request.Code, Name = request.Name.Trim(), Description = request.Description, DurationMinutes = request.DurationMinutes, Price = request.Price, Category = request.Category, RequiresBooking = true, IsActive = true, CreatedAt = DateTime.UtcNow };
        await _services.AddAsync(entity, cancellationToken);
        await _services.SaveChangesAsync(cancellationToken);
        return ApiResponse<CatalogServiceDto>.Success(new CatalogServiceDto { Id = entity.Id, Code = entity.Code, Name = entity.Name, DurationMinutes = entity.DurationMinutes, Price = entity.Price, IsActive = entity.IsActive }, "Servicio creado");
    }
}
