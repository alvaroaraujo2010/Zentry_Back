using Microsoft.AspNetCore.Mvc;
using Zentry.Application.DTOs.Users;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserAppService _service;

    public UsersController(IUserAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> List()
        => Ok(await _service.ListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var result = await _service.CreateAsync(request);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}