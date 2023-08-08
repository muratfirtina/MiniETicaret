using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.AuthorizationEndpoint.AssignRoleToEndpoint;

public class AssignRoleToEndpointCommandHandler: IRequestHandler<AssignRoleToEndpointCommandRequest, AssignRoleToEndpointCommandResponse>
{
    readonly IAuthorizationEndpointService _authorizationEndpointService;

    public AssignRoleToEndpointCommandHandler(IAuthorizationEndpointService authorizationEndpointService)
    {
        _authorizationEndpointService = authorizationEndpointService;
    }

    public async Task<AssignRoleToEndpointCommandResponse> Handle(AssignRoleToEndpointCommandRequest request, CancellationToken cancellationToken)
    {
        await _authorizationEndpointService.AssignRoleToEndpointAsync(request.Roles ,request.Menu, request.Code, request.Type);
        return new AssignRoleToEndpointCommandResponse()
        {

        };

    }
}