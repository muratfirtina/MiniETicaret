using System.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Role;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Persistence.Services;

public class RoleService : IRoleService
{
    readonly RoleManager<AppRole> _roleManager;

    public RoleService(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> CreateRoleAsync(string roleName)
    {
        IdentityResult result = await _roleManager.CreateAsync(new AppRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = roleName
        });
        return result.Succeeded;
    }

    public async Task<bool> DeleteRoleAsync(string roleId)
    {
        AppRole appRole = await _roleManager.FindByIdAsync(roleId);
        IdentityResult result = await _roleManager.DeleteAsync(appRole);
        return result.Succeeded;
    }

    public async Task<bool> UpdateRoleAsync(string roleId, string roleName)
    {
        AppRole role = await _roleManager.FindByIdAsync(roleId);
        role.Name = roleName;
        IdentityResult result = await _roleManager.UpdateAsync(role);
        return result.Succeeded;
    }

    public async Task<ListRoleDto> GetRolesAsync(int page, int size)
    {
        var query = _roleManager.Roles;
        IQueryable<AppRole>? rolesQuery;
        if (page != -1 && size != -1)
            rolesQuery = query.Skip(page * size).Take(size);
        else
        {
            rolesQuery = query;
        }

        return await Task.FromResult(new ListRoleDto()
        {
            TotalCount = query.Count(),
            Roles = rolesQuery.Select(x => new RoleDto()
            {
                RoleId = x.Id,
                RoleName = x.Name
            }).ToList()
        });
    }


    public async Task<(string roleId, string roleName)> GetRoleByIdAsync(string roleId)
    {
        string role = await _roleManager.GetRoleIdAsync(new AppRole() { Id = roleId });
        return (roleId, role);
    }
}