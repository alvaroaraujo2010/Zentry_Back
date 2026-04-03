using Zentry.Application.Common; using Zentry.Application.DTOs.Memberships;
namespace Zentry.Application.Services;
public class MembershipAppService:IMembershipAppService{
 public Task<ApiResponse<MembershipDto>> CreateAsync(CreateMembershipRequest r){
  return Task.FromResult(ApiResponse<MembershipDto>.Success(new MembershipDto{Id=Guid.NewGuid(),Name=r.Name,Price=r.Price}));
 }}