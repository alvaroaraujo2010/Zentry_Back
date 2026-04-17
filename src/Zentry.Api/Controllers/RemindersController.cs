using Microsoft.AspNetCore.Mvc;
using Zentry.Application.DTOs.Reminders;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Route("api/reminders")]
public class RemindersController : ControllerBase
{
    private readonly IReminderAppService _s;

    public RemindersController(IReminderAppService s)
    {
        _s = s;
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
        => Ok(await _s.ListAsync(cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReminderRequest r, CancellationToken cancellationToken)
    {
        var result = await _s.CreateAsync(r, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}
