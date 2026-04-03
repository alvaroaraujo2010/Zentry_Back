namespace Zentry.Application.Interfaces.Security;
public interface ICurrentUserService { Guid? UserId { get; } Guid? TenantId { get; } Guid? BranchId { get; } string? RoleCode { get; } bool IsAuthenticated { get; } }
