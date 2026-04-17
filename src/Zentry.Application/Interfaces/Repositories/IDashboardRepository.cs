using Zentry.Application.DTOs.Dashboard;

namespace Zentry.Application.Interfaces.Repositories;

public interface IDashboardRepository
{
    Task<DashboardSummaryDto?> GetSummaryAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
