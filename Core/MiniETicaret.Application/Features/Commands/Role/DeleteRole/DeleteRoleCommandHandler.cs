using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Role.DeleteRole;

public class DeleteRoleCommandHandler: IRequestHandler<DeleteRoleCommandRequest, DeleteRoleCommandResponse>
{
    readonly IRoleService _roleService;

    public DeleteRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<DeleteRoleCommandResponse> Handle(DeleteRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await _roleService.DeleteRoleAsync(request.RoleId);
        return new DeleteRoleCommandResponse()
        {
            Succeeded = result
        };
    }
}