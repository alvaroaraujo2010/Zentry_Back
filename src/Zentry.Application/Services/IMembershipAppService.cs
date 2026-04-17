using Zentry.Application.Common;
using Zentry.Application.DTOs.Memberships;
namespace Zentry.Application.Services;
public interface IMembershipAppService { Task<ApiResponse<List<MembershipDto>>> ListAsync(CancellationToken cancellationToken = default); Task<ApiResponse<MembershipDto>> CreateAsync(CreateMembershipRequest r, CancellationToken cancellationToken = default); Task<ApiResponse<MembershipDto>> UpdateAsync(Guid id, UpdateMembershipRequest r, CancellationToken cancellationToken = default); }
