using Zentry.Domain.Common;

namespace Zentry.Domain.Entities;

public class WhatsappLog : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public Guid? CustomerId { get; set; }
    public Guid? AppointmentId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string TemplateCode { get; set; } = string.Empty;
    public string? PayloadJson { get; set; }
    public string? ProviderMessageId { get; set; }
    public string Status { get; set; } = "PENDING";
    public string? ErrorMessage { get; set; }
    public DateTime? SentAt { get; set; }
}