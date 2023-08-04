using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Role.CreateRole;

public class CreateRoleCommandHandler: IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandResponse>
{
    IRoleService _roleService;

    public CreateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<CreateRoleCommandResponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var result= await _roleService.CreateRoleAsync(request.RoleName);
        return new CreateRoleCommandResponse()
        {
            Succeeded = result
        };
    }
}