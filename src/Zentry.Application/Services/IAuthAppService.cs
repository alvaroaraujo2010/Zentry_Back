using Zentry.Application.Common;
using Zentry.Application.DTOs.Auth;

namespace Zentry.Application.Services;
public interface IAuthAppService { Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default); }
