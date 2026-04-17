using Microsoft.AspNetCore.Mvc;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
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
