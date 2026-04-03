using Zentry.Application.DTOs.Auth;
using Zentry.Domain.Entities;

namespace Zentry.Application.Interfaces.Security;
public interface ITokenService { string CreateAccessToken(User user, string roleCode); string CreateRefreshToken(); CurrentUserDto ToCurrentUserDto(User user, string roleCode); }
