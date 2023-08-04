using MediatR;

namespace MiniETicaret.Application.Features.Commands.Role.DeleteRole;

public class DeleteRoleCommandRequest: IRequest<DeleteRoleCommandResponse>
{
    public string RoleId { get; set; }
}