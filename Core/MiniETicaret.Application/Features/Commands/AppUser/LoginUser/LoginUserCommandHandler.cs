using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Abstractions.Token;
using MiniETicaret.Application.DTOs;
using MiniETicaret.Application.Exceptions;

namespace MiniETicaret.Application.Features.Commands.AppUser.LoginUser;

public class LoginUserCommandHandler: IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
{
    readonly IAuthService _authService;

    public LoginUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
        var token = await _authService.LoginAsync(request.UsernameOrEmail, request.Password, 15);
        return new LoginUserSuccessCommandResponse() { Token = token };
    }
}