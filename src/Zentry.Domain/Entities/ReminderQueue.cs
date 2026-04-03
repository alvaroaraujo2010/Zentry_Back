using Zentry.Domain.Common;

namespace Zentry.Domain.Entities;

public class ReminderQueue : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public Guid AppointmentId { get; set; }
    public Guid CustomerId { get; set; }
    public string Channel { get; set; } = "WHATSAPP";
    public string TemplateCode { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime ScheduledFor { get; set; }
    public string Status { get; set; } = "PENDING";
    public int Attempts { get; set; }
    public string? LastError { get; set; }
    public DateTime? SentAt { get; set; }
}