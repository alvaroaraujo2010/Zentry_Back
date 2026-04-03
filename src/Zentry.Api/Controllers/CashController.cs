using Microsoft.AspNetCore.Mvc; using Zentry.Application.DTOs.Cash; using Zentry.Application.Services;
namespace Zentry.Api.Controllers;
[ApiController][Route("api/cash")]
public class CashController:ControllerBase{
 private readonly ICashAppService _s; public CashController(ICashAppService s){_s=s;}
 [HttpPost("open")] public async Task<IActionResult> Open(CreateCashSessionRequest r)=>Ok(await _s.OpenAsync(r));
}