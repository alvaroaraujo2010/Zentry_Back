using Microsoft.AspNetCore.Mvc;
using Zentry.Application.DTOs.Cash;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Route("api/cash")]
public class CashController : ControllerBase
{
    private readonly ICashAppService _s;

    public CashController(ICashAppService s)
    {
        _s = s;
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
        => Ok(await _s.ListAsync(cancellationToken));

    [HttpGet("current")]
    public async Task<IActionResult> Current(CancellationToken cancellationToken)
        => Ok(await _s.CurrentAsync(cancellationToken));

    [HttpPost("open")]
    public async Task<IActionResult> Open([FromBody] CreateCashSessionRequest r, CancellationToken cancellationToken)
    {
        var result = await _s.OpenAsync(r, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id, [FromBody] CloseCashSessionRequest r, CancellationToken cancellationToken)
    {
        var result = await _s.CloseAsync(id, r, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}
