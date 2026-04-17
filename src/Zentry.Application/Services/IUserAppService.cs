using Zentry.Application.Common;
using Zentry.Application.DTOs.Users;

namespace Zentry.Application.Services;

public interface IUserAppService
{
    Task<ApiResponse<List<UserDto>>> ListAsync();
    Task<ApiResponse<UserDto>> CreateAsync(CreateUserRequest request);
    Task<ApiResponse<UserDto>> UpdateAsync(Guid id, UpdateUserRequest request);
}