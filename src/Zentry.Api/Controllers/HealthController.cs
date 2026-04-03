using Microsoft.AspNetCore.Mvc;

namespace Zentry.Api.Controllers;
[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase { [HttpGet] public IActionResult Get() => Ok(new { ok = true, message = "Zentry API running" }); }
