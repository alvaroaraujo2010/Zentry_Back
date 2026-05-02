using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zentry.Api.Security;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Authorize(Roles = RoleGroups.AdminPortal)]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardAppService _service;

    public DashboardController(IDashboardAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Summary(CancellationToken cancellationToken)
        => Ok(await _service.GetSummaryAsync(cancellationToken));
}
