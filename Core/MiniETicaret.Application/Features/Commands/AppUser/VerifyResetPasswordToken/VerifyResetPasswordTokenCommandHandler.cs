using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.AppUser.VerifyResetPasswordToken;

public class VerifyResetPasswordTokenCommandHandler : IRequestHandler<VerifyResetPasswordTokenCommandRequest, VerifyResetPasswordTokenCommandResponse>
{
    readonly IAuthService _authService;

    public VerifyResetPasswordTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<VerifyResetPasswordTokenCommandResponse> Handle(VerifyResetPasswordTokenCommandRequest request, CancellationToken cancellationToken)
    {
        bool state = await _authService.VerifyResetPasswordTokenAsync(request.UserId, request.ResetToken);
        return new VerifyResetPasswordTokenCommandResponse()
        {
            State = state
        };
    }
}