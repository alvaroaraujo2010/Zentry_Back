namespace Zentry.Application.DTOs.Appointments;
public class CreateAppointmentRequest { public Guid BranchId { get; set; } public Guid CustomerId { get; set; } public Guid? StaffUserId { get; set; } public DateTime StartsAt { get; set; } public DateTime EndsAt { get; set; } public string? Notes { get; set; } }
