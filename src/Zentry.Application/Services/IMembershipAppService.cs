using Zentry.Application.Common; using Zentry.Application.DTOs.Memberships;
namespace Zentry.Application.Services;
public interface IMembershipAppService { Task<ApiResponse<MembershipDto>> CreateAsync(CreateMembershipRequest r); }