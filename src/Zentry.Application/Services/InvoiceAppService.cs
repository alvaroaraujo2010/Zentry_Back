using Zentry.Application.Common; using Zentry.Application.DTOs.Invoices; using Zentry.Domain.Entities;
namespace Zentry.Application.Services;
public class InvoiceAppService : IInvoiceAppService {
 public Task<ApiResponse<InvoiceDto>> CreateAsync(CreateInvoiceRequest r){
  var dto=new InvoiceDto{Id=Guid.NewGuid(),Total=r.Total,Status="CREATED"};
  return Task.FromResult(ApiResponse<InvoiceDto>.Success(dto));
 }}