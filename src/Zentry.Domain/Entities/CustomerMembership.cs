using Zentry.Domain.Common;
using Zentry.Domain.Enums;

namespace Zentry.Domain.Entities;

public class CustomerMembership : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid MembershipId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public int? RemainingVisits { get; set; }
    public bool AutoRenew { get; set; }
}
