using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Queries.Role.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRolesQueryResponse>
{
    readonly IRoleService _roleService;

    public GetRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<GetRolesQueryResponse> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
    {
        var (datas,count) = _roleService.GetRolesAsync(request.Page, request.Size);
        return new()
        {
            Datas = datas,
            TotalCount = count
        };
    }
}