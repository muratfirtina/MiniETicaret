using MiniETicaret.Application.DTOs.User;

namespace MiniETicaret.Application.Abstractions.Services;

public interface IUserService
{
    Task<CreateUserResponse> CreateAsync(CreateUser model);
}