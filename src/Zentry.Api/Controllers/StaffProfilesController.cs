using Microsoft.AspNetCore.Mvc;
using Zentry.Application.DTOs.StaffProfiles;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Route("api/staff-profiles")]
public class StaffProfilesController : ControllerBase
{
    private readonly IStaffProfileAppService _service;

    public StaffProfilesController(IStaffProfileAppService service)
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
    public async Task<IActionResult> Create([FromBody] CreateStaffProfileRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}