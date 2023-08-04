using MiniETicaret.Application.DTOs.Role;

namespace MiniETicaret.Application.Abstractions.Services;

public interface IRoleService
{
    Task<bool> CreateRoleAsync(string roleName);
    Task<bool> DeleteRoleAsync(string roleId);
    Task<bool> UpdateRoleAsync(string roleId, string roleName);
    Task<ListRoleDto> GetRolesAsync(int page,int size);
    Task<(string roleId,string roleName)> GetRoleByIdAsync(string roleId);

}