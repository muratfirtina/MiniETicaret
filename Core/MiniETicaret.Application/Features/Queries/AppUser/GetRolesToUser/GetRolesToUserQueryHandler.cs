using MediatR;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Role;

namespace MiniETicaret.Application.Features.Queries.AppUser.GetRolesToUser;

public class GetRolesToUserQueryHandler: IRequestHandler<GetRolesToUserQueryRequest,GetRolesToUserQueryResponse>
{
    readonly IUserService _userService;

    public GetRolesToUserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<GetRolesToUserQueryResponse> Handle(GetRolesToUserQueryRequest request, CancellationToken cancellationToken)
    {
        var userRoles = await _userService.GetRolesToUserAsync(request.UserId);
        return new GetRolesToUserQueryResponse
        {
            UserRoles = userRoles
        };
    }
}