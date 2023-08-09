using MiniETicaret.Application.DTOs.Role;
using MiniETicaret.Application.DTOs.User;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Application.Abstractions.Services;

public interface IUserService
{
    Task<CreateUserResponse> CreateAsync(CreateUser model);
    Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDateTime, int refreshTokenLifetime);
    Task UpdateForgotPasswordAsync(string userId, string resetToken, string newPassword);
    Task<ListUserDto> GetAllUsersAsync(int page, int size);
    Task AssignRoleToUserAsync(string userId, List<RoleDto> roles);
    public Task<List<RoleDto>> GetRolesToUserAsync(string userId);
}