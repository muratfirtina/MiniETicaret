using MediatR;

namespace MiniETicaret.Application.Features.Queries.AppUser.GetRolesToUser;

public class GetRolesToUserQueryRequest: IRequest<GetRolesToUserQueryResponse>
{
    public string UserId { get; set; }
}