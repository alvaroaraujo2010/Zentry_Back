namespace Zentry.Application.DTOs.Reminders;
public class CreateReminderRequest { public Guid TenantId { get; set; } public string Phone { get; set; } = ""; }