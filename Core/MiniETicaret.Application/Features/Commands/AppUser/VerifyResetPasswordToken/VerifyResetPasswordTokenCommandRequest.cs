using MediatR;

namespace MiniETicaret.Application.Features.Commands.AppUser.VerifyResetPasswordToken;

public class VerifyResetPasswordTokenCommandRequest : IRequest<VerifyResetPasswordTokenCommandResponse>
{
    public string UserId { get; set; }
    public string ResetToken { get; set; }
}