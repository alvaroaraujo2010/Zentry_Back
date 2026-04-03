using Microsoft.AspNetCore.Mvc; using Zentry.Application.DTOs.Reminders; using Zentry.Application.Services;
namespace Zentry.Api.Controllers;
[ApiController][Route("api/reminders")]
public class RemindersController:ControllerBase{
 private readonly IReminderAppService _s; public RemindersController(IReminderAppService s){_s=s;}
 [HttpPost] public async Task<IActionResult> Create(CreateReminderRequest r)=>Ok(await _s.CreateAsync(r));
}