namespace Zentry.Application.DTOs.Dashboard;

public class DashboardUpcomingAppointmentDto
{
    public Guid AppointmentId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Total { get; set; }
}
