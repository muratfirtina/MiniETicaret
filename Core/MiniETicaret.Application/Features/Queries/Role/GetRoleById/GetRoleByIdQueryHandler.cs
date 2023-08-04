using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Queries.Role.GetRoleById;

public class GetRoleByIdQueryHandler: IRequestHandler<GetRoleByIdQueryRequest, GetRoleByIdQueryResponse>
{
    readonly IRoleService _roleService;

    public GetRoleByIdQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<GetRoleByIdQueryResponse> Handle(GetRoleByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var data = await _roleService.GetRoleByIdAsync(request.RoleId);
        return new GetRoleByIdQueryResponse
        {
            RoleId = data.roleId,
            RoleName = data.roleName
        };
    }
}