using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.AppUser.FacebookLogin;

public class FacebookLoginCommandHandler: IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
{
    readonly IAuthService _authService;

    public FacebookLoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
    {
        var token = await _authService.FacebookLoginAsync(request.AuthToken,60*60);
        return new() { Token = token };
    }
    
}
