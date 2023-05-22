using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniETicaret.Application.Abstractions.Token;
using MiniETicaret.Application.DTOs;

namespace MiniETicaret.Application.Features.Commands.AppUser.GoogleLogin;

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
{
    readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    readonly ITokenHandler _tokenHandler;

    public GoogleLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request,
        CancellationToken cancellationToken)
    {
        var settins = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience =
                new List<string>() { "542377755542-uace25t7dirvb9rp86o9isajmvj4c2ml.apps.googleusercontent.com" },
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settins);
        var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
        Domain.Entities.Identity.AppUser user =
            await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        bool result = user != null;
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = payload.Email,
                    UserName = payload.Email,
                    NameSurname = payload.Name,
                };
                IdentityResult identityResult = await _userManager.CreateAsync(user);
                result = identityResult.Succeeded;
            }
        }

        if (result)
            await _userManager.AddLoginAsync(user, info);
        else
            throw new Exception("Invalid external authentication.");
        
        Token token = _tokenHandler.CreateAccessToken(5);
        
        return new()
        {
            Token = token
        };
        
    }
}