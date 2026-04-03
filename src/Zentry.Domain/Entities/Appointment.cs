using Zentry.Domain.Common;
using Zentry.Domain.Enums;

namespace Zentry.Domain.Entities;

public class Appointment : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid BranchId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? StaffUserId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.SCHEDULED;
    public string Source { get; set; } = "APP";
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public decimal BalanceDue { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }
}
