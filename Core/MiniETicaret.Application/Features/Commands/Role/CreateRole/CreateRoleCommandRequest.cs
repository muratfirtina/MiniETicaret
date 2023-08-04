using MediatR;

namespace MiniETicaret.Application.Features.Commands.Role.CreateRole;

public class CreateRoleCommandRequest: IRequest<CreateRoleCommandResponse>
{
    public string RoleName { get; set; }
}