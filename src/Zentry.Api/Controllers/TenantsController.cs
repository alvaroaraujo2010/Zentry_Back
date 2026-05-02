using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zentry.Api.Security;
using Zentry.Application.DTOs.Tenants;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Route("api/tenants")]
public class TenantsController : ControllerBase
{
    private readonly ITenantAppService _service;

    public TenantsController(ITenantAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = RoleGroups.Management)]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var result = await _service.ListAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("current-branding")]
    [Authorize(Roles = RoleGroups.TenantUsers)]
    public async Task<IActionResult> CurrentBranding(CancellationToken cancellationToken)
    {
        var result = await _service.GetCurrentBrandingAsync(cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }

    [HttpPut("current-branding")]
    [Authorize(Roles = RoleGroups.Management)]
    public async Task<IActionResult> UpdateCurrentBranding([FromBody] UpdateTenantBrandingRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateCurrentBrandingAsync(request, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateTenantRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}
