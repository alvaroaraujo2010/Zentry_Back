using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zentry.Api.Security;
using Zentry.Application.DTOs.Invoices;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Authorize(Roles = RoleGroups.Finance)]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceAppService _s;

    public InvoicesController(IInvoiceAppService s)
    {
        _s = s;
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
        => Ok(await _s.ListAsync(cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceRequest r, CancellationToken cancellationToken)
    {
        var result = await _s.CreateAsync(r, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}
