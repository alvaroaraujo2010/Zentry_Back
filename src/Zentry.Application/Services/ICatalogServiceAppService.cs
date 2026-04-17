using Zentry.Application.Common;
using Zentry.Application.DTOs.Services;

namespace Zentry.Application.Services;
public interface ICatalogServiceAppService { Task<ApiResponse<List<CatalogServiceDto>>> ListAsync(CancellationToken cancellationToken = default); Task<ApiResponse<CatalogServiceDto>> CreateAsync(CreateCatalogServiceRequest request, CancellationToken cancellationToken = default); Task<ApiResponse<CatalogServiceDto>> UpdateAsync(Guid id, UpdateCatalogServiceRequest request, CancellationToken cancellationToken = default); }
