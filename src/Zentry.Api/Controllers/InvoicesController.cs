using Microsoft.AspNetCore.Mvc; using Zentry.Application.DTOs.Invoices; using Zentry.Application.Services;
namespace Zentry.Api.Controllers;
[ApiController][Route("api/invoices")]
public class InvoicesController:ControllerBase{
 private readonly IInvoiceAppService _s; public InvoicesController(IInvoiceAppService s){_s=s;}
 [HttpPost] public async Task<IActionResult> Create(CreateInvoiceRequest r)=>Ok(await _s.CreateAsync(r));
}