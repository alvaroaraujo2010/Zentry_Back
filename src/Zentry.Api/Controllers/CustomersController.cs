using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zentry.Application.DTOs.Customers;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;
[ApiController]
//[Authorize]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerAppService _service;
    public CustomersController(ICustomerAppService service) { _service = service; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) => Ok(await _service.ListAsync(cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}
