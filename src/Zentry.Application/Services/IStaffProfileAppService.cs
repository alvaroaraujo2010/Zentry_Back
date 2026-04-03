using Zentry.Application.Common;
using Zentry.Application.DTOs.StaffProfiles;

namespace Zentry.Application.Services;

public interface IStaffProfileAppService
{
    Task<ApiResponse<List<StaffProfileDto>>> ListAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<StaffProfileDto>> CreateAsync(CreateStaffProfileRequest request, CancellationToken cancellationToken = default);
}