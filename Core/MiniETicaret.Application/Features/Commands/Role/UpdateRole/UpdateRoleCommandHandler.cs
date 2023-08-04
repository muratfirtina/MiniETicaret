using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Role.UpdateRole;

public class UpdateRoleCommandHandler: IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCommandResponse>
{
    readonly IRoleService _roleService;

    public UpdateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await _roleService.UpdateRoleAsync(request.RoleId, request.Name);
        return new UpdateRoleCommandResponse
        {
            Succeeded = result
        };
        
    }
}