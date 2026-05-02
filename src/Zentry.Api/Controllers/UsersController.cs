using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zentry.Api.Security;
using Zentry.Application.DTOs.Users;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;

[ApiController]
[Authorize(Roles = RoleGroups.Management)]
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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return result.Ok ? Ok(result) : BadRequest(result);
    }
}
