namespace Zentry.Application.DTOs.Appointments;
public class AppointmentDto { public Guid Id { get; set; } public Guid CustomerId { get; set; } public Guid? StaffUserId { get; set; } public DateTime StartsAt { get; set; } public DateTime EndsAt { get; set; } public string Status { get; set; } = string.Empty; public decimal Total { get; set; } }
