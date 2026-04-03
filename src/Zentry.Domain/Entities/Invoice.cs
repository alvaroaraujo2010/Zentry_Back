using Zentry.Domain.Common;
using Zentry.Domain.Enums;

namespace Zentry.Domain.Entities;

public class Invoice : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid BranchId { get; set; }
    public Guid? AppointmentId { get; set; }
    public Guid CustomerId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal BalanceDue { get; set; }
    public string PaymentStatus { get; set; } = "PENDING";
    public DateTime IssuedAt { get; set; }
    public DateTime? DueAt { get; set; }
    public string? Notes { get; set; }
    public Guid? CreatedByUserId { get; set; }
}
