using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zentry.Application.DTOs.Auth;
using Zentry.Application.Services;

namespace Zentry.Api.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthAppService _service;
    public AuthController(IAuthAppService service) { _service = service; }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.LoginAsync(request, cancellationToken);
        return result.Ok ? Ok(result) : Unauthorized(result);
    }
}
