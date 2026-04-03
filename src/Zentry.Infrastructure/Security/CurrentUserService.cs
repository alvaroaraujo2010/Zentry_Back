using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Zentry.Application.Interfaces.Security;

namespace Zentry.Infrastructure.Security;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;
    public CurrentUserService(IHttpContextAccessor http) { _http = http; }
    private ClaimsPrincipal? User => _http.HttpContext?.User;
    public Guid? UserId => Guid.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;
    public Guid? TenantId => Guid.TryParse(User?.FindFirst("tenant_id")?.Value, out var id) ? id : null;
    public Guid? BranchId => Guid.TryParse(User?.FindFirst("branch_id")?.Value, out var id) ? id : null;
    public string? RoleCode => User?.FindFirst(ClaimTypes.Role)?.Value;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
}
