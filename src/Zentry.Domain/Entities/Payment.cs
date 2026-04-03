using Zentry.Domain.Common;
using Zentry.Domain.Enums;

namespace Zentry.Domain.Entities;

public class Payment : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid BranchId { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid? AppointmentId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? CashSessionId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? ReferenceCode { get; set; }
    public decimal Amount { get; set; }
    public DateTime ReceivedAt { get; set; }
    public string Status { get; set; } = "PAID";
    public string? Notes { get; set; }
    public Guid? CreatedByUserId { get; set; }
}
