using MediatR;

namespace MiniETicaret.Application.Features.Commands.Role.UpdateRole;

public class UpdateRoleCommandRequest:IRequest<UpdateRoleCommandResponse>
{
    public string RoleId { get; set; }
    public string Name { get; set; }
}