using MediatR;

namespace MiniETicaret.Application.Features.Commands.AppUser.UpdateForgetPassword;

public class UpdateForgotPasswordCommandRequest: IRequest<UpdateForgotPasswordCommandResponse>
{
    public string UserId { get; set; }
    public string ResetToken { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
    
}