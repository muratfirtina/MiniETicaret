using MiniETicaret.Application.Abstractions.Services.Authentication;

namespace MiniETicaret.Application.Abstractions.Services;

public interface IAuthService : IExternalAuthentication, IInternalAuthentication
{
    Task PasswordResetAsync(string email);
    Task<bool> VerifyResetPasswordTokenAsync(string userId, string resetToken);
}