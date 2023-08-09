using MediatR;
using MiniETicaret.Application.DTOs.Role;

namespace MiniETicaret.Application.Features.Commands.AppUser.AssignRoleToUser;

public class AssignRoleToUserCommandRequest: IRequest<AssignRoleToUserCommandResponse>
{
    public string UserId { get; set; }
    public List<RoleDto> Roles { get; set; }
}