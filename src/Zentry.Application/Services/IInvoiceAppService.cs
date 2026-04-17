using Zentry.Application.Common;
using Zentry.Application.DTOs.Invoices;
namespace Zentry.Application.Services;
public interface IInvoiceAppService { Task<ApiResponse<List<InvoiceDto>>> ListAsync(CancellationToken cancellationToken = default); Task<ApiResponse<InvoiceDto>> CreateAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default); }
