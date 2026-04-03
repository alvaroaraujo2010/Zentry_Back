namespace Zentry.Application.DTOs.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public CurrentUserDto User { get; set; } = new();
}
public class CurrentUserDto
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public Guid? BranchId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RoleCode { get; set; } = "OWNER";
}
