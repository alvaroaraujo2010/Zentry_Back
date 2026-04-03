using Zentry.Domain.Common;

namespace Zentry.Domain.Entities;

public class Role : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PermissionsJson { get; set; }
    public bool IsSystem { get; set; }
}
