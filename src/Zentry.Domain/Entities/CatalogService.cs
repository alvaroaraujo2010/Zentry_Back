using Zentry.Domain.Common;

namespace Zentry.Domain.Entities;

public class CatalogService : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public decimal Price { get; set; }
    public string? Category { get; set; }
    public bool RequiresBooking { get; set; } = true;
    public bool IsActive { get; set; } = true;
}
