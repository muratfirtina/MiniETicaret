using MediatR;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Exceptions;

namespace MiniETicaret.Application.Features.Commands.AppUser.UpdateForgetPassword;

public class UpdateForgotPasswordCommandHandler: IRequestHandler<UpdateForgotPasswordCommandRequest, UpdateForgotPasswordCommandResponse>
{
    readonly IUserService _userService;

    public UpdateForgotPasswordCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UpdateForgotPasswordCommandResponse> Handle(UpdateForgotPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        if (!request.Password.Equals(request.PasswordConfirm))
        {
            throw new ResetPasswordException("Passwords do not match. Please confirm password.");
        }
        
        await _userService.UpdateForgotPasswordAsync(request.UserId, request.ResetToken, request.Password);
        return new();
    }
}