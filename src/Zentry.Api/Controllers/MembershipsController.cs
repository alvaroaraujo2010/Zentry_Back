using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zentry.Api.Security;
using Zentry.Application.DTOs.Memberships;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Authorize(Roles = RoleGroups.TenantUsers)]
[Route("api/memberships")]
public class MembershipsController : ControllerBase
{
    private readonly IMembershipAppService _s;

    public MembershipsController(IMembershipAppService s)
    {
        _s = s;
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
        => Ok(await _s.ListAsync(cancellationToken));

    [HttpPost]
    [Authorize(Roles = RoleGroups.Management)]
    public async Task<IActionResult> Create([FromBody] CreateMembershipRequest r, CancellationToken cancellationToken)
    {
        var result = await _s.CreateAsync(r, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = RoleGroups.Management)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMembershipRequest r, CancellationToken cancellationToken)
    {
        var result = await _s.UpdateAsync(id, r, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}
