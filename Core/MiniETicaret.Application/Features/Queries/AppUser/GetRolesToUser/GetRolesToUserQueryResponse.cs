using MiniETicaret.Application.DTOs.Role;

namespace MiniETicaret.Application.Features.Queries.AppUser.GetRolesToUser;

public class GetRolesToUserQueryResponse
{
    public List<RoleDto> UserRoles { get; set; }
}