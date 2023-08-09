using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Queries.AppUser.GetAllUsers;

public class GetAllUsersQueryHandler:IRequestHandler<GetAllUsersQueryRequest,GetAllUsersQueryResponse>
{
    readonly IUserService _userService;

    public GetAllUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
    {
        var datas = await _userService.GetAllUsersAsync(request.Page, request.Size);
        return new()
        {
            Users = datas.Users,
            TotalCount = datas.TotalCount
        };
    }
}