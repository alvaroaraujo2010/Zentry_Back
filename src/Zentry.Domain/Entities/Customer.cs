using Zentry.Domain.Common;

namespace Zentry.Domain.Entities;

public class Customer : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid? BranchId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? Notes { get; set; }
    public string? TagsJson { get; set; }
    public DateTime? LastVisitAt { get; set; }
    public bool IsActive { get; set; } = true;
}
