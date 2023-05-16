using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniETicaret.Application.Exceptions;

namespace MiniETicaret.Application.Features.Commands.AppUser.LoginUser;

public class LoginUserCommandHandler: IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
{
    readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;

    public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
        if (user == null)
            user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
        if (user == null)
            throw new NotFoundUserExceptions("Username or password is wrong! Or email is wrong!");
        
        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (result.Succeeded)//Authentication başarılı
        {
            //.....yetkileri belirleyeceğiz
        }
        return new();
    }
}