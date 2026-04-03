using Microsoft.AspNetCore.Mvc; using Zentry.Application.DTOs.Memberships; using Zentry.Application.Services;
namespace Zentry.Api.Controllers;
[ApiController][Route("api/memberships")]
public class MembershipsController:ControllerBase{
 private readonly IMembershipAppService _s; public MembershipsController(IMembershipAppService s){_s=s;}
 [HttpPost] public async Task<IActionResult> Create(CreateMembershipRequest r)=>Ok(await _s.CreateAsync(r));
}