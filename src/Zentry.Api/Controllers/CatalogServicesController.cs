using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zentry.Application.DTOs.Services;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;
[ApiController]
//[Authorize]
[Route("api/catalog-services")]
public class CatalogServicesController : ControllerBase
{
    private readonly ICatalogServiceAppService _service;
    public CatalogServicesController(ICatalogServiceAppService service) { _service = service; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) => Ok(await _service.ListAsync(cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCatalogServiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}
