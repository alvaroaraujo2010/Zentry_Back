using Zentry.Application.Common; using Zentry.Application.DTOs.Cash;
namespace Zentry.Application.Services;
public interface ICashAppService { Task<ApiResponse<CashSessionDto>> OpenAsync(CreateCashSessionRequest r); }