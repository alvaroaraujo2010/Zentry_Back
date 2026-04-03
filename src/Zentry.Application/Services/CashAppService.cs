using Zentry.Application.Common; using Zentry.Application.DTOs.Cash;
namespace Zentry.Application.Services;
public class CashAppService:ICashAppService{
 public Task<ApiResponse<CashSessionDto>> OpenAsync(CreateCashSessionRequest r){
  return Task.FromResult(ApiResponse<CashSessionDto>.Success(new CashSessionDto{Id=Guid.NewGuid(),OpeningAmount=r.OpeningAmount,Status="OPEN"}));
 }}