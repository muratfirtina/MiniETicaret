using MediatR;
using MiniETicaret.Application.DTOs.Role;

namespace MiniETicaret.Application.Features.Commands.AuthorizationEndpoint.AssignRoleToEndpoint;

public class AssignRoleToEndpointCommandRequest: IRequest<AssignRoleToEndpointCommandResponse>
{
    public List<RoleDto> Roles { get; set; }
    public string Code { get; set; }
    public string Menu { get; set; }
    public Type? Type { get; set; }
}