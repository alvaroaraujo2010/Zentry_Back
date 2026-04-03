using Zentry.Domain.Common;

namespace Zentry.Domain.Entities;

public class Subscription : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public string PlanCode { get; set; } = "BASIC";
    public string Status { get; set; } = "TRIAL";
    public DateTime StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
    public DateTime? RenewsAt { get; set; }
    public decimal Amount { get; set; }
    public string? FeaturesJson { get; set; }
}