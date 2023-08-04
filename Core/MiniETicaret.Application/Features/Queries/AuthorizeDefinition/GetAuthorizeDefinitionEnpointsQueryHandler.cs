using MediatR;
using MiniETicaret.Application.Abstractions.Services.Configurations;
using MiniETicaret.Application.DTOs.Configuration;

namespace MiniETicaret.Application.Features.Queries.AuthorizeDefinition;

public class GetAuthorizeDefinitionEnpointsQueryHandler: IRequestHandler<GetAuthorizeDefinitionEnpointsQueryRequest, GetAuthorizeDefinitionEnpointsQueryResponse>
{
    readonly IApplicationService _applicationService;

    public GetAuthorizeDefinitionEnpointsQueryHandler(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    public Task<GetAuthorizeDefinitionEnpointsQueryResponse> Handle(GetAuthorizeDefinitionEnpointsQueryRequest request, CancellationToken cancellationToken)
    {
        var result = _applicationService.GetAuthorizeDefinitionEnpoints(typeof(MenuDto));
        return Task.FromResult(new GetAuthorizeDefinitionEnpointsQueryResponse
        {
            AuthorizeMenu = result
        });
    }
}