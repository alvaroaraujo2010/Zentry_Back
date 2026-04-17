using Zentry.Application.Common;
using Zentry.Application.DTOs.Dashboard;

namespace Zentry.Application.Services;

public interface IDashboardAppService
{
    Task<ApiResponse<DashboardSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken = default);
}
