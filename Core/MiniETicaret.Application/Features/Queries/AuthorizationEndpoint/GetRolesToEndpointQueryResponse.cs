using MiniETicaret.Application.DTOs.Role;

namespace MiniETicaret.Application.Features.Queries.AuthorizationEndpoint;

public class GetRolesToEndpointQueryResponse
{
    public List<RoleDto> Roles { get; set; }
}