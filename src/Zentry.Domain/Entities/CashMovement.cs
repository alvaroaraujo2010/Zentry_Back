using Zentry.Domain.Common;

namespace Zentry.Domain.Entities;

public class CashMovement : BaseEntity, ITenantEntity
{
    public Guid CashSessionId { get; set; }
    public Guid TenantId { get; set; }
    public Guid BranchId { get; set; }
    public Guid? PaymentId { get; set; }
    public string MovementType { get; set; } = "INCOME";
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? CreatedByUserId { get; set; }
}
