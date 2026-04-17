namespace Zentry.Application.DTOs.Users;

public class UserDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? BranchId { get; set; }
    public string Email { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Role { get; set; } = "";
    public string Status { get; set; } = "";
}