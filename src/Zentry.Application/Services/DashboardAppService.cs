using Zentry.Application.Common;
using Zentry.Application.DTOs.Dashboard;
using Zentry.Application.Interfaces.Repositories;
using Zentry.Application.Interfaces.Security;

namespace Zentry.Application.Services;

public class DashboardAppService : IDashboardAppService
{
    private readonly IDashboardRepository _dashboard;
    private readonly ICurrentUserService _current;

    public DashboardAppService(IDashboardRepository dashboard, ICurrentUserService current)
    {
        _dashboard = dashboard;
        _current = current;
    }

    public async Task<ApiResponse<DashboardSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        if (!_current.TenantId.HasValue)
        {
            return ApiResponse<DashboardSummaryDto>.Fail("Tenant no encontrado.");
        }

        var summary = await _dashboard.GetSummaryAsync(_current.TenantId.Value, cancellationToken);
        if (summary is null)
        {
            return ApiResponse<DashboardSummaryDto>.Fail("No fue posible cargar el dashboard.");
        }

        summary.Alerts = BuildAlerts(summary);
        return ApiResponse<DashboardSummaryDto>.Success(summary, "Dashboard cargado");
    }

    private static List<DashboardAlertDto> BuildAlerts(DashboardSummaryDto summary)
    {
        var alerts = new List<DashboardAlertDto>();

        if (summary.OpenCashSessions == 0)
        {
            alerts.Add(new DashboardAlertDto { Level = "warning", Message = "No hay una caja abierta en este momento." });
        }

        if (summary.PendingInvoices > 0)
        {
            alerts.Add(new DashboardAlertDto { Level = "warning", Message = $"Tienes {summary.PendingInvoices} factura(s) con saldo pendiente." });
        }

        if (summary.PendingReminders > 0)
        {
            alerts.Add(new DashboardAlertDto { Level = "info", Message = $"Hay {summary.PendingReminders} recordatorio(s) pendientes por procesar." });
        }

        if (alerts.Count == 0)
        {
            alerts.Add(new DashboardAlertDto { Level = "success", Message = "Todo en orden por ahora. No hay alertas criticas." });
        }

        return alerts;
    }
}
