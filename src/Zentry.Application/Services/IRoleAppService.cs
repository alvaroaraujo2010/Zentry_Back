using Zentry.Application.Common;
using Zentry.Application.DTOs.Roles;

namespace Zentry.Application.Services;

public interface IRoleAppService
{
    Task<ApiResponse<List<RoleDto>>> ListAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<RoleDto>> CreateAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<RoleDto>> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken cancellationToken = default);
}
