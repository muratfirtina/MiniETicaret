using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniETicaret.Application.Abstractions.Token;
using MiniETicaret.Application.DTOs;
using MiniETicaret.Application.Exceptions;

namespace MiniETicaret.Application.Features.Commands.AppUser.LoginUser;

public class LoginUserCommandHandler: IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
{
    readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
    readonly ITokenHandler _tokenHandler;

    public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
        if (user == null)
            user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
        if (user == null)
            throw new NotFoundUserExceptions();
        
        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (result.Succeeded)//Authentication başarılı
        {
            Token token = _tokenHandler.CreateAccessToken(5);
            return new LoginUserSuccessCommandResponse()
            {
                Token = token
            };
        }
        throw new AuthenticationErrorException();

    }
}