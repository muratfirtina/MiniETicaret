using System.Text.Json;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Abstractions.Token;
using MiniETicaret.Application.DTOs;
using MiniETicaret.Application.DTOs.Facebook;
using MiniETicaret.Application.Exceptions;
using MiniETicaret.Application.Features.Commands.AppUser.LoginUser;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Persistence.Services;

public class AuthService : IAuthService
{
    readonly HttpClient _httpClient;
    readonly IConfiguration _configuration;
    readonly UserManager<AppUser> _userManager;
    readonly ITokenHandler _tokenHandler;
    readonly SignInManager<AppUser> _signInManager;
    readonly IUserService _userService;

    public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
        _userService = userService;
        _httpClient = httpClientFactory.CreateClient();
    }
    async Task<Token>CreateUserExternalLoginAsync(AppUser user,string email, string name, int accessTokenLifetime, UserLoginInfo info)
    {
        bool result = user != null;
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    UserName = email,
                    NameSurname = name,
                };
                IdentityResult identityResult = await _userManager.CreateAsync(user);
                result = identityResult.Succeeded;
            }
        }

        if (result)
        {
            await _userManager.AddLoginAsync(user, info);

            Token token = _tokenHandler.CreateAccessToken(accessTokenLifetime, user);
            await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration,refreshTokenLifetime:5);
            return token;
        }
        throw new Exception("Invalid external authentication.");
    }

    public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifetime)
    {
        string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/" +
                                                                      $"access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}" +
                                                                      $"&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}" +
                                                                      $"&grant_type=client_credentials");
        FacebookAccessTokenResponse? facebookAccessTokenResponse 
            = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);
        
        string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?" +
                                                                            $"input_token={authToken}" +
                                                                            $"&access_token={facebookAccessTokenResponse?.AccessToken}");
        
        FacebookUserAccessTokenValidation? validation 
            = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);
        if (validation?.Data.IsValid != null)
        {
            string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name" +
                                                                       $"&access_token={authToken}");
            
            FacebookUserInfoResponse? facebookUserInfo
                = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);
            
            var info = new UserLoginInfo("FACEBOOK",validation.Data.UserId, "FACEBOOK");
            Domain.Entities.Identity.AppUser user =
                await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateUserExternalLoginAsync(user,facebookUserInfo.Email,facebookUserInfo.Name,accessTokenLifetime,info);
        }
        throw new Exception("Invalid external authentication.");
    }
    

    public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifetime)
    {
        var settins = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience =
                new List<string?>() { _configuration["ExternalLoginSettings:Google:Client_ID"] },
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settins);
        var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
        Domain.Entities.Identity.AppUser user =
            await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        
        return await CreateUserExternalLoginAsync(user,payload.Email,payload.Name,accessTokenLifetime,info);

    }


    public async Task<Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifetime)
    {
        var user = await _userManager.FindByNameAsync(userNameOrEmail);
        if (user == null)
            user = await _userManager.FindByEmailAsync(userNameOrEmail);
        if (user == null)
            throw new NotFoundUserExceptions();
        
        var result = await _signInManager.PasswordSignInAsync(user,password, false,false);
        if (result.Succeeded)//Authentication başarılı
        {
            Token token = _tokenHandler.CreateAccessToken(accessTokenLifetime, user);
            await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration,refreshTokenLifetime:15);
            return token;
        }
        throw new AuthenticationErrorException();
    }

    public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
    {
        AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user != null && user?.RefreshTokenEndDateTime > DateTime.UtcNow)
        {
            Token token = _tokenHandler.CreateAccessToken(15, user);
            await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration,refreshTokenLifetime:15);
            return token;
        }
        else
            throw new AuthenticationErrorException();

    }
}