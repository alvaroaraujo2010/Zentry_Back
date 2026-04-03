namespace Zentry.Application.DTOs.StaffProfiles;

public class CreateStaffProfileRequest
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Title { get; set; }
    public decimal CommissionRate { get; set; }
    public bool CanTakeAppointments { get; set; } = true;
}