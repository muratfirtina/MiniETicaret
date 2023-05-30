using MiniETicaret.Application.DTOs.User;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Application.Abstractions.Services;

public interface IUserService
{
    Task<CreateUserResponse> CreateAsync(CreateUser model);
    Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDateTime, int refreshTokenLifetime);
}