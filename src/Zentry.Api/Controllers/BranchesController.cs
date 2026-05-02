using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zentry.Api.Security;
using Zentry.Application.DTOs.Branches;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Authorize(Roles = RoleGroups.TenantUsers)]
[Route("api/branches")]
public class BranchesController : ControllerBase
{
    private readonly IBranchAppService _service;

    public BranchesController(IBranchAppService service)
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
    [Authorize(Roles = RoleGroups.Management)]
    public async Task<IActionResult> Create([FromBody] CreateBranchRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = RoleGroups.Management)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBranchRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}
