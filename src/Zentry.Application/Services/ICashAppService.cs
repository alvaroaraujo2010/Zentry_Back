using Zentry.Application.Common;
using Zentry.Application.DTOs.Cash;
namespace Zentry.Application.Services;
public interface ICashAppService { Task<ApiResponse<CashSessionDto?>> CurrentAsync(CancellationToken cancellationToken = default); Task<ApiResponse<List<CashSessionDto>>> ListAsync(CancellationToken cancellationToken = default); Task<ApiResponse<CashSessionDto>> OpenAsync(CreateCashSessionRequest r, CancellationToken cancellationToken = default); Task<ApiResponse<CashSessionDto>> CloseAsync(Guid id, CloseCashSessionRequest r, CancellationToken cancellationToken = default); }
