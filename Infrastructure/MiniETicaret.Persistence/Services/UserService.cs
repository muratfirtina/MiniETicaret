using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Application.Abstractions.Helpers;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Role;
using MiniETicaret.Application.DTOs.User;
using MiniETicaret.Application.Exceptions;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Persistence.Services;

public class UserService:IUserService
{
    readonly UserManager<AppUser> _userManager;
    readonly RoleManager<AppRole> _roleManager;

    public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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

    public async Task<ListUserDto> GetAllUsersAsync(int page, int size)
    {
        var query = _userManager.Users;
        IQueryable<AppUser>? usersQuery;
        if (page == -1 && size == -1)
        {
            usersQuery = query;
        }
        else
        {
            usersQuery = query.Skip(page * size).Take(size);
        }
        return await Task.FromResult(new ListUserDto()
        {
            TotalCount = query.Count(),
            Users = await usersQuery.Select(x => new UserDto()
            {
                Id = x.Id,
                Email = x.Email,
                NameSurname = x.NameSurname,
                UserName = x.UserName,
                TwoFactorEnabled = x.TwoFactorEnabled
            }).ToListAsync()
            
        });
    }

    public async Task AssignRoleToUserAsync(string userId, List<RoleDto> roles)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, roles.Select(x => x.RoleName).ToList());
        }

    }

    public async Task<List<RoleDto>> GetRolesToUserAsync(string userId)
    {
        //kullanıcıların rollerini getirir.
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return await _roleManager.Roles.Where(x => userRoles.Contains(x.Name)).Select(x => new RoleDto()
            {
                RoleName = x.Name,
                RoleId = x.Id
            }).ToListAsync();
        }
        return new List<RoleDto>();
        
    }
}