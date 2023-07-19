using System.Text;
using Microsoft.AspNetCore.Identity;
using MiniETicaret.Application.Abstractions.Helpers;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.User;
using MiniETicaret.Application.Exceptions;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Persistence.Services;

public class UserService:IUserService
{
    readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUser model)
    {
        IdentityResult result = await _userManager.CreateAsync(new ()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    NameSurname = model.NameSurname,
                    UserName = model.UserName
                }, model.Password);
               
               CreateUserResponse response = new(){ IsSuccess = result.Succeeded };
               if (result.Succeeded)
                   response.Message = "User Created Successful."; //todo: dil desteği eklenecek
               else
                   foreach (var error in result.Errors)
                       response.Message += $"{error.Code} - {error.Description}\n";
               return response;
               
    }

    public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDateTime, int refreshTokenLifetime)
    {
        if (user != null)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDateTime = accessTokenDateTime.AddSeconds(refreshTokenLifetime);
            await _userManager.UpdateAsync(user);
        }
        else
            throw new NotFoundUserExceptions();

    }

    public async Task UpdateForgotPasswordAsync(string userId, string resetToken, string newPassword)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            resetToken = resetToken.UrlDecode();
            
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (result.Succeeded)
                await _userManager.UpdateSecurityStampAsync(user);
            else
                throw new ResetPasswordException();
        }
        
    }
}