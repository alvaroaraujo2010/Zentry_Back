using Microsoft.AspNetCore.Mvc;
using Zentry.Application.DTOs.Roles;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly IRoleAppService _service;

    public RolesController(IRoleAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var result = await _service.ListAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}