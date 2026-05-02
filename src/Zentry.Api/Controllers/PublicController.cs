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
    private readonly ICatalogServiceAppService _catalogServices;

    public PublicController(ITenantAppService tenants, ICatalogServiceAppService catalogServices)
    {
        _tenants = tenants;
        _catalogServices = catalogServices;
    }

    [HttpGet("business")]
    public async Task<IActionResult> Business([FromQuery] string? code, CancellationToken cancellationToken)
        => Ok(await _tenants.GetPublicBrandingAsync(code, cancellationToken));

    [HttpGet("services")]
    public async Task<IActionResult> Services([FromQuery] string? code, CancellationToken cancellationToken)
        => Ok(await _catalogServices.ListPublicAsync(code, cancellationToken));
}
