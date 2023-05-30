using MediatR;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs;

namespace MiniETicaret.Application.Features.Commands.AppUser.RefreshTokenLogin;

public class RefreshTokenLoginCommandHandler: IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>
{
    readonly IAuthService _authService;

    public RefreshTokenLoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<RefreshTokenLoginCommandResponse> Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
    {
        Token token = await _authService.RefreshTokenLoginAsync(request.RefreshToken);
        return new RefreshTokenLoginCommandResponse
        {
            Token = token
        };
    }
}