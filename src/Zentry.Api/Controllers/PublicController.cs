using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/public")]
public class PublicController : ControllerBase
{
    private readonly ITenantAppService _tenants;

    public PublicController(ITenantAppService tenants)
    {
        _tenants = tenants;
    }

    [HttpGet("business")]
    public async Task<IActionResult> Business([FromQuery] string? code, CancellationToken cancellationToken)
        => Ok(await _tenants.GetPublicBrandingAsync(code, cancellationToken));
}
