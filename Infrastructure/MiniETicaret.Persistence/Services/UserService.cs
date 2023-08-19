using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Application.Abstractions.Helpers;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Role;
using MiniETicaret.Application.DTOs.User;
using MiniETicaret.Application.Exceptions;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Persistence.Services;

public class UserService : IUserService
{
    readonly UserManager<AppUser> _userManager;
    readonly RoleManager<AppRole> _roleManager;
    readonly IEndpointReadRepository _endpointReadRepository;

    public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        IEndpointReadRepository endpointReadRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _endpointReadRepository = endpointReadRepository;
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUser model)
    {
        IdentityResult result = await _userManager.CreateAsync(new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = model.Email,
            NameSurname = model.NameSurname,
            UserName = model.UserName
        }, model.Password);

        CreateUserResponse response = new CreateUserResponse { IsSuccess = result.Succeeded };
        if (result.Succeeded)
        {
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            await _userManager.AddToRoleAsync(user, "User");

            response.Message = "Created successfully.";
        }
        else
        {
            foreach (var error in result.Errors)
            {
                response.Message += $"{error.Code} - {error.Description}\n";
            }
        }
        return response;
    }


    public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDateTime,
        int refreshTokenLifetime)
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

    public async Task<List<RoleDto>> GetRolesToUserAsync(string userIdOrName)
    {
        
        AppUser user = await _userManager.FindByIdAsync(userIdOrName);
        if (user == null)
            user = await _userManager.FindByNameAsync(userIdOrName);
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

    public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
    {
        var userRoles = await GetRolesToUserAsync(name);
        if (!userRoles.Any())
            return false;
        Endpoint? endpoint = await _endpointReadRepository.Table.Include(e => e.Roles)
            .FirstOrDefaultAsync(e => e.Code == code);

        if (endpoint == null)
            return false;

        var hasRole = false;
        var endpointRoles = endpoint.Roles.Select(r => r.Name).ToList();
        foreach (var userRole in userRoles)
        {
            foreach (var endpointRole in endpointRoles)
                if(userRole.RoleName == endpointRole)
                    return true;
            
        }
        return false;
        
        /*foreach (var userRole in userRoles)
            {
                if (!hasRole)
                {
                    foreach (var endpointRole in endpointRoles)
                    {
                        if (userRole.RoleName == endpointRole)
                        {
                            hasRole = true;
                            break;
                        }
                    }
                }
                else
                    break;
            }
            return hasRole;*/
    }
}