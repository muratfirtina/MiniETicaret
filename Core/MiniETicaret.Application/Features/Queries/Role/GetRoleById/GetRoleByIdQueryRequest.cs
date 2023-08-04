using MediatR;

namespace MiniETicaret.Application.Features.Queries.Role.GetRoleById;

public class GetRoleByIdQueryRequest:IRequest<GetRoleByIdQueryResponse>
{
    public string RoleId { get; set; }
}