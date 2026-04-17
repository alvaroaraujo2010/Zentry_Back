using Zentry.Application.Common;
using Zentry.Application.DTOs.Branches;

namespace Zentry.Application.Services;

public interface IBranchAppService
{
    Task<ApiResponse<List<BranchDto>>> ListAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<BranchDto>> CreateAsync(CreateBranchRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<BranchDto>> UpdateAsync(Guid id, UpdateBranchRequest request, CancellationToken cancellationToken = default);
}
