using Zentry.Domain.Common;

namespace Zentry.Domain.Entities;

public class CashSession : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid BranchId { get; set; }
    public Guid OpenedByUserId { get; set; }
    public Guid? ClosedByUserId { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public decimal OpeningAmount { get; set; }
    public decimal? ClosingAmount { get; set; }
    public decimal ExpectedAmount { get; set; }
    public decimal DifferenceAmount { get; set; }
    public string Status { get; set; } = "OPEN";
    public string? Notes { get; set; }
}
