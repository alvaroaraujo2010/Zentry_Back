using Zentry.Application.Common;
using Zentry.Application.DTOs.Tenants;

namespace Zentry.Application.Services;

public interface ITenantAppService
{
    Task<ApiResponse<List<TenantDto>>> ListAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<TenantDto>> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken = default);
}