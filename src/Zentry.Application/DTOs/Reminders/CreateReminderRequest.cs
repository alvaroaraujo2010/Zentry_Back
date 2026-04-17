namespace Zentry.Application.DTOs.Reminders;
public class CreateReminderRequest { public Guid TenantId { get; set; } public Guid AppointmentId { get; set; } public Guid CustomerId { get; set; } public string Channel { get; set; } = "WHATSAPP"; public string TemplateCode { get; set; } = "APPOINTMENT_REMINDER"; public string? Phone { get; set; } public DateTime ScheduledFor { get; set; } }
