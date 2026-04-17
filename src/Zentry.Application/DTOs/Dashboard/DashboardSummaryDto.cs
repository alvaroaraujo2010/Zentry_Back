namespace Zentry.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public string TenantName { get; set; } = string.Empty;
    public string Timezone { get; set; } = "UTC";
    public DateTime BusinessDate { get; set; }
    public int CustomersCount { get; set; }
    public int AppointmentsToday { get; set; }
    public decimal RevenueToday { get; set; }
    public int PendingInvoices { get; set; }
    public int ActiveMemberships { get; set; }
    public int PendingReminders { get; set; }
    public int OpenCashSessions { get; set; }
    public List<DashboardUpcomingAppointmentDto> UpcomingAppointments { get; set; } = new();
    public List<DashboardAlertDto> Alerts { get; set; } = new();
}
