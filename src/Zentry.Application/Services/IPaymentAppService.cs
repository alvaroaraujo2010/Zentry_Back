using Zentry.Application.Common;
using Zentry.Application.DTOs.Payments;

namespace Zentry.Application.Services;
public interface IPaymentAppService { Task<ApiResponse<List<PaymentDto>>> ListAsync(CancellationToken cancellationToken = default); Task<ApiResponse<PaymentDto>> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default); }
