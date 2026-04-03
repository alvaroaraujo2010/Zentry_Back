namespace Zentry.Application.DTOs.Users;

public class CreateUserRequest
{
    public Guid TenantId { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? BranchId { get; set; }
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string FullName { get; set; } = "";
}