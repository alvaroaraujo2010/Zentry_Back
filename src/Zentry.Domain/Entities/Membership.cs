using Zentry.Domain.Common;
using Zentry.Domain.Enums;

namespace Zentry.Domain.Entities;

public class Membership : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int BillingCycleDays { get; set; } = 30;
    public int? VisitsPerCycle { get; set; }
    public bool IsActive { get; set; } = true;
}
