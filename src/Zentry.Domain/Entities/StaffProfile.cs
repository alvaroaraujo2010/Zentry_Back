using Zentry.Domain.Common;

namespace Zentry.Domain.Entities;

public class StaffProfile
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Title { get; set; }
    public decimal CommissionRate { get; set; }
    public bool CanTakeAppointments { get; set; } = true;
    public string? WorkScheduleJson { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
