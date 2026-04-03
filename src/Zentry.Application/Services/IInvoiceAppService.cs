using Zentry.Application.Common; using Zentry.Application.DTOs.Invoices;
namespace Zentry.Application.Services;
public interface IInvoiceAppService { Task<ApiResponse<InvoiceDto>> CreateAsync(CreateInvoiceRequest request); }