using MiniETicaret.Application.DTOs.Role;

namespace MiniETicaret.Application.Features.Queries.Role.GetRoles;

public class GetRolesQueryResponse
{
    public object Roles { get; set; }
    public int TotalCount { get; set; }
}